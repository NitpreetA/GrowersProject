
using Growers.io.Config;
using Growers.io.DataRepos;
using Growers.io.Services;
using Microsoft.Extensions.Configuration;
using System.Reflection;

/*
 * Team: Growers.IO
 * Semester: Winter 2024
 * Date: May 14, 2024
 * Course: Application Development III
 * Description: MAUI App class
 */

namespace Growers.io
{
    public partial class App : Application
    {
        public static Settings Settings { get; private set; }

        static UserInfoRepo _user;
        public static UserInfoRepo User
        {
            get
            {
                if (_user == null)
                    _user = new UserInfoRepo(AuthService.UserCreds.User, Settings.FirebaseDatabase);

                return _user;
            }
        }


        static UserToContainerRepo _uc;
        public static UserToContainerRepo UserContainer
        {
            get
            {
                if (_uc == null)
                    _uc = new UserToContainerRepo(AuthService.UserCreds.User, Settings.FirebaseDatabase);

                return _uc;
            }
        }

        private static ContainerRepo _crepo;
        public static ContainerRepo Containers {
            get
            {
                if (_crepo == null)
                    _crepo = new ContainerRepo(AuthService.UserCreds.User, Settings.FirebaseDatabase);

                return _crepo;
            }
        }

        public static IoTHubService IoTHubService { get; set; }

        public static PlantDataRepo PlantDataRepo { get; } = new PlantDataRepo();
        public static SecurityDataRepo SecurityDataRepo { get; } = new SecurityDataRepo();
        public static GeoLocationDataRepo GeoLocationDataRepo { get; } = new GeoLocationDataRepo();

        public App()
        {
            InitializeComponent();

            // Setup config
            var a = Assembly.GetExecutingAssembly();
            var stream = a.GetManifestResourceStream("Growers.io.appsettings.json");

            var config = new ConfigurationBuilder()
                        .AddJsonStream(stream)
                        .Build();

            Settings = config.GetRequiredSection(nameof(Settings)).Get<Settings>();

            IoTHubService = new IoTHubService(Settings.EventHubString);
            IoTHubService.StartReceivingMessages();

            MainPage = new AppShell();
        }
    }
}
