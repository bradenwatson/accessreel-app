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
        MovieMode mode = MovieMode.Popular;
        if(mode == MovieMode.Popular)
        {
            
        }

        GetPopularFilmReviews();
    }

    private async void GetPopularFilmReviews()
    {
        if (BindingContext is ReviewsViewModel vm)
        {
            //await vm.movieClient.GetReviewsForPopularMovies(1);
            for(int i = 0; i < 3; i++)
            {
                await vm.movieClient.GetReviewsForMoviesByName("Saw X", 1); // (1) get only 1 review for this movie
                await vm.movieClient.GetReviewsForMoviesByName("Cats", 1);
                // instead of for, use a pre-defined [OP'OCollection] to match site
                // reviews if wanting
            }
        }
    }
}