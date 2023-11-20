using AccessReelApp.Views;

namespace AccessReelApp;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
		Routing.RegisterRoute(nameof(Page1), typeof(Page1));
        Routing.RegisterRoute(nameof(Page2), typeof(Page2));
        Routing.RegisterRoute(nameof(NewsPage), typeof(NewsPage)); // register route for a page
		Routing.RegisterRoute(nameof(ReviewsPage), typeof(ReviewsPage));
		Routing.RegisterRoute(nameof(InterviewsPage), typeof(InterviewsPage));
		Routing.RegisterRoute(nameof(SignUpLogin), typeof(SignUpLogin));
    }
}
