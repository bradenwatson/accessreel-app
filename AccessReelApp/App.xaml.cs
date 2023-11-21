using AccessReelApp.Views;

namespace AccessReelApp;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		MainPage = new AppShell();
	}

    protected override void OnStart()
    {
        // App is starting or returning to the foreground
        base.OnStart();
    }

    protected override void OnSleep()
    {
        // App is going to the background
        base.OnSleep();
    }

    protected override void OnResume()
    {
        // App is resuming from the background
        base.OnResume();
    }
}
