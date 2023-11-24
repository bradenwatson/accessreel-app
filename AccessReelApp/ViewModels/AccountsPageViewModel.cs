using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccessReelApp.ViewModels
{
    public partial class AccountsPageViewModel : ObservableObject
    {
        [ObservableProperty] string firstName;
        [ObservableProperty] string lastName;
        [ObservableProperty] string password;

        [RelayCommand]
        public async Task ChangePassword()
        {
            await Task.CompletedTask; // replace with whatever code
        }

        [RelayCommand]
        public async Task OpenUrl(object paramter)
        {
            if(paramter == null)
            {
                return;
            }

            // do stuff... 

            await Task.CompletedTask; // replace with whatever code
        }

        [RelayCommand]
        public async Task SaveChanges()
        {
            await Task.CompletedTask; // replace with whatever code
        }
    }
}
