using Growers.io.Models;
using Growers.io.Services;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using System.Collections.ObjectModel;

namespace Growers.io.Views;

/*
 * Team: Growers.IO
 * Semester: Winter 2024
 * Date: May 14, 2024
 * Course: Application Development III
 * Description: Home page backing C# code
 */

public partial class HomePage : ContentPage
{
    public ObservableCollection<Container> Containers { get; set; }
	public HomePage()
	{
		InitializeComponent();
        BindingContext = App.User;
        Containers = App.UserContainer.Containers;
        AddPinsToMap();
	}

    private async void AddContainer_Clicked(object sender, EventArgs e)
    {
		await Navigation.PushAsync(new AddContainerPage());
    }

    private void AddPinsToMap()
    {

        foreach (var cont in Containers)
        {
            var latitude = cont.Location.GPSLatitude.LastOrDefault()?.Value ?? 0;
            var longitude = cont.Location.GPSLongitude.LastOrDefault()?.Value ?? 0;

            var pin = new CustomPin
            {
                Label = cont.Name,
                Type = PinType.Place,
                Location = new Location(latitude, longitude),
                Container = cont
            };

            pin.InfoWindowClicked += OnMarkerClicked;

            map.Pins.Add(pin);
        }

        if(Containers.Count > 0)
        {
            var initialLocation = new Location(Containers[0].Location.GPSLatitude.LastOrDefault()?.Value ?? 0,
                                                   Containers[0].Location.GPSLongitude.LastOrDefault()?.Value ?? 0);
            map.MoveToRegion(MapSpan.FromCenterAndRadius(initialLocation, Distance.FromMiles(5)));
        }
    }

    private async void OnMarkerClicked(object sender, EventArgs e)
    {
        var pin = sender as CustomPin;
        if (pin != null)
        {
            await Navigation.PushAsync(new ContainerInfoPage(pin.Container));
        }
    }
}       


public class CustomPin : Pin
    {
        public Container Container{ get; set; }
    }