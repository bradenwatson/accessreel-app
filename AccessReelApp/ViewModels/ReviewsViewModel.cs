using AccessReelApp.Models;
using AccessReelApp.Prototypes;
using AccessReelApp.Views;
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
        // Implement view model properties and funcs

        // Implement view model properties and funcs

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

        ObservableCollection<ReviewCell> filteredReviewsList;
        public ObservableCollection<ReviewCell> FilteredReviewsList
        {
            get { return filteredReviewsList; }
            set
            {
                if (filteredReviewsList != value)
                {
                    filteredReviewsList = value;
                    OnPropertyChanged(nameof(FilteredReviewsList));
                }
            }
        }

        string selectFilter;
        public string SelectedFilter
        {
            get { return selectFilter; }
            set
            {
                if (selectFilter != value)
                {
                    selectFilter = value;
                    ApplyFilter();
                    OnPropertyChanged(nameof(SelectedFilter));
                }
            }
        }

        public TmdbApiClient movieClient = new("aea36407a9c725c8f82390f7f30064a1");
        public ReviewsViewModel()
        {
            movieClient.ReviewFetched += (review) =>
            {
                Application.Current.Dispatcher.Dispatch(() =>
                {
                    MovieReviewsList.Add(review);
                });
            };

            FilmInfo filmInfo = new FilmInfo();
            
            FilteredReviewsList = MovieReviewsList;
        }

        void ApplyFilter()
        {
            switch (SelectedFilter)
            {
                case "Newest":
                    FilteredReviewsList = new ObservableCollection<ReviewCell>(MovieReviewsList.OrderByDescending(m => m.PostDate));
                    break;

                case "Oldest":
                    FilteredReviewsList = new ObservableCollection<ReviewCell>(MovieReviewsList.OrderBy(m => m.PostDate));
                    break;

                default:
                    break;
            }
        }
    }
}
