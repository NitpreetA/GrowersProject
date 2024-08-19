using Growers.io.Enums;
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
 * Description: Class for a sensor reading received from the IoT Hub. Generic type for value.
 */

namespace Growers.io.Models
{
    /// <summary>
    /// Representation of a sensor reading.
    /// </summary>
    public class Reading<T>
    {
        public Reading()
        {

        }

        public Reading(ReadingType type, T value, string unit, DateTime timeStamp)
        {
            Type = type;
            Value = value;
            Unit = unit;
            TimeStamp = timeStamp;
        }

        /// <summary>
        /// Reading type for the current instance of the reading.
        /// </summary>
        public ReadingType Type { get; set; }

        /// <summary>
        /// Value of the reading.
        /// </summary>
        public T Value { get; set; }
        
        /// <summary>
        /// Unit of the reading.
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// TimeStamp of the reading.
        /// </summary>
        public DateTime TimeStamp { get; set; }
    }
}
