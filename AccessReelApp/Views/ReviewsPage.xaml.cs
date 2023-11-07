using AccessReelApp.ViewModels;

namespace AccessReelApp.Views;

public partial class ReviewsPage : ContentPage
{
    public enum MovieMode
    {
        Popular,
        AccessReelOrdered,
    };

	public ReviewsPage(ReviewsViewModel vm)
	{
        InitializeComponent();
		BindingContext = vm;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        MovieMode mode = MovieMode.AccessReelOrdered;
        if(mode == MovieMode.Popular)
        {
            GetPopularFilmReviews();
        }

        if(mode == MovieMode.AccessReelOrdered)
        {
            GetMovieReviewsBySiteOrder();
        }
    }

    private async void GetPopularFilmReviews()
    {
        if (BindingContext is ReviewsViewModel vm)
        {
            await vm.movieClient.GetReviewsForPopularMovies(2);
        }
    }

    private async void GetMovieReviewsBySiteOrder()
    {
        if (BindingContext is ReviewsViewModel vm)
        {
            await vm.movieClient.GetReviewsForMoviesByName(new string[] { "The Dive" }, 1);
        }
    }
}