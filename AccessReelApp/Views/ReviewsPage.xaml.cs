using AccessReelApp.Messages;
using AccessReelApp.ViewModels;
using CommunityToolkit.Mvvm.Messaging;

namespace AccessReelApp.Views;

public partial class ReviewsPage : ContentPage, IRecipient<OpenPageMessage>
{
    public enum MovieMode
    {
        Popular,
        AccessReelOrdered,
    };

	public ReviewsPage(ReviewsViewModel vm)
	{
        InitializeComponent();
		BindingContext = vm;
        WeakReferenceMessenger.Default.Register(this);
        LoadARReviews();
    }

	private async void LoadARReviews()
	{
		if(BindingContext is ReviewsViewModel vm)
		{
			await vm.movieClient.PullAccessReelData();
		}
	}

    public async void Receive(OpenPageMessage message)
    {
        if (message.Value == "Invalid Url")
        {
            await Shell.Current.DisplayAlert(message.Value, "The item's url seems to be invalid.", "OK");
        }
    }
}