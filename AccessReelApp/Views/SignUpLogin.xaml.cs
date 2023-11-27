using AccessReelApp.Messages;
using AccessReelApp.ViewModels;
using CommunityToolkit.Mvvm.Messaging;

namespace AccessReelApp.Views;

public partial class SignUpLogin : ContentPage, IRecipient<OpenPageMessage>
{
	public SignUpLogin(SignUpLoginViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;

        WeakReferenceMessenger.Default.Register(this);
    }

    public async void Receive(OpenPageMessage message)
    {
        if (message.Value == "Invalid Details")
        {
            await Shell.Current.DisplayAlert(message.Value, "Please enter username and password", "OK");
        }
    }

    private async void FacebookImageButton_Clicked(object sender, EventArgs e)
    {
        try
        {
            Uri uri = new Uri(
                "https://accessreel.com/wp/wp-login.php?action=wordpress_social_authenticate&mode=login&provider=Facebook&redirect_to=https%3A%2F%2Faccessreel.com%2Fwp%2Fwp-login.php");

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

    private async void GoogleImageButton_Clicked(object sender, EventArgs e)
    {
        try
        {
            Uri uri = new Uri(
                "https://accessreel.com/wp/wp-login.php?action=wordpress_social_authenticate&mode=login&provider=Google&redirect_to=https%3A%2F%2Faccessreel.com%2Fwp%2Fwp-login.php");

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