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
}