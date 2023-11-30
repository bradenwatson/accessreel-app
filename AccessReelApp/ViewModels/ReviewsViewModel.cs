using AccessReelApp.Messages;
using AccessReelApp.Models;
using AccessReelApp.Prototypes;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccessReelApp.ViewModels
{
    public partial class ReviewsViewModel : ObservableObject, INotifyPropertyChanged
    {
        // Implement view model properties and funcse
        int numberOfMoviesToAddToReviewPage = 10;

        [ObservableProperty]
        ObservableCollection<string> filterList = new()
        {
            "Newest",
            "Oldest",
            "Title (A-Z)",
            "Title (Z-A)",
            "Most Comments",
            "Most Views",
            "Most Followers",
            "Top Site Rated",
            "Top User Rated",
            "Awards",
        };

        [ObservableProperty]
        ObservableCollection<string> dateFilterList = new()
        {
            "Posted any date",
            "Posted in the last year",
            "Posted in the last month",
            "Posted in the last week",
            "Posted in the last day",
        };

        [ObservableProperty]
        ObservableCollection<ReviewCell> movieReviewsList = new ObservableCollection<ReviewCell>();
        public TmdbApiClient movieClient = new("aea36407a9c725c8f82390f7f30064a1");

        public event PropertyChangedEventHandler PropertyChanged;

        private ReviewCell _selectedItem;
        public ReviewCell SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                if (_selectedItem != value)
                {
                    _selectedItem = value;
                    OnPropertyChanged(nameof(SelectedItem));
                    // Perform actions when SelectedItem changes
                    HandleSelectedItemChange(value);
                }
            }
        }

        // other properties, methods, and commands go here

        private async void HandleSelectedItemChange(ReviewCell selectedItem)
        {
            if(selectedItem is null)
            {
                return; // escape
            }

            try
            {
                Uri uri = new(selectedItem.CriticUrl);
                await Browser.Default.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
            }
            catch (Exception)
            {
                WeakReferenceMessenger.Default.Send(new OpenPageMessage("Invalid Url."));
                throw;
            }

            // Deselect the item to remove the highlight
            SelectedItem = null;
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ReviewsViewModel()
        {

            movieClient.ReviewFetched += (review) =>
            {
                // Throttling by introducing a delay between each update
                Task.Delay(TimeSpan.FromMilliseconds(50)).ContinueWith(_ =>
                {
                    Application.Current.Dispatcher.Dispatch(() =>
                    {
                        if (movieReviewsList.Count < numberOfMoviesToAddToReviewPage)
                        {
                            MovieReviewsList.Add(review);
                        }
                    });
                }, TaskScheduler.Default);
            };
        }
    }
}
