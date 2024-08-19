using System.Text;
using Azure.Messaging.EventHubs.Consumer;
using Growers.io.Enums;
using Growers.io.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Reflection;
using Microsoft.Azure.Devices;
using Growers.io.DataRepos;
using Azure.Messaging.EventHubs;
using System.Security.Cryptography.X509Certificates;
using Growers.io.Interfaces;
using Microsoft.Rest;
using LiteDB;

/*
 * Team: Growers.IO
 * Semester: Winter 2024
 * Date: May 28, 2024
 * Course: Application Development III
 * Description: IoTHubService for listening to events.
 */

namespace Growers.io.Services
{
    public class IoTHubService
    {
        private readonly string _connectionString;

        /// <summary>
        /// Event for when IoTHub connection is lost.
        /// </summary>
        public event EventHandler ConnectionLost;
        /// <summary>
        /// Event for when IoTHub connection is restored.
        /// </summary>
        public event EventHandler ConnectionRestored;

        private bool _isConnected = true;

        /// <summary>
        /// Creates service with hub connection string.
        /// </summary>
        /// <param name="connectionString">The conneciton string</param>
        public IoTHubService(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Starts listening for all incoming events.
        /// </summary>
        public void StartReceivingMessages()
        {
            Task.Run(() => ReceiveMessagesFromDeviceAsync(_connectionString));
        }

        // Start listening for messages from devices.
        private async Task ReceiveMessagesFromDeviceAsync(string connectionString)
        {
            EventHubConsumerClientOptions options = new()
            {
                RetryOptions = new()
                {
                    Mode = EventHubsRetryMode.Fixed,
                    Delay = TimeSpan.FromSeconds(2),
                    MaximumDelay = TimeSpan.FromSeconds(2),
                    MaximumRetries = 100,
                    TryTimeout = TimeSpan.FromSeconds(2),
                }
            };
            while(true)
            {
                await using var consumer = new EventHubConsumerClient(
                    EventHubConsumerClient.DefaultConsumerGroupName,
                    connectionString,
                    options);
                try
                {

                    await foreach (PartitionEvent partitionEvent in consumer.ReadEventsAsync(true))
                    {
                        if(!_isConnected)
                        {
                            _isConnected = true;
                            ConnectionRestored?.Invoke(this, new EventArgs());
                        }

                        Console.WriteLine($"Message received on partition {partitionEvent.Partition.PartitionId}:");

                        string rawData = Encoding.UTF8.GetString(partitionEvent.Data.Body.ToArray());

                        var readingTypeRaw = (string)partitionEvent.Data.Properties["reading-type"];
                        ReadingType readingType;
                        if(!Enum.TryParse(readingTypeRaw, out readingType))
                        {
                            // If an unknown reading type is received, just ignore it.
                            continue;
                        }

                        dynamic timeStamp = partitionEvent.Data.SystemProperties["x-opt-enqueued-time"];
                        var deviceId = (string)partitionEvent.Data.SystemProperties["iothub-connection-device-id"];

                        try
                        {
                            var data = (JObject)JsonConvert.DeserializeObject(rawData);

                            var reading = CreateReading(
                                readingType,
                                data,
                                timeStamp.DateTime.ToLocalTime());

                            if(reading is not null)
                            {
                                InsertToRepo(readingType, reading, deviceId);
                            }
                        }
                        catch(Exception ex)
                        {
                            Console.WriteLine($"Exception:\n{ex}");
                        }
                    }
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    if(_isConnected)
                    {
                        _isConnected = false;
                        ConnectionLost?.Invoke(this, new EventArgs());
                    }
                    await Task.Delay(3000);
                }
            }
        }

        // Creates a Reading<unknown> from the data passed in.
        private object CreateReading(ReadingType type, JObject data, DateTime timeStamp)
        {
            if (data["value"].Type == JTokenType.String)
            {
                return null;
            }
            if (!data.TryGetValue("unit", out JToken jUnit))
            {
                return null;
            }
            string unit = jUnit.ToString();

            var readingInfo = typeof(ReadingType).GetMember(type.ToString()).FirstOrDefault();
            Type valueType = readingInfo.GetCustomAttribute<ReadingTypeAttribute>().ValueType;

            Type readingType = typeof(Reading<>).MakeGenericType(valueType);

            // Get the method to call Value on a JObject
            MethodInfo getValueMethod = typeof(JObject).GetMethod(nameof(JObject.Value)).MakeGenericMethod(valueType);
            object oValue = getValueMethod.Invoke(data, new object[] { "value" });

            // Create an instance of Reading of type readingType
            return Activator.CreateInstance(readingType, type, oValue, unit, timeStamp);
        }

        // Inserts a reading into the respective data repo.
        private void InsertToRepo(ReadingType type, object reading, string deviceId)
        {
            var readingInfo = typeof(ReadingType).GetMember(type.ToString()).FirstOrDefault();
            var readingAttributes = readingInfo.GetCustomAttribute<ReadingTypeAttribute>();

            PropertyInfo dataRepoProp = typeof(App).GetProperty(readingAttributes.RepoType.Name, BindingFlags.Public | BindingFlags.Static);
            var dataRepoInstance = dataRepoProp.GetValue(null);

            var subsysType = readingAttributes.RepoType.GetInterfaces()[0];

            var addReadingMethod = 
                subsysType
                .GetMethod(nameof(ISubsystemRepo<object>.AddReading))
                .MakeGenericMethod(readingAttributes.ValueType);

            addReadingMethod.Invoke(dataRepoInstance, new object[] { reading, deviceId });
        }
    }
}

