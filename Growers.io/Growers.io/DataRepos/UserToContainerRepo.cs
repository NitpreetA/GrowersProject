using Firebase.Auth;
using Growers.io.Models;
using Growers.io.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Container = Growers.io.Models.Container;

/*
 * Team: Growers.IO
 * Semester: Winter 2024
 * Date: April 29, 2024
 * Course: Application Development III
 * Description: Data repository user/container mappings.
 */

namespace Growers.io.DataRepos
{
    public class UserToContainerRepo : INotifyPropertyChanged
    {
        /// <summary>
        /// Event from INotifyPropertyChanged
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Notify view when a property is changed
        /// </summary>
        /// <param name="propertyName">Property affected</param>
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Constructor for creating a user/container repo. Creates connection to database.
        /// </summary>
        /// <param name="user">Firebase user to access database with</param>
        /// <param name="databaseUrl">Firebase database connection URL</param>
        public UserToContainerRepo(User user, string databaseUrl)
        {
            MappingRepo = new DatabaseService<UserToContainer>(
                user,
                nameof(UserToContainer),
                databaseUrl,
                nameof(UserToContainer));

            MappingRepo.Items.CollectionChanged += (sender, e) =>
            {
                OnPropertyChanged(nameof(Containers));
            };
        }

        /// <summary>
        /// Getter for DatabaseService for <see cref="UserToContainer"/>(s)
        /// </summary>UserToContainer
        public DatabaseService<UserToContainer> MappingRepo { get; private set ; }

        /// <summary>
        /// Gets all containers for a given user ID.
        /// </summary>
        /// <param name="userUID">User UID to associate containers with.</param>
        /// <returns>A list of relevant containers.</returns>
        public ObservableCollection<Container> GetContainersFromUserUID(string userUID)
        {
            try
            {
                var mappings = MappingRepo.Items.Where(
                    c => c.OwnerUID == userUID ||
                    c.FarmerUIDs.Any(f => f == userUID));

                var list = new ObservableCollection<Container>(App.Containers.ContainerDb.Items.Where(c => mappings.Any(m => m.ContainerKey == c.Key)));
                return list;    
            }
            catch { }
            return new();
        }

        /// <summary>
        /// Gets a list of containers relevant to the currently logged in user.
        /// </summary>
        private ObservableCollection<Container> _containers;
        public ObservableCollection<Container> Containers 
        { 
            get => GetContainersFromUserUID(App.User.UserCreds.User.Uid);
        }
    }
}
