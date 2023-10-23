using AccessReelApp.Views;

namespace AccessReelApp;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		MainPage = new AppShell();
	}

	
}
