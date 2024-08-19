namespace Growers.io.Views;

/*
 * Team: Growers.IO
 * Semester: Winter 2024
 * Date: May 14, 2024
 * Course: Application Development III
 * Description: Farmer home page backing C# code
 */

public partial class FarmerHome : ContentPage
{
	public FarmerHome()
	{
		InitializeComponent();
		BindingContext = App.UserContainer;
	}
}