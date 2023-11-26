using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccessReelApp.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        [ObservableProperty] string text = "Initial!"; // the text value binded to the XAML.

        // e
    }
}
