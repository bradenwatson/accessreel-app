using AccessReelApp.ViewModels;

namespace AccessReelApp.Views;

public partial class ReviewsPage : ContentPage
{
	public ReviewsPage(ReviewsViewModel vm)
	{
        InitializeComponent();
		BindingContext = vm;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if(BindingContext is ReviewsViewModel vm)
        {
            GetPopularFilmReviews();
        }
    }

    private async void GetPopularFilmReviews()
    {
        if (BindingContext is ReviewsViewModel vm)
        {
            await vm.movieClient.GetReviewsForPopularMovies(1);
        }
    }
}