namespace Growers.io.Views;
using Firebase.Auth;
using Growers.io.Interfaces;
using Growers.io.Services;
using Growers.io.ViewModels;
using Newtonsoft.Json;
using System.Runtime.ExceptionServices;

/*
 * Team: Growers.IO
 * Semester: Winter 2024
 * Date: May 14, 2024
 * Course: Application Development III
 * Description: Login page backing C# code
 */

public partial class LogInPage : ContentPage, IViewInteractions
{
	public LogInPage()
	{
		InitializeComponent();
        var viewModel = new SignInViewModel(this);
        BindingContext = viewModel;
	}
}