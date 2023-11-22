using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Maui.Maps;
using Microsoft.Maui.Controls.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Plugin.LocalNotification.NotificationRequestGeofence;

namespace AccessReelApp.ViewModels
{
    public partial class MapsPageViewModel : ObservableObject
    {
        // Define an initial location for the map
        [ObservableProperty]
        public MapSpan initialMapLocation;

        public MapsPageViewModel()
        {
            // Set an initial location for the map when the ViewModel is created
            InitialMapLocation = MapSpan.FromCenterAndRadius(
                new Location(-33.8688, 151.2093), 
                Distance.FromKilometers(50));    // Zoom level 
        }
    }
}