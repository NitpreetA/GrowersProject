using Growers.io.DataRepos;
using Growers.io.Interfaces;
using Growers.io.Services;
using Newtonsoft.Json;
using System.Runtime.CompilerServices;

/*
 * Team: Growers.IO
 * Semester: Winter 2024
 * Date: April 29, 2024
 * Course: Application Development III
 * Description: Class to represent a farm container, with all sub-system inside.
 */

namespace Growers.io.Models
{
    /// <summary>
    /// Class for farm containers.
    /// </summary>
    public class Container : IHasUKey
    {
        public string Key { get; set; }

        private string _name;
        /// <summary>
        /// Name of container
        /// </summary>
        /// <exception cref="ArgumentException">Thrown when description is empty.</exception>
        public string Name
        {
            get => _name;
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException($"{nameof(Name)} cannot be empty.");
                }
                _name = value;
            }
        }

        private string _deviceId;
        /// <summary>
        /// Device ID for container device
        /// </summary>
        /// <exception cref="ArgumentException">Thrown when device ID is empty.</exception>
        public string DeviceID
        {
            get => _deviceId;
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException($"{nameof(DeviceID)} cannot be empty.");
                }
                _deviceId = value;
            }
        }

        private IoTClientService _clientService;
        /// <summary>
        /// Getter for IoTClientService
        /// </summary>
        [JsonIgnore]
        public IoTClientService ClientService
        {
            get
            {
                _clientService ??= new IoTClientService(App.Settings.IoTHubString, DeviceID);
                return _clientService;
            }
        }

        private Plant _plant;
        /// <summary>
        /// Plant sub-system
        /// </summary>
        [JsonIgnore]
        public Plant Plant
        {
            get
            {
                _plant ??= App.PlantDataRepo.GetSubsystemFromDeviceId(DeviceID);
                return _plant;
            }
        }

        private GeoLocation _geoLocation;
        /// <summary>
        /// Geo-location sub-system
        /// </summary>
        [JsonIgnore]
        public GeoLocation Location
        {
            get
            {
                _geoLocation ??= App.GeoLocationDataRepo.GetSubsystemFromDeviceId(DeviceID);
                return _geoLocation;
            }
        }

        private Security _security;
        /// <summary>
        /// Security sub-system
        /// </summary>
        [JsonIgnore]
        public Security Security
        {
            get
            {
                _security ??= App.SecurityDataRepo.GetSubsystemFromDeviceId(DeviceID);
                return _security;
            }
        }
    }
}

