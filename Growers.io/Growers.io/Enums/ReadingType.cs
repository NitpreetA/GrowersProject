using Growers.io.DataRepos;
using System.Runtime.CompilerServices;

/*
 * Team: Growers.IO
 * Semester: Winter 2024
 * Date: April 29, 2024
 * Course: Application Development III
 * Description: Enum for reading types.
 */

namespace Growers.io.Enums
{

    /// <summary>
    /// Attributes for a <see cref="ReadingType"/>
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class ReadingTypeAttribute : Attribute
    {
        public Type ValueType { get; private set; }
        public Type RepoType { get; private set; }

        public ReadingTypeAttribute(Type type, Type subsystem)
        {
            this.ValueType = type;
            this.RepoType = subsystem;
        }
    }

    /// <summary>
    /// Enum for reading type from IoT Hub.
    /// </summary>
    public enum ReadingType
    {
        [ReadingType(typeof(double), typeof(PlantDataRepo))]
        Temperature,

        [ReadingType(typeof(double), typeof(PlantDataRepo))]
        Humidity,

        [ReadingType(typeof(double), typeof(PlantDataRepo))]
        WaterLevel,

        [ReadingType(typeof(int), typeof(PlantDataRepo))]
        SoilMoisture,

        [ReadingType(typeof(int), typeof(SecurityDataRepo))]
        NoiseLevel,

        [ReadingType(typeof(int), typeof(SecurityDataRepo))]
        Luminosity,

        [ReadingType(typeof(int), typeof(SecurityDataRepo))]
        Motion,

        [ReadingType(typeof(int), typeof(SecurityDataRepo))]
        DoorState,

        [ReadingType(typeof(double), typeof(GeoLocationDataRepo))]
        GPSLongitude,

        [ReadingType(typeof(double), typeof(GeoLocationDataRepo))]
        GPSLatitude,

        [ReadingType(typeof(double), typeof(GeoLocationDataRepo))]
        Pitch,

        [ReadingType(typeof(double), typeof(GeoLocationDataRepo))]
        Yaw,

        [ReadingType(typeof(double), typeof(GeoLocationDataRepo))]
        Roll,

        [ReadingType(typeof(double), typeof(GeoLocationDataRepo))]
        AccelerationX,

        [ReadingType(typeof(double), typeof(GeoLocationDataRepo))]
        AccelerationY,

        [ReadingType(typeof(double), typeof(GeoLocationDataRepo))]
        AccelerationZ
    }
}
