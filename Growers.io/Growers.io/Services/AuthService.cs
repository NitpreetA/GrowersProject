using Firebase.Auth;
using Firebase.Auth.Providers;
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
 * Description: Service for Firebase authentication, storing current client and user credentials.
 */

namespace Growers.io.Services
{
    /// <summary>
    /// Static class for managing authetication with Firebase.
    /// </summary>
    public static class AuthService
    {
        // Configure...
        private static FirebaseAuthConfig config = new FirebaseAuthConfig
        {
            ApiKey = App.Settings.FirebaseApiKey,
            AuthDomain = App.Settings.FirebaseDomain,
            Providers = new FirebaseAuthProvider[]
            {
                // Add and configure individual providers
                new EmailProvider()
            },
        };
        /// <summary>
        /// Getter for <see cref="FirebaseAuthClient"/> user client.
        /// </summary>
        public static FirebaseAuthClient Client { get; } = new FirebaseAuthClient(config);

        /// <summary>
        /// Getter/Setter for <see cref="UserCredential"/> user credentials.
        /// </summary>
        public static UserCredential UserCreds { get; set; }
    }
}
