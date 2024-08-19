using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Firebase.Auth;
using Growers.io.Interfaces;
using Growers.io.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * Team: Growers.IO
 * Semester: Winter 2024
 * Date: May 14, 2024
 * Course: Application Development III
 * Description: Sign up viewmodel
 */

namespace Growers.io.ViewModels
{
    public partial class SignUpViewModel : ObservableObject
    {
        [ObservableProperty]
        string username;

        [ObservableProperty]
        string email;

        [ObservableProperty]
        string password;

        [ObservableProperty]
        string rePassword;

        [ObservableProperty]
        bool isOwner;

        [ObservableProperty]
        bool isTechnician;

        private readonly IViewInteractions viewInteractions;

        /// <summary>
        /// Viewmodel for sign up view.
        /// </summary>
        /// <param name="viewInteractions">Interface for view interactions</param>
        public SignUpViewModel(IViewInteractions viewInteractions)
        {
            this.viewInteractions = viewInteractions;
        }

        [RelayCommand]
        async Task Proceed()
        {

            if (Password != RePassword)
            {
                await viewInteractions.DisplayAlert("Input Error", "Your passwords must match.", "Alrighty");
                return;
            }
            if (IsOwner == false && IsTechnician == false)
            {
                await viewInteractions.DisplayAlert("Input Error", "You must select a role, either a farmer technian or a fleet owner.", "Alrighty");
                return;
            }

            try
            {
                if (Connectivity.Current.NetworkAccess == NetworkAccess.None)
                {
                    await viewInteractions.DisplayAlert("Error", "Not connected to a network", "OK");
                }
                AuthService.UserCreds =
                    await AuthService.Client.CreateUserWithEmailAndPasswordAsync(Email, Password, Username);

                Models.UserInfo user = new Models.UserInfo()
                {
                    UID = AuthService.UserCreds.User.Uid,
                    IsTechician = IsTechnician,
                    Name = AuthService.UserCreds.User.Info.DisplayName
                };

                await App.User.UserInfoDb.AddItemsAsync(user);
                App.User.UserCreds = AuthService.UserCreds;
                (Shell.Current as AppShell).UpdateTech(user.IsTechician);
                await viewInteractions.DisplayAlert("Success", "Created account successfully", "OK");
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
                await viewInteractions.DisplayAlert("Error", "Invalid login. Please ensure that you  are using correct login information.", "Cancel");
            }
            catch (Exception ex)
            {
                await viewInteractions.DisplayAlert("Error", ex.Message, "Cancel");
            }
        }
    }
}
