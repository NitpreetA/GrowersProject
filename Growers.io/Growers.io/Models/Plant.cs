using Azure.Messaging.EventHubs.Consumer;
using Growers.io.DataRepos;
using Growers.io.Enums;
using Growers.io.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

/*
 * Team: Growers.IO
 * Semester: Winter 2024
 * Date: April 29, 2024
 * Course: Application Development III
 * Description: Class to represent the plant sub-system, contains all sensor data and actuator controls.
 */

namespace Growers.io.Models
{
    /// <summary>
    /// Controller and viewer class for plant sub-system.
    /// </summary>
    public class Plant : INotifyPropertyChanged
    {
        /// <summary>
        /// Plant sub-system
        /// </summary>
        public Plant()
        {
            Temperature.CollectionChanged += (_, _) => { NotifyPropertyChanged(nameof(Temperature)); };
            Humidity.CollectionChanged += (_, _) => { NotifyPropertyChanged(nameof(Humidity)); };
            WaterLevel.CollectionChanged += (_, _) => { NotifyPropertyChanged(nameof(WaterLevel)); }; 
            SoilMoisture.CollectionChanged += (_, _) => { NotifyPropertyChanged(nameof(SoilMoisture)); };
        }

        /// <summary>
        /// All temperature readings.
        /// </summary>
        public ObservableCollection<Reading<double>> Temperature { get; set; } = new();
        /// <summary>
        /// All humidity readings.
        /// </summary>
        public ObservableCollection<Reading<double>> Humidity { get; set; } = new();
        /// <summary>
        /// All water level readings
        /// </summary>
        public ObservableCollection<Reading<double>> WaterLevel { get; set; } = new();
        /// <summary>
        /// All soil moisture readings
        /// </summary>
        public ObservableCollection<Reading<int>> SoilMoisture { get; set; } = new();

        /// <summary>
        /// Actuator for fan
        /// </summary>
        public Actuator Fan { get; set; } = new Actuator(CommandType.Fan);

        /// <summary>
        /// Actuator for light
        /// </summary>
        public Actuator Light { get; set; } = new Actuator(CommandType.Led);

        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

