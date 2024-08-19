using Growers.io.Interfaces;
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
 * Description: Class to map a container to a owner, or a farmer.
 */

namespace Growers.io.Models
{
    /// <summary>
    /// Class to map a user to a container. Inherits from IHasUKey.
    /// </summary>
    public class UserToContainer : IHasUKey
    {
        public string Key { get; set; }

        private string _ownerUID;
        /// <summary>
        /// Fleet owner UID tasked with managing container
        /// </summary>
        /// <exception cref="ArgumentException">Thrown when ownerUID is empty.</exception>
        public string OwnerUID 
        {
            get => _ownerUID;
            set
            {
                if(string.IsNullOrEmpty(value)) 
                {
                    throw new ArgumentException("OwnerUID cannot be empty.");
                }
                _ownerUID = value;
            }
        }

        /// <summary>
        /// Farmer UID tasked with managing the farm
        /// </summary>
        public IEnumerable<string> FarmerUIDs { get; set; } = Enumerable.Empty<string>();

        private string _containerKey;
        /// <summary> 
        /// Container key to map to
        /// </summary>
        /// <exception cref="ArgumentException">Thrown when ContainerKey is empty.</exception>
        public string ContainerKey 
        {
            get => _containerKey;
            set
            {
                if(string.IsNullOrEmpty(value)) 
                {
                    throw new ArgumentException("ContainerKey cannot be empty.");
                }
                _containerKey = value;
            }
        }
    }
}
