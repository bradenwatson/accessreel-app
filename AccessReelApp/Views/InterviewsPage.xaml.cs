using AccessReelApp.database_structures;
using AccessReelApp.Messages;
using AccessReelApp.ViewModels;
using CommunityToolkit.Mvvm.Messaging;
using System.Diagnostics;
using System.Windows.Input;

namespace AccessReelApp.Views;

public partial class InterviewsPage : ContentPage
{
    //DatabaseControl databaseControl = new DatabaseControl();
    //public ICommand TapCommand => new Command<string>(async (url) => await Launcher.OpenAsync(url));

    public InterviewsPage(InterviewsViewModel vm)
    {
        InitializeComponent();
        ViewModelLocator.InterviewsViewModelInstance = vm;
        BindingContext = vm;

        // Add ItemTapped event handlers for both ListView MSR
        interviewsListView.ItemTapped += interviewsListView_ItemTapped;

    }

    // This will open the url of the news item as defined in the news object of the listview MSR

    private async void interviewsListView_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        if (e.Item is InterviewsItem selectedInterviewsItem)
        {
            try
            {
                //Uri uri = new Uri(selectedInterviewsItem.InterviewsUrl);
                await Browser.OpenAsync(selectedInterviewsItem.InterviewsUrl, BrowserLaunchMode.SystemPreferred);
                // Deselect the item to remove the highlight
                interviewsListView.SelectedItem = null;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Unable to open the url", "ok");
            }

        }
    }
}