using Growers.io.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

/*
 * Team: Growers.IO
 * Semester: Winter 2024
 * Date: May 14, 2024
 * Course: Application Development III
 * Description: Interface for a subsystem's data repository.
 */

namespace Growers.io.Interfaces
{
    /// <summary>
    /// Interface for subsystem's data repository.
    /// </summary>
    /// <typeparam name="T">Type of subsystem</typeparam>
    public interface ISubsystemRepo<T>
    {
        /// <summary>
        /// Adds a reading into the subsystem's appropriate collection of readings.
        /// </summary>
        /// <typeparam name="U">Type of reading, must match the collection type.</typeparam>
        /// <param name="reading">Reading to add.</param>
        public void AddReading<U>(Reading<U> reading, string deviceId)
        {
            var collectionType = typeof(ICollection<>).MakeGenericType(reading.GetType());

            PropertyInfo collectionProp = typeof(T).GetProperty(reading.Type.ToString());

            var readingCollection = collectionProp.GetValue(GetSubsystemFromDeviceId(deviceId));

            MethodInfo addMethod = collectionType.GetMethod("Add");
            addMethod.Invoke(readingCollection, new object[] { reading });
        }

        /// <summary>
        /// Gets the subsystem according to a deviceId.
        /// </summary>
        /// <param name="deviceId">DeviceId to associate subsystem with</param>
        /// <returns>The subsystem</returns>
        public T GetSubsystemFromDeviceId(string deviceId);
    }
}
