using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * Team: Growers.IO
 * Semester: Winter 2024
 * Date: May 28, 2024
 * Course: Application Development III
 * Description: Converter for reading which have a double type.
 */

namespace Growers.io.Converters
{
    /// <summary>
    /// Converter for the last value of a collection of readings of type double. Inherits from <see cref="LastElementConverter"/>
    /// </summary>
    public class DoubleConverter : LastElementConverter
    {
        public override string Format(object value, string unit)
        {
            if(value is double)
            {
                return $"{Math.Round((double)value, 2)}{unit}";
            }
            else
            {
                return $"{value.ToString()}{unit}";
            }
        }
    }
}
