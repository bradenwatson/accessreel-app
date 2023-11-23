using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Maui.Maps;
using Microsoft.Maui.Controls.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Plugin.LocalNotification.NotificationRequestGeofence;
using System.Collections.ObjectModel;

namespace AccessReelApp.ViewModels
{
    public partial class MapsPageViewModel : ObservableObject
    {
        [ObservableProperty] ObservableCollection<Event> events;

        public MapsPageViewModel()
        {
            // Initialize Events collection with sample data
            Events = new ObservableCollection<Event>
            {
                new Event
                {
                    Title = "Event 1",
                    Description = "Description of Event 1",
                    StartTime = DateTime.Now.AddHours(1), // Example start time (1 hour from now)
                    Location = "Location 1"
                },
                new Event
                {
                    Title = "Event 2",
                    Description = "Description of Event 2",
                    StartTime = DateTime.Now.AddHours(2), // Example start time (2 hours from now)
                    Location = "Location 2"
                },
                // Add more events as needed
            };
        }
    }

    public class Event
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartTime { get; set; }
        public string Location { get; set; }
    }
}