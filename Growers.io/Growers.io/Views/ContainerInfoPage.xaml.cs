

using Growers.io.DataRepos;
using Growers.io.Enums;
using Growers.io.Interfaces;
using Growers.io.Models;
using Growers.io.ViewModels;
using LiveChartsCore.SkiaSharpView.Maui;
using SkiaSharp;
using System.Collections.ObjectModel;

namespace Growers.io.Views;

/*
 * Team: Growers.IO
 * Semester: Winter 2024
 * Date: May 14, 2024
 * Course: Application Development III
 * Description: Container info view backing C# code
 */

public partial class ContainerInfoPage : ContentPage,IViewInteractions
{
    public ContainerInfoPage(Container container)
	{
		InitializeComponent();
        
        var viewModel = new ContainerInfoViewModel(this, container, Connectivity.Current);
        BindingContext = viewModel;
    }
}
