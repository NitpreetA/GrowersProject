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
 * Description: Interface for CRUD operation for a generic type of data.
 */

namespace Growers.io.Interfaces
{
    /// <summary>
    /// Generic interface for interacting with data in the database.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IDataStore<T>
    {
        /// <summary>
        /// Adds an item to the repository.
        /// </summary>
        /// <param name="item">Item to add</param>
        /// <returns>Task of bool indicating whether the operation was successful or not.</returns>
        Task<bool> AddItemsAsync(T item);

        /// <summary>
        /// Updates an item to the repository.
        /// </summary>
        /// <param name="item">Item to update</param>
        /// <returns>Task of bool indicating whether the operation was successful or not.</returns>
        Task<bool> UpdateItemsAsync(T item);

        /// <summary>
        /// Deletes an item to the repository.
        /// </summary>
        /// <param name="item">Item to delete</param>
        /// <returns>Task of bool indicating whether the operation was successful or not.</returns>
        Task<bool> DeleteItemsAsync(T item);

        /// <summary>
        /// Gets all item within the repository.
        /// </summary>
        /// <param name="forceRefresh">Force refresh repository. Defaults to false.</param>
        /// <returns>Task of collection of collection of items in the repository.</returns>
        Task<IEnumerable<T>> GetItemsAsync(bool forceRefresh = false);


        /// <summary>
        /// Getter for local representation for repository.
        /// </summary>
        /// <returns>Collection of items within the repository.</returns>
        public ObservableCollection<T> Items { get; }
    }
}
