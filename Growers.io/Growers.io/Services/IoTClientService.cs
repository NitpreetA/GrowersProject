using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Shared;

/*
 * Team: Growers.IO
 * Semester: Winter 2024
 * Date: May 20, 2024
 * Course: Application Development III
 * Description: Service for Azure IoT client service.
 */

namespace Growers.io.Services
{
    public class IoTClientService
    {
        private string _connectionString;
        /// <summary>
        /// Getter for IoT connection string.
        /// </summary>
        public string ConnectionString 
        { 
            get => _connectionString; 
            private set
            {
                if(string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException($"{nameof(ConnectionString)} cannot be empty.");
                }
                _connectionString = value;
            }
        }

        private string _deviceId;
        /// <summary>
        /// Getter for IoT deviceId.
        /// </summary>
        public string DeviceId 
        { 
            get => _deviceId; 
            private set
            {
                if(string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException($"{nameof(DeviceId)} cannot be empty.");
                }
                _deviceId = value;
            }
        }

        /// <summary>
        /// Getter for ServiceClient
        /// </summary>
        public ServiceClient Client { get; private set; }

        /// <summary>
        /// Getter for RegistryManager
        /// </summary>
        public RegistryManager RegistryManager { get; private set; }

        /// <summary>
        /// Creates an instance of IoTClientService with a connection string and deviceId
        /// </summary>
        /// <param name="connectionString">Connection string for device</param>
        /// <param name="deviceId">Device to interact with</param>
        public IoTClientService(string connectionString, string deviceId)
        {
            ConnectionString = connectionString;
            DeviceId = deviceId;

            try
            {
                Client = ServiceClient.CreateFromConnectionString(ConnectionString);
                RegistryManager = RegistryManager.CreateFromConnectionString(ConnectionString);
            }
            catch(Exception ex)
            {
                // This probably won't happen.
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Sends a C2D message with the message provided.
        /// </summary>
        /// <param name="message"></param>
        /// <exception cref="Exception">Thrown when message fails to send or bad data.</exception>
        public async Task<(bool, Exception)> SendMessage(Message message)
        {
            try
            {
                await Client.SendAsync(DeviceId, message, null);
            }
            catch (Exception ex)
            {
                return (false, ex);
            }
            return (true, null);
        }

        /// <summary>
        /// Invokes a direct method for the current device.
        /// </summary>
        /// <param name="deviceMethod">Method to invoke</param>
        /// <returns>Method result</returns>
        public async Task<(bool, CloudToDeviceMethodResult, Exception)> SendDirectMessage(CloudToDeviceMethod deviceMethod)
        {
            try
            {
                var result = await Client.InvokeDeviceMethodAsync(DeviceId, deviceMethod);
                return (true, result, null);
            }
            catch (Exception ex)
            {
                return (false, null, ex);
            }
        }

        /// <summary>
        /// Gets the telemetryInterval from the IoTHub device twin.
        /// </summary>
        /// <returns>The telemetryInterval as a string (within the task)</returns>
        public async Task<int> GetTelemetryInterval()
        {
            const int DEFAULT_INTERVAL = 5;
            try
            {
                var twin = await RegistryManager.GetTwinAsync(DeviceId);
                return twin.Properties.Desired["telemetryInterval"];
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error occurred getting telemetryInterval. Message:\n{ex.Message}");
                return DEFAULT_INTERVAL;
            }
        }

        /// <summary>
        /// Sets the telemetryInterval on IoTHub device twin.
        /// </summary>
        /// <param name="interval">The new telemetry interval</param>
        /// <returns>Boolean stating if it was success</returns>
        public async Task<bool> SetTelemetryInterval(int interval)
        {
            try
            {
                var twin = await RegistryManager.GetTwinAsync(DeviceId);
                twin.Properties.Desired["telemetryInterval"] = interval;
                await RegistryManager.UpdateTwinAsync(DeviceId, twin, twin.ETag);
                return true;
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error occurred setting telemetryInterval. Message:\n{ex.Message}");
                return false;
            }
        }
    }
}
