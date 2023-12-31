﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Plugin.LocalNotification.NotificationRequestGeofence;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using Microsoft.Maui.Controls;

namespace AccessReelApp.ViewModels
{
    public partial class MapsPageViewModel : ObservableObject
    {
        [ObservableProperty] ObservableCollection<Event> events;
        [ObservableProperty] bool isImageVisible;
        public ICommand EventSelectedCommand { get; }
        public ICommand ToggleImageCommand { get; }

        public MapsPageViewModel()
        {
            // Initialize Events collection with sample data
            Events = new ObservableCollection<Event>
            {
                new Event
                {
                    Title = "Event: Killers of the Flower Moon",
                    Description = "Killers of the Flower Moon is a 2023 American epic western crime drama film directed by Martin Scorsese",
                    StartTime = DateTime.Now.AddHours(1), // Example start time (1 hour from now)
                    Location = "Location: Luna Cinemas",
                },
                new Event
                {
                    Title = "Event: The Marvels",
                    Description = "Carol Danvers gets her powers entangled with those of Kamala Khan and Monica Rambeau, forcing them to work together to save the universe.",
                    StartTime = DateTime.Now.AddHours(2), // Example start time (2 hours from now)
                    Location = "Location: Luna Cinemas",
                },
            };
            EventSelectedCommand = new RelayCommand<Event>(OnEventSelected);
            ToggleImageCommand = new RelayCommand(ToggleImageVisibility);
        }

        private void ToggleImageVisibility()
        {
            IsImageVisible = !IsImageVisible;
        }

        [ObservableProperty] Event selectedEvent;
        private void OnEventSelected(Event selectedEvent)
        {
            if (selectedEvent != null)
            {
                selectedEvent.IsImageVisible = !selectedEvent.IsImageVisible;
                // Update other properties as needed
            }
        }

        public class Event : ObservableObject
        {
            public string Title { get; set; }
            public string Description { get; set; }
            public DateTime StartTime { get; set; }
            public string Location { get; set; }
            public double Latitude { get; set; } // Used for when receiving Latitude and Longitude data
            public double Longitude { get; set; }

            private bool _isImageVisible;
            public bool IsImageVisible
            {
                get => _isImageVisible;
                set => SetProperty(ref _isImageVisible, value);
            }
        }
    }
}

