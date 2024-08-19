using Firebase.Auth;
using Growers.io.Services;
using System.ComponentModel;
using UserInfo = Growers.io.Models.UserInfo;

/*
 * Team: Growers.IO
 * Semester: Winter 2024
 * Date: April 29, 2024
 * Course: Application Development III
 * Description: Data repository for user information related to a given Firebase user.
 */

namespace Growers.io.DataRepos
{
    /// <summary>
    /// Data repo for <see cref="UserInfo"/>(s)
    /// </summary>
    public class UserInfoRepo : INotifyPropertyChanged
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
        /// Constructor for creating a UserInfoRepo. Creates connection to database.
        /// </summary>
        /// <param name="user">Firebase user to access database with</param>
        /// <param name="databaseUrl">Firebase database connection URL</param>
        public UserInfoRepo(User user, string databaseUrl)
        {
            UserInfoDb = new DatabaseService<UserInfo>(
                user,
                nameof(UserInfo),
                databaseUrl,
                nameof(UserInfo));
        }

        /// <summary>
        /// Getter for DatabaseService for <see cref="UserInfo"/>(s)
        /// </summary>
        public DatabaseService<UserInfo> UserInfoDb { get; private set; }


        /// <summary>
        /// Getter/setter for user credentials
        /// </summary>
        private UserCredential _urs;
        public UserCredential UserCreds { 
            get { 
                return _urs; 
            }
            set 
            {
                _urs = value;
                OnPropertyChanged(nameof(UserCreds));
            } 
        }
    }
}
