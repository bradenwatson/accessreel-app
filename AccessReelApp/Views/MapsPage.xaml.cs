using AccessReelApp.ViewModels;
using static Plugin.LocalNotification.NotificationRequestGeofence;
using Microsoft.Maui.Controls;

namespace AccessReelApp.Views;

public partial class MapsPage : ContentPage
{
	public MapsPage(MapsPageViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
    }

    // methods

    protected override void OnAppearing()
    {
        base.OnAppearing();
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
    }
}