using Firebase.Auth;
using Growers.io.Services;
using System.ComponentModel;
using Container = Growers.io.Models.Container;

/*
 * Team: Growers.IO
 * Semester: Winter 2024
 * Date: April 29, 2024
 * Course: Application Development III
 * Description: Data repository for farm containers.
 */

namespace Growers.io.DataRepos
{
    /// <summary>
    /// Static data repo for <see cref="Container"/>(s). 
    /// </summary>
    public class ContainerRepo : INotifyPropertyChanged
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
        /// Constructor for creating a container repo. Creates connection to database.
        /// </summary>
        /// <param name="user">Firebase user to access database with</param>
        /// <param name="databaseUrl">Firebase database connection URL</param>
        public ContainerRepo(User user, string databaseUrl)
        {
            ContainerDb = new DatabaseService<Container>(
                user,
                nameof(Container),
                databaseUrl,
                nameof(Container));
        }

        /// <summary>
        /// Getter for DatabaseService for <see cref="Container"/>(s)
        /// </summary>
        public DatabaseService<Container> ContainerDb { get; private set; }
    }
}
