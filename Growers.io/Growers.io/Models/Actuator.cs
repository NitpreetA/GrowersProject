using Firebase.Auth;
using Growers.io.Enums;
using Growers.io.Services;
using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Common.Exceptions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

/*
 * Team: Growers.IO
 * Semester: Winter 2024
 * Date: April 29, 2024
 * Course: Application Development III
 * Description: Class to control hardware actuator using IoT Hub.
 */

namespace Growers.io.Models
{
    /// <summary>
    /// Class for interfacing with actuator for a container.
    /// </summary>
    public class Actuator : INotifyPropertyChanged
    {
        /// <summary>
        /// Creates instance of Actuator using command type and initial value.
        /// </summary>
        /// <param name="type">Type of CommandType that the actuator permits.</param>
        public Actuator(CommandType type)
        {
            Type = type;
            CurrentValue = false;
        }

        /// <summary>
        /// Getter for command type of actuator.
        /// </summary>
        public CommandType Type { get; private set; }

        /// <summary>
        /// Getter for Type in lowercase.
        /// </summary>
        public string TypeLower => Type.ToString().ToLower();

        /// <summary>
        /// Getter/setter for current state of actuator.
        /// </summary>
        private bool _cv;
        public bool CurrentValue 
        {
            get => _cv;
            set 
            { 
                _cv = value;
                OnPropertyChanged(nameof(CurrentValue));
            } 
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Sends command to actuator using IoT Hub.
        /// </summary>
        /// <returns>Boolean indicating whether current state of the actuator was updated.</returns>
        public async Task<(bool, string)> SendCommand(IoTClientService client)
        {
            try
            {
                bool newValue = CurrentValue;
                var commandMessage = new
                    Message(Encoding.ASCII.GetBytes(GetMessage(newValue)));

                var is_online = await client.SendDirectMessage(new CloudToDeviceMethod("is_online"));
                if(!is_online.Item1)
                {
                    return (false, "Device offline, cannot control actuator.");
                }

                commandMessage.Properties.Add(new KeyValuePair<string, string>("command-type", TypeLower));

                await client.SendMessage(commandMessage);

                CurrentValue = newValue;
                return (true, null);
            }
            catch { }
            return (false, "An unknown error occurred when sending command.");
        }

        /// <summary>
        /// Gets the latest actuator from IoT hub
        /// </summary>
        /// <param name="client">Client in which actuator exists</param>
        /// <returns>Task for status of operation</returns>
        public async Task<(bool, string)> GetLatestState(IoTClientService client)
        {
            var method = new CloudToDeviceMethod("get_actuator_value");
            method.SetPayloadJson($"{{\"command-type\":\"{TypeLower}\"}}");

            (bool success, CloudToDeviceMethodResult result, Exception error) = await client.SendDirectMessage(method);

            if(success)
            {
                if(result.Status == 200)
                {
                    var payload = (JObject)JsonConvert.DeserializeObject(result.GetPayloadAsJson());
                    var value = payload["value"].ToString().ToLower();
                    var index = Array.IndexOf(Commands(), value);
                    CurrentValue = index == 0;
                }
                return (true, null);
            }
            else
            {
                if(error is DeviceNotFoundException)
                {
                    return (false, "Device offline when retrieving latest actuator state.");
                }
                else
                {
                    return (false, "Unknown error when retrieving latest actuator state.");
                }
            }
        }

        // Gets the message payload based on the desired state.
        private string GetMessage(bool state)
        {
            var options = Commands();
            string value = state ? options[0] : options[1];  
            return $"{{\"value\":\"{value}\"}}";
        }

        // Gets an array possible "value"s based on the current reading type.
        private string[] Commands()
        {
            switch (Type)
            {
                case CommandType.Servo:
                    return new string[] { "open", "close" };
                case CommandType.Fan:
                    return new string[] { "on", "off" };
                case CommandType.Led:
                    return new string[] { "on", "off" };
                case CommandType.Buzzer:
                    return new string[] { "on", "off" };
                default:
                    return new string[] { "", "" }; 
            }
        }
    }
}
