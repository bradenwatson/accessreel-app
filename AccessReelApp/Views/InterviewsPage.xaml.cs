using AccessReelApp.ViewModels;

namespace AccessReelApp.Views;

public partial class InterviewsPage : ContentPage
{
	public InterviewsPage(InterviewsViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}