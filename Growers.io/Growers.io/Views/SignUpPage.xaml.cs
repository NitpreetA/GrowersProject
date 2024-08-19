using Firebase.Auth;
using Growers.io.Interfaces;
using Growers.io.Services;
using Growers.io.ViewModels;

namespace Growers.io.Views;

/*
 * Team: Growers.IO
 * Semester: Winter 2024
 * Date: May 14, 2024
 * Course: Application Development III
 * Description: Sign up page backing C# code
 */

public partial class SignUpPage : ContentPage, IViewInteractions
{
    public SignUpPage()
    {
        InitializeComponent();
        var viewModel = new SignUpViewModel(this);

        BindingContext = viewModel;
    }
}
