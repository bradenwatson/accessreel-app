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
        ObservableCollection<ReviewCell> movieReviewsList = new()
        {
            new ReviewCell
            {
                MovieTitle = "Teenage Mutant Ninja Turtles: Mutant Mayhem Review",
                MovieDescription = "Paramount Pictures and Nickelodeon Films presents their most radical movie this year - Teenage Mutant Ninja Turtles: Mutant Mayhem.",
                PosterUrl = "turtles.jpg",
            },
            new ReviewCell
            {
                MovieTitle = "Teenage Mutant Ninja Turtles: Mutant Mayhem Review",
                MovieDescription = "Paramount Pictures and Nickelodeon Films presents their most radical movie this year - Teenage Mutant Ninja Turtles: Mutant Mayhem.",
                PosterUrl = "turtles.jpg",
            }
        };
    }
}
