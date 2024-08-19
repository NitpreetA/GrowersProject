namespace Growers.io.Views;
using Firebase.Auth;
using Growers.io.Services;

/*
 * Team: Growers.IO
 * Semester: Winter 2024
 * Date: May 14, 2024
 * Course: Application Development III
 * Description: Profile page backing C# code
 */

public partial class ProfilePage : ContentPage
{
	public UserInfo UserInfo {  get; set; }
	public ProfilePage()
	{
		InitializeComponent();

        BindingContext = App.User;
    }
}