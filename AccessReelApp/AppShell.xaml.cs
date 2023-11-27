using AccessReelApp.Views;

namespace AccessReelApp;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
		Routing.RegisterRoute(nameof(NewPage1), typeof(NewPage1));
		Routing.RegisterRoute(nameof(NewsPage), typeof(NewsPage)); // register route for a page
		Routing.RegisterRoute(nameof(ReviewsPage), typeof(ReviewsPage));
		Routing.RegisterRoute(nameof(InterviewsPage), typeof(InterviewsPage));
		Routing.RegisterRoute(nameof(SignUpLogin), typeof(SignUpLogin));
    }

    private async void FacebookIcon_Clicked(object sender, EventArgs e)
    {
        try
        {
            Uri uri = new Uri("https://www.facebook.com/accessreel");
            BrowserLaunchOptions options = new BrowserLaunchOptions()
            {
                LaunchMode = BrowserLaunchMode.SystemPreferred,
                TitleMode = BrowserTitleMode.Show,
                PreferredToolbarColor = Colors.Violet,
                PreferredControlColor = Colors.SandyBrown
            };

            await Browser.Default.OpenAsync(uri, options);
        }
        catch { return; }
    }

    private async void InstagramIcon_Clicked(object sender, EventArgs e)
    {
        try
        {
            Uri uri = new Uri("https://www.instagram.com/accessreel.com_official/");
            BrowserLaunchOptions options = new BrowserLaunchOptions()
            {
                LaunchMode = BrowserLaunchMode.SystemPreferred,
                TitleMode = BrowserTitleMode.Show,
                PreferredToolbarColor = Colors.Violet,
                PreferredControlColor = Colors.SandyBrown
            };

            await Browser.Default.OpenAsync(uri, options);
        }
        catch { return; }
    }

    private async void XIcon_Clicked(object sender, EventArgs e)
    {
        try
        {
            Uri uri = new Uri("https://twitter.com/accessreel");
            BrowserLaunchOptions options = new BrowserLaunchOptions()
            {
                LaunchMode = BrowserLaunchMode.SystemPreferred,
                TitleMode = BrowserTitleMode.Show,
                PreferredToolbarColor = Colors.Violet,
                PreferredControlColor = Colors.SandyBrown
            };

            await Browser.Default.OpenAsync(uri, options);
        }
        catch { return; }
    }

    private async void YoutubeIcon_Clicked(object sender, EventArgs e)
    {
        try
        {
            Uri uri = new Uri("https://www.youtube.com/@AccessReel");
            BrowserLaunchOptions options = new BrowserLaunchOptions()
            {
                LaunchMode = BrowserLaunchMode.SystemPreferred,
                TitleMode = BrowserTitleMode.Show,
                PreferredToolbarColor = Colors.Violet,
                PreferredControlColor = Colors.SandyBrown
            };

            await Browser.Default.OpenAsync(uri, options);
        }
        catch { return; }
    }
}
