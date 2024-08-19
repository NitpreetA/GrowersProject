using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Growers.io.DataRepos;
using Growers.io.Interfaces;
using Growers.io.Models;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Maui;
using LiveChartsCore.SkiaSharpView.Painting;
using Microsoft.Maui.Controls.Handlers;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * Team: Growers.IO
 * Semester: Winter 2024
 * Date: May 28, 2024
 * Course: Application Development III
 * Description: ViewModel for container info page
 */

namespace Growers.io.ViewModels
{
    public partial class ContainerInfoViewModel : ObservableObject
    {
        /// <summary>
        /// Creates viewmodel using <see cref="IViewInteractions"/>, and relevant container and connectivity.
        /// </summary>
        /// <param name="viewInteractions">The view</param>
        /// <param name="container">Container to show</param>
        /// <param name="connectivity">Connectivity status</param>
        public ContainerInfoViewModel(IViewInteractions viewInteractions, Container container, IConnectivity connectivity)
        {
            this.viewInteractions = viewInteractions;
            this.connectivity = connectivity;

            Container = container;

            SecurityChart = GetSecurityChart();
            PlantChart = GetPlantChart();
            GeoChart = GetGeoChart();

            CurrentUser = App.User.UserInfoDb.Items.FirstOrDefault(f=>f.UID == App.User.UserCreds.User.Uid);

            GetLatestActuatorStates();
            GetTelemetryInterval();

            App.IoTHubService.ConnectionLost += (_, _) =>
            {
                ContainerHeader = "Lost connection to Hub";
                SetAllSeriesColor(SKColors.OrangeRed.WithAlpha(125));
            };

            App.IoTHubService.ConnectionRestored += (_, _) =>
            {
                ContainerHeader = "";
                SetAllSeriesColor(SKColors.LightBlue.WithAlpha(125));
            };
        }

        private void SetAllSeriesColor(SKColor color)
        {
            foreach (CartesianChart chart in SecurityChart.Concat(PlantChart).Concat(GeoChart))
            {
                var series = chart.Series.FirstOrDefault() as LineSeries<DateTimePoint>;
                series.Fill = new SolidColorPaint(color);
            }
        }

        private void GetTelemetryInterval()
        {
            try
            {
                var interval = Task.Run(Container.ClientService.GetTelemetryInterval);
                interval.Wait();

                TelemetryInterval = interval.Result;
            }
            catch(Exception ex)
            {
                Task.Run(() => viewInteractions.DisplayAlert("Error", "Failed to get telemetry interval.", "Ok"));
            }
        }

        private void GetLatestActuatorStates()
        {
            // I want to say that this entire method is stupid, but MAUI left me no choice so it had to be this way.
            try
            {
                var fan = Task.Run(() => Container.Plant.Fan.GetLatestState(Container.ClientService));
                var light = Task.Run(() => Container.Plant.Light.GetLatestState(Container.ClientService));
                var buzzer = Task.Run(() => Container.Security.Buzzer.GetLatestState(Container.ClientService));
                var doorlock = Task.Run(() => Container.Security.DoorLock.GetLatestState(Container.ClientService));

                // Wait till all are complete
                fan.Wait();
                light.Wait();
                buzzer.Wait(); 
                doorlock.Wait();

                List<(bool, string)> results = new List<(bool, string)>() { fan.Result, light.Result, buzzer.Result, doorlock.Result };

                (bool res, string error) = results.FirstOrDefault(r => !r.Item1, (true, ""));
                if(!res)
                {
                    Task.Run(() => viewInteractions.DisplayAlert("Error", error, "Ok"));
                }
            }
            catch(Exception ex)
            {
                Task.Run(() => ShowAlertAndPop("Error", "Failed to connect to Azure IoT Hub", "Ok"));
            }
        }

        private async Task ShowAlertAndPop(string title, string body, string buttonText)
        {
            await viewInteractions.DisplayAlert(title, body, buttonText);
            await viewInteractions.Navigation.PopAsync();
        }

        private readonly IViewInteractions viewInteractions;
        private readonly IConnectivity connectivity;

        [ObservableProperty]
        Container container;

        [ObservableProperty]
        string containerHeader;

        [ObservableProperty]
        UserInfo currentUser;

        [ObservableProperty]
        int telemetryInterval;

        [ObservableProperty]
        ObservableCollection<CartesianChart> securityChart;

        [ObservableProperty]
        ObservableCollection<CartesianChart> plantChart;

        [ObservableProperty]
        ObservableCollection<CartesianChart> geoChart;

        private ObservableCollection<CartesianChart> GetSecurityChart()
        {
            try
            {
                var intToPoint = (Reading<int> d) => (d.TimeStamp, (double)d.Value);
                var boolToPoint = (Reading<bool> d) => (d.TimeStamp, d.Value == true ? (double)1 : 0);

                var noiseLevelChart = new LineGraph<int>(Container.Security.NoiseLevel, ChartsRepo.GetChart("Noise Level Chart", 0, 1000), intToPoint);
                var luminosityChart = new LineGraph<int>(Container.Security.Luminosity, ChartsRepo.GetChart("Luminosity Level Chart", 0, 1000), intToPoint);
                var doorStateChart = new LineGraph<int>(Container.Security.DoorState, ChartsRepo.GetChart("Door State Chart", 0, 1), intToPoint);
                var motionChart = new LineGraph<int>(Container.Security.Motion, ChartsRepo.GetChart("Motion Detection Chart", 1, 1), intToPoint);

                return new() { noiseLevelChart.Chart, luminosityChart.Chart, doorStateChart.Chart, motionChart.Chart };
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private ObservableCollection<CartesianChart> GetPlantChart()
        {
            try
            {
                var doubleToPoint = (Reading<double> d) => (d.TimeStamp, d.Value);
                var intToPoint = (Reading<int> d) => (d.TimeStamp, (double)d.Value);

                var temperatureChart = new LineGraph<double>(Container.Plant.Temperature, ChartsRepo.GetChart("Temperature Chart", 0, 40), doubleToPoint);
                var humidityChart = new LineGraph<double>(Container.Plant.Humidity, ChartsRepo.GetChart("Humidity Chart", 0, 100), doubleToPoint);
                var waterLevelChart = new LineGraph<double>(Container.Plant.WaterLevel, ChartsRepo.GetChart("Water Level Chart", 0, 6), doubleToPoint);
                var soilMoistureChart = new LineGraph<int>(Container.Plant.SoilMoisture, ChartsRepo.GetChart("Soil Moisture Chart", 300, 800), intToPoint);

                return new() { temperatureChart.Chart, humidityChart.Chart, waterLevelChart.Chart, soilMoistureChart.Chart };
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private ObservableCollection<CartesianChart> GetGeoChart()
        {
            try
            {
                var doubleToPoint = (Reading<double> d) => (d.TimeStamp, d.Value);

                var pitchChart = new LineGraph<double>(Container.Location.Pitch, ChartsRepo.GetChart("Pitch Chart", -360, 360), doubleToPoint);
                var yawChart = new LineGraph<double>(Container.Location.Yaw, ChartsRepo.GetChart("Yaw Chart", -360, 360), doubleToPoint);
                var rollChart = new LineGraph<double>(Container.Location.Roll, ChartsRepo.GetChart("Roll Chart", -360, 360), doubleToPoint);
                var accXChart = new LineGraph<double>(Container.Location.AccelerationX, ChartsRepo.GetChart("AccelerationX Chart", -1500, 1500), doubleToPoint);
                var accYChart = new LineGraph<double>(Container.Location.AccelerationY, ChartsRepo.GetChart("AccelerationY Chart", -1500, 1500), doubleToPoint);
                var accZChart = new LineGraph<double>(Container.Location.AccelerationZ, ChartsRepo.GetChart("AccelerationZ Chart", -1500, 1500), doubleToPoint);

                return new() { pitchChart.Chart, yawChart.Chart, rollChart.Chart, accXChart.Chart, accYChart.Chart, accZChart.Chart };
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Toggle the actuator when a ToggleSwitch command is sent.
        /// </summary>
        /// <param name="actuator">The actuator to control</param>
        [RelayCommand]
        public async Task ToggleSwitch(Actuator actuator)
        {
            if(connectivity.NetworkAccess == NetworkAccess.None)
            {
                await viewInteractions.DisplayAlert("Error", "Please reconnect to the internet to send an actuator command.", "Ok");
                actuator.CurrentValue = !actuator.CurrentValue;
                return;
            }

            (var success, var error) = await actuator.SendCommand(Container.ClientService);
            if(!success)
            {
                actuator.CurrentValue = !actuator.CurrentValue;
                await viewInteractions.DisplayAlert("Error", error, "Ok");
            }
        }

        /// <summary>
        /// Update the telemetry inverval on device twin.
        /// </summary>
        [RelayCommand]
        public async Task UpdateTelemetryInterval()
        {
            const int MIN_INTERVAL = 1, MAX_INTERVAL = 60;
            if(TelemetryInterval < MIN_INTERVAL || TelemetryInterval > MAX_INTERVAL)
            {
                await viewInteractions.DisplayAlert("Failure", $"Telemetry interval must be {MIN_INTERVAL}-{MAX_INTERVAL} inclusively.", "Ok");
                return;
            }

            var success = await Container.ClientService.SetTelemetryInterval(TelemetryInterval);
            if(success)
            {
                await viewInteractions.DisplayAlert("Success", $"Successfully updated telemetryInterval to {TelemetryInterval}", "Ok");
            }
            else
            {
                await viewInteractions.DisplayAlert("Failure", "Failed to update telemetryInterval", "Ok");
            }
        }
    }
}
