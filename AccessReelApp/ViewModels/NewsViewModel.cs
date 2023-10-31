using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccessReelApp.ViewModels
{
    public partial class NewsViewModel : ObservableObject
    {
        // Implement view model properties and funcs

        [ObservableProperty]
        ObservableCollection<string> myList = new()
        {
            "Item A",
            "Item B",
            "Item C",
            "Item D",
        };
    }
}
