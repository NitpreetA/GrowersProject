using Growers.io.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

/*
 * Team: Growers.IO
 * Semester: Winter 2024
 * Date: May 28, 2024
 * Course: Application Development III
 * Description: Converter for a last element for an ObservableCollection of Reading(s)
 */

namespace Growers.io.Converters
{
    /// <summary>
    /// Converter for a last element for an ObservableCollection of Reading(s)
    /// </summary>
    public class LastElementConverter : IValueConverter
    {
        /// <summary>
        /// Virtual function for returning the format of the reading's value and unit.
        /// </summary>
        /// <param name="value">Value of reading</param>
        /// <param name="unit">Unit of reading</param>
        /// <returns>String of formatted reading.</returns>
        public virtual string Format(object value, string unit)
        {
            return value.ToString() + unit;
        }

        public virtual object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Reading type and their value and unit properties.
            var readingType = value.GetType().GetGenericArguments()[0];
            var valueMethod = readingType.GetProperty("Value");
            var unitMethod = readingType.GetProperty("Unit");

            // LastOrDefault method for the ObservableCollection
            var lastMethod = typeof(Queryable)
                .GetMethods()
                .FirstOrDefault(m => m.Name == "LastOrDefault" && m.GetParameters().Length == 1)
                .MakeGenericMethod(readingType);

            // Converting the ObservableCollection to Queryable, so we can call LastOrDefault
            var asQueryable = typeof(Queryable)
                .GetMethods()
                .FirstOrDefault(m => m.Name == "AsQueryable")
                .MakeGenericMethod(readingType);

            if (value is not null)
            {
                // Get last element
                var last = lastMethod.Invoke(value, new object[] { asQueryable.Invoke(value, new object[] { value }) });
                if(last != null)
                {
                    // Get reading and unit, and return the formatted string.
                    object? readingValue = valueMethod.GetValue(last);
                    string? readingUnit = unitMethod.GetValue(last).ToString();
                    if(readingValue is not null && readingUnit is not null)
                    {
                        return Format(readingValue, readingUnit);
                    }
                }
            }
            // If there are no readings in the collection, return a placeholder to show that.
            return "----";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new ObservableCollection<object>();
        }
    }
}
