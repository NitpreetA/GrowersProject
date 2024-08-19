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
 * Description: Class to configure the application's external connections.
 */

namespace Growers.io.Config
{
    /// <summary>
    /// Settings for mobile application
    /// </summary>
    public class Settings
    {
        /// <summary>
        /// Firebase authentication domain
        /// </summary>
        public string FirebaseDomain { get; set; }

        /// <summary>
        /// Firebase API key
        /// </summary>
        public string FirebaseApiKey { get; set; }

        /// <summary>
        /// Firebase database connection string
        /// </summary>
        public string FirebaseDatabase { get; set; }

        /// <summary>
        /// IoTHub event hub compatible connection string
        /// </summary>
        public string EventHubString { get; set; }

        /// <summary>
        /// IoTHub connection string
        /// </summary>
        public string IoTHubString { get; set; }
    }
}
