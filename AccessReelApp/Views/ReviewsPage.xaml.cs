using AccessReelApp.ViewModels;

namespace AccessReelApp.Views;

public partial class ReviewsPage : ContentPage
{
	public ReviewsPage(ReviewsViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
    }

   
}