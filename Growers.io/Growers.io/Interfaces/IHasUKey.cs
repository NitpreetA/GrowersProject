using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * Team: Growers.IO
 * Semester: Winter 2024
 * Date: April 29, 2024
 * Course: Application Development III
 * Description: Interface for a unique key for a Firebase real-time database.
 */

namespace Growers.io.Interfaces
{
    /// <summary>
    /// Interface for unique key implementation. For use with Firebase real-time database.
    /// </summary>
    public interface IHasUKey
    {
        /// <summary>
        /// Getter/Setter for database key.
        /// </summary>
        public string Key { get; set; }
    }
}
