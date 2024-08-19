using System.Collections.ObjectModel;
using Growers.io.Models;

namespace Growers.io.Views;

/*
 * Team: Growers.IO
 * Semester: Winter 2024
 * Date: May 14, 2024
 * Course: Application Development III
 * Description: Container page backing C# code
 */

public partial class ContainersPage : ContentPage
{
    public ObservableCollection<Container> Containers { get; set; }
    public ContainersPage()
    {
        InitializeComponent();
        BindingContext = this;
        Containers = App.UserContainer.Containers;
    }
    
    private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        var container = e.Parameter as Container;
        await Navigation.PushAsync(new ContainerInfoPage(container));
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        Containers = App.UserContainer.Containers;
    }
}

