using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * Team: Growers.IO
 * Semester: Winter 2024
 * Date: May 28, 2024
 * Course: Application Development III
 * Description: Converter for door state readings.
 */

namespace Growers.io.Converters
{
    /// <summary>
    /// Converter for the last value of a collection of door state readings. Inherits from <see cref="LastElementConverter"/>
    /// </summary>
    public class DoorStateConverter : LastElementConverter
    {
        public override string Format(object value, string unit)
        {
            var valueInt = (int)value;

            return valueInt == 0 ? "Closed" : "Open";
        }
    }
}
