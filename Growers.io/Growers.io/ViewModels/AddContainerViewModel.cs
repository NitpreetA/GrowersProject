using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Growers.io.Interfaces;
using Growers.io.Models;
using System.Collections.ObjectModel;
using System.Linq;

/*
 * Team: Growers.IO
 * Semester: Winter 2024
 * Date: May 14, 2024
 * Course: Application Development III
 * Description: Viewmodel for add container page
 */

namespace Growers.io.ViewModels
{
    public partial class AddContainerViewModel : ObservableObject
    {
        private readonly IViewInteractions viewInteractions;
        private readonly IDataStore<Container> containerDb;
        private readonly IDataStore<UserToContainer> userToContainerDb;
        private readonly IDataStore<UserInfo> userInfoDb;
        private readonly string currentUserUID;
        
        [ObservableProperty]
        string containerName;

        [ObservableProperty]
        string deviceID;

        [ObservableProperty]
        ObservableCollection<UserInfo> users;

        private List<UserInfo> SelectedFarmers { get; set; } = new();

        /// <summary>
        /// Creates viewmodel for add container page
        /// </summary>
        /// <param name="viewViewModelInteractions">Interface to view interactions</param>
        /// <param name="containerDb">Container db interface</param>
        /// <param name="userToContainerDb">User to container db interface</param>
        /// <param name="userInfoDb">Userinfo db interface</param>
        /// <param name="currentUserUID">Current user UID</param>
        public AddContainerViewModel(
            IViewInteractions viewViewModelInteractions,
            IDataStore<Container> containerDb,
            IDataStore<UserToContainer> userToContainerDb,
            IDataStore<UserInfo> userInfoDb,
            string currentUserUID)
        {
            this.viewInteractions = viewViewModelInteractions;
            this.containerDb = containerDb;
            this.userToContainerDb = userToContainerDb;
            this.userInfoDb = userInfoDb;
            this.currentUserUID = currentUserUID;
            users = new ObservableCollection<UserInfo>(userInfoDb.Items.Where(f => f.IsTechician == true));

            SelectedFarmersChangedCommand = new Command(SelectedFarmersChanged);
        }

        // Note: Binding to SelectedItems doesn't work for 2+ years (https://github.com/dotnet/maui/issues/8435).
        // RelayCommand doesn't work when binding to SelectionChangedCommand. How is this possible?!
        public Command SelectedFarmersChangedCommand { get; set; }
        void SelectedFarmersChanged(object collectionView)
        {
            SelectedFarmers = (collectionView as CollectionView).SelectedItems.Cast<UserInfo>().ToList();
        }

        // Creates a container
        [RelayCommand]
        async Task CreateContainer()
        {
            try
            {
                if(SelectedFarmers.Count < 1)
                {
                    await viewInteractions.DisplayAlert("Error", "Please select at least one farmer.", "Ok");
                    return;
                }

                var newContainer = new Container()
                {
                    Name = ContainerName,
                    DeviceID = DeviceID
                };

                await containerDb.AddItemsAsync(newContainer);
                await userToContainerDb.AddItemsAsync(new UserToContainer
                {
                    OwnerUID = currentUserUID,
                    ContainerKey = newContainer.Key,
                    FarmerUIDs = SelectedFarmers.Select(f => f.UID)
                });
                
                await viewInteractions.DisplayAlert("Success", "Container created successfully", "Ok");
                await viewInteractions.Navigation.PopAsync();
            }
            catch(Exception ex)
            {
                await viewInteractions.DisplayAlert("Error", $"{ex.Message}", "Ok");
            }
        }
    }
}
