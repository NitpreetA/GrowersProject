namespace Growers.io.Views;

/*
 * Team: Growers.IO
 * Semester: Winter 2024
 * Date: May 14, 2024
 * Course: Application Development III
 * Description: Welcome page backing C# code
 */

public partial class WelcomePage : ContentPage
{
	public WelcomePage()
	{
		InitializeComponent();
	}

    private async void LogInBtn_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new LogInPage());
    }

    private async void SignUpBtn_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new SignUpPage());

    }
}