using CommunityToolkit.Mvvm.ComponentModel;
using Growers.io.Services;
using System.ComponentModel;

/*
 * Team: Growers.IO
 * Semester: Winter 2024
 * Date: May 28, 2024
 * Course: Application Development III
 * Description: The AppShell, inheriting from MAUI Shell.
 */

namespace Growers.io
{
    public partial class AppShell : Shell, INotifyPropertyChanged
    {

        public AppShell()
        {
            InitializeComponent();
            BindingContext = this;

        }

        public void UpdateTech(bool tech)
        {
            if (tech)
            {
                farmertab.FlyoutItemIsVisible = true;
                techtab.FlyoutItemIsVisible = false;
            }
            else
            {
                farmertab.FlyoutItemIsVisible = false;
                techtab.FlyoutItemIsVisible = true;
            }

        }

        private void Btn_logOut_Clicked(object sender, EventArgs e)
        {
            AuthService.Client.SignOut();
            Shell.Current.FlyoutIsPresented = false;
            Shell.Current.GoToAsync("//MainPage");
        }
    }
}
