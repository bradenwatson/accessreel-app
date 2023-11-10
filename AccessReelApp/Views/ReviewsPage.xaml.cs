using AccessReelApp.ViewModels;

namespace AccessReelApp.Views;

public partial class ReviewsPage : ContentPage
{
	public ReviewsPage(ReviewsViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;

		LoadARReviews();
    }

	private async void LoadARReviews()
	{
		if(BindingContext is ReviewsViewModel vm)
		{
			await vm.movieClient.PullAccessReelData();
		}
	}
}