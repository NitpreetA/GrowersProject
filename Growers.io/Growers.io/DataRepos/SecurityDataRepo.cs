using Growers.io.Models;
using Growers.io.Enums;
using System.Collections.ObjectModel;
using Growers.io.Interfaces;
using Security = Growers.io.Models.Security;

/*
 * Team: Growers.IO
 * Semester: Winter 2024
 * Date: April 29, 2024
 * Course: Application Development III
 * Description: Data repository for security sub-system data.
 */

namespace Growers.io.DataRepos
{
    /// <summary>
    /// SecurityDataRepo is the repository for subsystems and readings. Inherits from <see cref="ISubsystemRepo{T}"/>.
    /// </summary>
    public class SecurityDataRepo : ISubsystemRepo<Models.Security>

    {
        public SecurityDataRepo()
        {

        }

        /// <summary>
        /// Getter for dictionary of security subsystems with the deviceId as the key.
        /// </summary>
        public Dictionary<string, Models.Security> SecuritySubsystems { get; private set; } = new();

        public Models.Security GetSubsystemFromDeviceId(string deviceId)
        {
            if(!SecuritySubsystems.ContainsKey(deviceId))
            {
                SecuritySubsystems.Add(deviceId, new Models.Security());
            }
            return SecuritySubsystems[deviceId];
        }

        // Get test noise level readings
        private static ObservableCollection<Reading<int>> GetNoiseLevel() 
        {
            return new ObservableCollection<Reading<int>>()
            {
                new (){Type = ReadingType.NoiseLevel, Value = 220,Unit = "Noise Ratio",TimeStamp = DateTime.Now             },
                new (){Type = ReadingType.NoiseLevel, Value = 20,Unit = "Noise Ratio", TimeStamp = DateTime.Now.AddDays(2)},
                new (){Type = ReadingType.NoiseLevel, Value = 500,Unit = "Noise Ratio",TimeStamp = DateTime.Now.AddDays(4)  },
                new (){Type = ReadingType.NoiseLevel, Value = 370,Unit = "Noise Ratio",TimeStamp = DateTime.Now.AddDays(6)  },
                new (){Type = ReadingType.NoiseLevel, Value = 825,Unit = "Noise Ratio",TimeStamp = DateTime.Now.AddDays(8)  },
            
            };
        }

        // Get test luminosity readings
        private static ObservableCollection<Reading<int>> GetLuminosityLevel()
        {
            return new ObservableCollection<Reading<int>>()
            {
                new (){Type = ReadingType.Luminosity, Value = 0,Unit = "lx"  ,TimeStamp = DateTime.Now             },
                new (){Type = ReadingType.Luminosity, Value = 20,Unit = "lx" ,TimeStamp = DateTime.Now.AddDays(2) },
                new (){Type = ReadingType.Luminosity, Value = 50,Unit = "lx" ,TimeStamp = DateTime.Now.AddDays(4) },
                new (){Type = ReadingType.Luminosity, Value = 30,Unit = "lx" ,TimeStamp = DateTime.Now.AddDays(6) },
                new (){Type = ReadingType.Luminosity, Value = 525,Unit = "lx",TimeStamp = DateTime.Now.AddDays(8)  },

            };
        }

        // Get test motion readings
        private static ObservableCollection<Reading<int>> GetMotion()
        {
            return new ObservableCollection<Reading<int>>()
            {
                new (){Type = ReadingType.Motion, Value = 0,Unit = "Detection",TimeStamp = DateTime.Now             },
                new (){Type = ReadingType.Motion, Value = 1,Unit = "Detection" ,TimeStamp = DateTime.Now.AddDays(2) },
                new (){Type = ReadingType.Motion, Value = 0,Unit = "Detection",TimeStamp = DateTime.Now.AddDays(4)  },
                new (){Type = ReadingType.Motion, Value = 0,Unit = "Detection" ,TimeStamp = DateTime.Now.AddDays(6) },
                new (){Type = ReadingType.Motion, Value = 1,Unit = "Detection" ,TimeStamp = DateTime.Now.AddDays(8) },

            };
        }

        // Get test door state readings
        private static ObservableCollection<Reading<int>> GetDoorState()
        {
            return new ObservableCollection<Reading<int>>()
            {
                new (){Type = ReadingType.DoorState, Value = 0,Unit = "Is Open",TimeStamp = DateTime.Now             },
                new (){Type = ReadingType.DoorState, Value = 1,Unit = "Is Open", TimeStamp = DateTime.Now.AddDays(2) },
                new (){Type = ReadingType.DoorState, Value = 0,Unit = "Is Open",TimeStamp = DateTime.Now.AddDays(4)  },
                new (){Type = ReadingType.DoorState, Value = 1,Unit = "Is Open", TimeStamp = DateTime.Now.AddDays(6) },
                new (){Type = ReadingType.DoorState, Value = 0,Unit = "IS Open", TimeStamp = DateTime.Now.AddDays(8) },

            };
        }
    }
}
