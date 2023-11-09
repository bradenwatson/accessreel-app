using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            if(parameter is string paramValue)
            {
                await Task.Delay(2); // replace with code.
            }
        }

        [RelayCommand]
        public async Task Login()
        {
            await Task.Delay(2); // replace with code.
        }
    }
}

