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
            //if(Username == nullorEmpty | Password == null)
            if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password))
            {
                WeakReferenceMessenger.Default.Send(new OpenPageMessage("Invalid Details"));
                return Task.CompletedTask;
            } // this is an escape clause no need for an else

            // Perform any login logic here
            // can await an authentication service etc
            // any handle success/failure
            return Task.CompletedTask;
        }
    }
}

