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
 * Description: Class to represent the geo-location sub-system, contains all sensor data and actuator controls.
 */

namespace Growers.io.Models
{
    /// <summary>
    /// Controller and viewer class for geo-location sub-system.
    /// </summary>
    public class GeoLocation : INotifyPropertyChanged
    {
        public GeoLocation()
        {
            GPSLongitude.CollectionChanged += (_, _) => { NotifyPropertyChanged(nameof(GPSLongitude)); };
            GPSLatitude.CollectionChanged += (_, _) => { NotifyPropertyChanged(nameof(GPSLatitude)); };
            Pitch.CollectionChanged += (_, _) => { NotifyPropertyChanged(nameof(Pitch)); };
            Yaw.CollectionChanged += (_, _) => { NotifyPropertyChanged(nameof(Yaw)); };
            Roll.CollectionChanged += (_, _) => { NotifyPropertyChanged(nameof(Roll)); };
            AccelerationX.CollectionChanged += (_, _) => { NotifyPropertyChanged(nameof(AccelerationX)); };
            AccelerationY.CollectionChanged += (_, _) => { NotifyPropertyChanged(nameof(AccelerationY)); };
            AccelerationZ.CollectionChanged += (_, _) => { NotifyPropertyChanged(nameof(AccelerationZ)); };
        }

        /// <summary>
        /// All readings for gps longitude
        /// </summary>
        public ObservableCollection<Reading<double>> GPSLongitude { get; set; } = new();
        /// <summary>
        /// All readings for gps latitude
        /// </summary>
        public ObservableCollection<Reading<double>> GPSLatitude { get; set; } = new();
        /// <summary>
        /// All readings for pitch angle
        /// </summary>
        public ObservableCollection<Reading<double>> Pitch { get; set; } = new();
        /// <summary>
        /// All readings for yaw angle
        /// </summary>
        public ObservableCollection<Reading<double>> Yaw { get; set; } = new();
        /// <summary>
        /// All readings for roll angle
        /// </summary>
        public ObservableCollection<Reading<double>> Roll { get; set; } = new();
        /// <summary>
        /// All readings for acceleration X
        /// </summary>
        public ObservableCollection<Reading<double>> AccelerationX { get; set; } = new();
        /// <summary>
        /// All readings for acceleration Y
        /// </summary>
        public ObservableCollection<Reading<double>> AccelerationY { get; set; } = new();
        /// <summary>
        /// All readings for acceleration Z
        /// </summary>
        public ObservableCollection<Reading<double>> AccelerationZ { get; set; } = new();

        /// <summary>
        /// Actuator for buzzer
        /// </summary>
        public Actuator Buzzer { get; set; } = new Actuator(CommandType.Buzzer);

        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
