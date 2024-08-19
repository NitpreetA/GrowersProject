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
 * Description: Class to represent the security sub-system, contains all sensor data and actuator controls.
 */

namespace Growers.io.Models
{
    /// <summary>
    /// Controller and viewer class for security sub-system.
    /// </summary>
    public class Security : INotifyPropertyChanged
    {
        public Security()
        {
            NoiseLevel.CollectionChanged += (_, _) => { NotifyPropertyChanged(nameof(NoiseLevel)); };
            Luminosity.CollectionChanged += (_, _) => { NotifyPropertyChanged(nameof(Luminosity)); };
            Motion.CollectionChanged += (_, _) => { NotifyPropertyChanged(nameof(Motion)); };
            DoorState.CollectionChanged += (_, _) => { NotifyPropertyChanged(nameof(DoorState)); };
        }

        /// <summary>
        /// All noise level readings
        /// </summary>
        public ObservableCollection<Reading<int>> NoiseLevel { get; set; } = new();
        /// <summary>
        /// All luminosity readings
        /// </summary>
        public ObservableCollection<Reading<int>> Luminosity { get; set; } = new();
        /// <summary>
        /// All motion readings
        /// </summary>
        public ObservableCollection<Reading<int>> Motion { get; set; } = new();
        /// <summary>
        /// All door state readings
        /// </summary>
        public ObservableCollection<Reading<int>> DoorState { get; set; } = new();

        /// <summary>
        /// Actuator for door lock
        /// </summary>
        public Actuator DoorLock { get; set; } = new Actuator(CommandType.Servo);

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
