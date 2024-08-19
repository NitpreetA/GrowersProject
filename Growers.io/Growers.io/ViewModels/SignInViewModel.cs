using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Firebase.Auth;
using Growers.io.Interfaces;
using Growers.io.Models;
using Growers.io.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserInfo = Growers.io.Models.UserInfo;

/*
 * Team: Growers.IO
 * Semester: Winter 2024
 * Date: May 14, 2024
 * Course: Application Development III
 * Description: Sign in viewmodel
 */

namespace Growers.io.ViewModels
{
    public partial class SignInViewModel : ObservableObject
    {
        [ObservableProperty]
        string email;

        [ObservableProperty]
        string password;

        private readonly IViewInteractions viewInteractions;

        /// <summary>
        /// Viewmodel for sign in view.
        /// </summary>
        /// <param name="viewInteractions">Interface for view interactions</param>
        public SignInViewModel(IViewInteractions viewInteractions)
        {
            this.viewInteractions = viewInteractions;

            Email = "nit@gmail.com";
            Password = "123456";
        }

        // Proceeds with login
        [RelayCommand]
        async Task Proceed()
        {
            if (Connectivity.Current.NetworkAccess == NetworkAccess.None)
            {
                await viewInteractions.DisplayAlert("Error", "Not connected to a network", "OK");
                return;
            }
            if(string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password))
            {
                await viewInteractions.DisplayAlert("Error", "Email or password field empty", "OK");
                return;
            }

            try
            {

                AuthService.UserCreds =
                    await AuthService.Client.SignInWithEmailAndPasswordAsync(Email, Password);

                await viewInteractions.DisplayAlert("Success", "You logged in successfuly", "Ok");

                App.User.UserCreds = AuthService.UserCreds;

                Models.UserInfo user = App.User.UserInfoDb.Items.FirstOrDefault(f => f.UID == App.User.UserCreds.User.Uid);
                //MAui is horrible and wont bind a property in xaml 
                (Shell.Current as AppShell).UpdateTech(user.IsTechician);

                if (user.IsTechician)
                {
                    await Shell.Current.GoToAsync($"//Farmer/HomePage");
                }
                else 
                {
                    await Shell.Current.GoToAsync($"//Tech/HomePage");
                }
            }
            catch (FirebaseAuthException ex)
            { 
                await viewInteractions.DisplayAlert("Error","Invalid login. Please ensure that you  are using correct login information." , "Cancel");
            }
            catch (Exception ex)
            {
                await viewInteractions.DisplayAlert("Error", ex.Message, "Cancel");
            }
        }
    }
}

