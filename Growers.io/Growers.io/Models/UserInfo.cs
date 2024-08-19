using Growers.io.Interfaces;

/*
 * Team: Growers.IO
 * Semester: Winter 2024
 * Date: April 29, 2024
 * Course: Application Development III
 * Description: Class to represent user information, to be used alongside Firebase users.
 */

namespace Growers.io.Models
{
    /// <summary>
    /// Model for UserInfo collection. To be used in addition with Firebase authentication and real-time database.
    /// </summary>
    public class UserInfo : IHasUKey
    {
        public string Key { get; set; }

        /// <summary>
        /// UID to associate info to a Firebase user.
        /// </summary>
        public string UID { get; set; }

        /// <summary>
        /// Boolean to indicate whether a Firebase is a technician or not.
        /// </summary>
        public bool IsTechician { get; set; }

        /// <summary>
        /// Name of Firebase user.
        /// </summary>
        /// <exception cref="ArgumentException">throws when name is null or empty</exception>
        private string _name;
        public string Name
        {
            get { return _name; }
            set 
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentException("Name cannont be null or empty");
                _name = value;
            } 
        }
    }
}
