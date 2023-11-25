using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public async Task OpenUrl(object paramater)
        {
            if (paramater == null || !(paramater is string url))
            {
                return;
            }

            try
            {
                await Browser.OpenAsync(url, BrowserLaunchMode.SystemPreferred);
            }
            catch (Exception ex)
            {
                // Handle any exceptions that may occur while opening the URL
                Debug.WriteLine($"Error opening URL: {ex.Message}");
            }

            await Task.CompletedTask; // replace with whatever code
        }

        [RelayCommand]
        public async Task SaveChanges()
        {
            await Task.CompletedTask; // replace with whatever code
        }
    }
}
