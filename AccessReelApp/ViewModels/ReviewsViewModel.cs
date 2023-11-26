using AccessReelApp.Models;
using AccessReelApp.Prototypes;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccessReelApp.ViewModels
{
    public partial class ReviewsViewModel : ObservableObject
    {
        // Implement view model properties and funcse

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
        public ReviewsViewModel()
        {
            movieClient.ReviewFetched += (review) =>
            {
                // Throttling by introducing a delay between each update
                Task.Delay(TimeSpan.FromMilliseconds(50)).ContinueWith(_ =>
                {
                    Application.Current.Dispatcher.Dispatch(() =>
                    {
                        MovieReviewsList.Add(review);
                    });
                }, TaskScheduler.Default);
            };
        }
    }
}
