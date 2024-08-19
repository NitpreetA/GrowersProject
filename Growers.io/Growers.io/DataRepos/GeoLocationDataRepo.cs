using Growers.io.Enums;
using Growers.io.Interfaces;
using Growers.io.Models;
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
 * Description: Data repository for geolocation sub-system data.
 */

namespace Growers.io.DataRepos
{
    /// <summary>
    /// GeoLocationDataRepo is the repository for subsystems and readings. Inherits from <see cref="ISubsystemRepo{T}"/>.
    /// </summary>
    public class GeoLocationDataRepo : ISubsystemRepo<GeoLocation>
    {
        public GeoLocationDataRepo()
        {

        }

        /// <summary>
        /// Getter for dictionary of geolocation subsystems with the deviceId as the key.
        /// </summary>
        public Dictionary<string, GeoLocation> GeoLocationSubsystems { get; private set; } = new();

        public GeoLocation GetSubsystemFromDeviceId(string deviceId)
        {
            if(!GeoLocationSubsystems.ContainsKey(deviceId))
            {
                GeoLocationSubsystems.Add(deviceId, new GeoLocation() {GPSLongitude = GetGPSLongitude(), GPSLatitude = GetGPSLatitude() });
            }
            return GeoLocationSubsystems[deviceId];
        }

        // Gets test GPS longitude values
        private static ObservableCollection<Reading<double>> GetGPSLongitude()
        {
            return new ObservableCollection<Reading<double>>()
            {
                new (){Type = ReadingType.GPSLongitude, Value = -75,Unit = "",TimeStamp = DateTime.Now              },
            };
        }

        // Gets test GPS latitude values
        private static ObservableCollection<Reading<double>> GetGPSLatitude()
        {
            return new ObservableCollection<Reading<double>>()
            {
                new (){Type = ReadingType.GPSLatitude, Value = 45,Unit = "",TimeStamp = DateTime.Now              },
            };
        }

        // Gets test pitch readings
        private static ObservableCollection<Reading<double>> GetPitch()
        {
            return new ObservableCollection<Reading<double>>()
            {
                new (){Type = ReadingType.Pitch, Value = 220,Unit = "pitch", TimeStamp = DateTime.Now   },
                new (){Type = ReadingType.Pitch, Value = 20,Unit = "pitch", TimeStamp = DateTime.Now.AddDays(2)},
                new (){Type = ReadingType.Pitch, Value = 500,Unit = "pitch",TimeStamp = DateTime.Now.AddDays(4)  },
                new (){Type = ReadingType.Pitch, Value = 370,Unit = "pitch",TimeStamp = DateTime.Now.AddDays(6)  },
                new (){Type = ReadingType.Pitch, Value = 825,Unit = "pitch",TimeStamp = DateTime.Now.AddDays(8)  },
            };
        }

        // Gets test yaw readings
        private static ObservableCollection<Reading<double>> GetYaw()
        {
            return new ObservableCollection<Reading<double>>()
            {
                new (){Type = ReadingType.Yaw, Value = 220,Unit = "yaw", TimeStamp = DateTime.Now   },
                new (){Type = ReadingType.Yaw, Value = 20,Unit =  "yaw", TimeStamp = DateTime.Now.AddDays(2)},
                new (){Type = ReadingType.Yaw, Value = 500,Unit = "yaw",TimeStamp = DateTime.Now.AddDays(4)  },
                new (){Type = ReadingType.Yaw, Value = 370,Unit = "yaw",TimeStamp = DateTime.Now.AddDays(6)  },
                new (){Type = ReadingType.Yaw, Value = 825,Unit = "yaw",TimeStamp = DateTime.Now.AddDays(8)  },
            };
        }

        // Gets test roll readings
        private static ObservableCollection<Reading<double>> GetRoll()
        {
            return new ObservableCollection<Reading<double>>()
            {
                new (){Type = ReadingType.Roll, Value = 220,Unit = "roll", TimeStamp = DateTime.Now   },
                new (){Type = ReadingType.Roll, Value = 20,Unit =  "roll", TimeStamp = DateTime.Now.AddDays(2)},
                new (){Type = ReadingType.Roll, Value = 500,Unit = "roll",TimeStamp = DateTime.Now.AddDays(4)  },
                new (){Type = ReadingType.Roll, Value = 370,Unit = "roll",TimeStamp = DateTime.Now.AddDays(6)  },
                new (){Type = ReadingType.Roll, Value = 825,Unit = "roll",TimeStamp = DateTime.Now.AddDays(8)  },
            };
        }
    }
}
