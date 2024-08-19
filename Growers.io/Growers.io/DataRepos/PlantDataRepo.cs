using Growers.io.Enums;
using Growers.io.Interfaces;
using Growers.io.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

/*
 * Team: Growers.IO
 * Semester: Winter 2024
 * Date: April 29, 2024
 * Course: Application Development III
 * Description: Data repository for plant sub-system data.
 */

namespace Growers.io.DataRepos
{
    /// <summary>
    /// PlantDataRepo is the repository for subsystems and readings. Inherits from <see cref="ISubsystemRepo{T}"/>.
    /// </summary>
    public class PlantDataRepo : ISubsystemRepo<Plant>
    {
        public PlantDataRepo()
        {

        }

        /// <summary>
        /// Getter for dictionary of plant subsystems with the deviceId as the key.
        /// </summary>
        public Dictionary<string, Plant> PlantSubsystems { get; private set; } = new();

        public Plant GetSubsystemFromDeviceId(string deviceId)
        {
            if(!PlantSubsystems.ContainsKey(deviceId))
            {
                PlantSubsystems.Add(deviceId, new Plant());
            }
            return PlantSubsystems[deviceId];
        }

        // Gets test temperature readings
        private static ObservableCollection<Reading<double>> GetTestTemperatures()
        {
            return new ObservableCollection<Reading<double>>()
            {
                new() { Type = ReadingType.Temperature, Value = 21, Unit = "°C",TimeStamp = DateTime.Today           },
                new() { Type = ReadingType.Temperature, Value = 24, Unit = "°C",TimeStamp = DateTime.Today.AddDays(2)},
                new() { Type = ReadingType.Temperature, Value = 25, Unit = "°C",TimeStamp = DateTime.Today.AddDays(4)},
                new() { Type = ReadingType.Temperature, Value = 22, Unit = "°C",TimeStamp = DateTime.Today.AddDays(6)},
                new() { Type = ReadingType.Temperature, Value = 26, Unit = "°C",TimeStamp = DateTime.Today.AddDays(8)},
            };
        }

        // Gets test humidity readings
        private static ObservableCollection<Reading<double>> GetTestHumidity()
        {
            return new ObservableCollection<Reading<double>>()
            {
                new() { Type = ReadingType.Humidity, Value = 40, Unit = "%",TimeStamp = DateTime.Today            },
                new() { Type = ReadingType.Humidity, Value = 50, Unit = "%",TimeStamp = DateTime.Today.AddDays(2) },
                new() { Type = ReadingType.Humidity, Value = 45, Unit = "%",TimeStamp = DateTime.Today.AddDays(4) },
                new() { Type = ReadingType.Humidity, Value = 46, Unit = "%",TimeStamp = DateTime.Today.AddDays(6) },
                new() { Type = ReadingType.Humidity, Value = 56, Unit = "%",TimeStamp = DateTime.Today.AddDays(8) },
            };
        }

        // Gets test water level readings
        private static ObservableCollection<Reading<double>> GetTestWaterLevel()
        {
            return new ObservableCollection<Reading<double>>()
            {
                new() { Type = ReadingType.WaterLevel, Value = 0, Unit = "mm"  ,TimeStamp = DateTime.Today            },
                new() { Type = ReadingType.WaterLevel, Value = 2, Unit = "mm"  ,TimeStamp = DateTime.Today.AddDays(2) },
                new() { Type = ReadingType.WaterLevel, Value = 3, Unit = "mm"  ,TimeStamp = DateTime.Today.AddDays(4) },
                new() { Type = ReadingType.WaterLevel, Value = 2, Unit = "mm"  ,TimeStamp = DateTime.Today.AddDays(6) },
                new() { Type = ReadingType.WaterLevel, Value = 4.8, Unit = "mm",TimeStamp = DateTime.Today.AddDays(8) },
            };
        }

        // Gets test soil moisture readings
        private static ObservableCollection<Reading<int>> GetTestSoilMoisture()
        {
            return new ObservableCollection<Reading<int>>()
            {
                new() { Type = ReadingType.SoilMoisture, Value = 620, Unit = string.Empty ,TimeStamp = DateTime.Today           },
                new() { Type = ReadingType.SoilMoisture, Value = 600, Unit = string.Empty ,TimeStamp = DateTime.Today.AddDays(2)},
                new() { Type = ReadingType.SoilMoisture, Value = 602, Unit = string.Empty ,TimeStamp = DateTime.Today.AddDays(4)},
                new() { Type = ReadingType.SoilMoisture, Value = 582, Unit = string.Empty ,TimeStamp = DateTime.Today.AddDays(6)},
                new() { Type = ReadingType.SoilMoisture, Value = 553, Unit = string.Empty ,TimeStamp = DateTime.Today.AddDays(8)},
            };
        }
    }
}
