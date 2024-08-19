using Growers.io.Interfaces;
using Growers.io.Models;
using Growers.io.ViewModels;
using System.Collections.ObjectModel;

namespace Growers.io.Views;

/*
 * Team: Growers.IO
 * Semester: Winter 2024
 * Date: May 14, 2024
 * Course: Application Development III
 * Description: Add container page view c# code
 */

public partial class AddContainerPage : ContentPage, IViewInteractions
{

    public AddContainerPage()
	{
		InitializeComponent();

        var viewModel = new AddContainerViewModel(
			this, 
			App.Containers.ContainerDb, 
			App.UserContainer.MappingRepo, 
			App.User.UserInfoDb,
			App.User.UserCreds.User.Uid);

		BindingContext = viewModel;
	}
}