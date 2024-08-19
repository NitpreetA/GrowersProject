using Firebase.Database;
using Firebase.Database.Offline;
using Growers.io.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * Team: Growers.IO
 * Semester: Winter 2024
 * Date: April 29, 2024
 * Course: Application Development III
 * Description: Service for managing CRUD operations and connection to Firebase real-time database.
 */

namespace Growers.io.Services
{
    /// <summary>
    /// Generic implementation of a DatabaseService, implementing the <see cref="IDataStore{T}"/> where T implements <see cref="IHasUKey"/>.
    /// This DatabaseService utilizes Firebase real-time database.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DatabaseService<T> : IDataStore<T> where T : class, IHasUKey
    {
        /// <summary>
        /// Constructor for creating a service to Firebase real-time database.
        /// </summary>
        /// <param name="user">Firebase user to authenticate with.</param>
        /// <param name="path">Path to access in the database.</param>
        /// <param name="BaseUrl">Base path for data in the database.</param>
        /// <param name="customKey">Custom to accessing database.</param>
        public DatabaseService(Firebase.Auth.User user, string path, string BaseUrl, string customKey = "")
        {
            FirebaseOptions options = new FirebaseOptions()
            {
                AuthTokenAsyncFactory = async () => await user.GetIdTokenAsync()
            };
            var client = new FirebaseClient(BaseUrl, options);
            _realtimeDb =
                client.Child(path)
                .AsRealtimeDatabase<T>(customKey, "", StreamingOptions.LatestOnly, InitialPullStrategy.MissingOnly, true);

            client.Child(path)
                .AsObservable<T>()
                .Subscribe(e =>
                {
                    if(!Items.Any(i => i.Key == e.Key))
                    {
                        Items.Add(e.Object);
                    }
                });

            if (_items == null)
                Task.Run(() => LoadItems()).Wait();
        }
        
        // RealTimeDatabase connection.
        private readonly RealtimeDatabase<T> _realtimeDb;

        // Collection of items.
        private ObservableCollection<T> _items;

        /// <summary>
        /// Getter for all items in the database.
        /// </summary>
        public ObservableCollection<T> Items
        {
            get
            {
                if(_items == null)
                    Task.Run(() => LoadItems()).Wait();

                return _items;

            }
        }

        /// <summary>
        /// Loads all items in the local collection of items.
        /// </summary>
        /// <returns>Task indicating when the item loading completed.</returns>
        private async Task LoadItems()
        {
            var result = await GetItemsAsync();
            _items = new ObservableCollection<T>(result);
        }

        /// <summary>
        /// Adds an item to the database.
        /// </summary>
        /// <param name="item">Item to add.</param>
        /// <returns>Task of a boolean indicating whether the operation was successful.</returns>
        public async Task<bool> AddItemsAsync(T item)
        {
            try
            {
                item.Key = _realtimeDb.Post(item);
                _realtimeDb.Put(item.Key, item);

                Items.Add(item);
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return await Task.FromResult(false);
            }
            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemsAsync(T item)
        {
            try
            {
                _realtimeDb.Delete(item.Key);
                Items.Remove(item);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return await Task.FromResult(false);
            }
            return await Task.FromResult(true);
        }
        public async Task<bool> UpdateItemsAsync(T item)
        {
            try
            {
                _realtimeDb.Put(item.Key, item);
                var idx = Items.IndexOf(Items.FirstOrDefault(x => x.Key == item.Key));
                Items[idx] = item;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return await Task.FromResult(false);
            }
            return await Task.FromResult(true);
        }
        public async Task<IEnumerable<T>> GetItemsAsync(bool forceRefresh =false)
        {
            if (_realtimeDb.Database?.Count == 0)
            {
                try
                {
                    
                    await _realtimeDb.PullAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    return null;
                }
            }
            IEnumerable<T> result = _realtimeDb.Once().Select(x => x.Object);
            return await Task.FromResult(result);

        }
    }
}
