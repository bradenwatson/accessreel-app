using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using CommunityToolkit.Mvvm.Messaging;
using AccessReelApp.Messages;

namespace AccessReelApp.ViewModels
{
    public partial class SignUpLoginViewModel : ObservableObject
    {
        const string DEFAULT_USERNAME = "test";
        const string DEFAULT_PASSWORD = "test";

        [ObservableProperty] bool isSignedUp;
        [ObservableProperty] bool isSignedIn;

        [ObservableProperty] string username = string.Empty;
        [ObservableProperty] string password = string.Empty;
        [ObservableProperty] bool rememberMe = false;

        [RelayCommand]
        public async Task OpenUrl(object parameter)
        {
            try
            {
                await Launcher.OpenAsync(parameter.ToString());
            } 
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        [RelayCommand]
        public Task Login()
        {
            if(Username == null | Password == null)
            {
                WeakReferenceMessenger.Default.Send(new OpenPageMessage("Please enter both username and password"));
            } 
            else
            {
                if (Username == DEFAULT_USERNAME && Password == DEFAULT_PASSWORD)
                {
                    IsSignedIn = true;
                }
            }
            return Task.CompletedTask;
        }
    }
}
