using AccessReelApp.ViewModels;

namespace AccessReelApp.Views;

public partial class NewsPage : ContentPage
{
	public NewsPage(NewsViewModel vm)
	{
		InitializeComponent();
        ViewModelLocator.NewsViewModelInstance = vm;
        BindingContext = vm;


        // Add ItemTapped event handlers for both ListView MSR
        newsListView.ItemTapped += OnNewsItemTapped;
        trailersListView.ItemTapped += OnTrailerItemTapped;

    }

    // This will open the url of the news item as defined in the news object of the listview MSR
    private async void OnNewsItemTapped(object sender, ItemTappedEventArgs e)
    {
        if (e.Item is NewsItem selectedNewsItem)
        {
            try
            {
                Uri uri = new Uri(selectedNewsItem.NewsUrl);
                await Browser.Default.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);

                // Deselect the item to remove the highlight
                newsListView.SelectedItem = null;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Unable to open the url", "ok");
            }

        }
    }

    private async void OnTrailerItemTapped(object sender, ItemTappedEventArgs e)
    {
        if (e.Item is NewsItem selecteTrailerItem)
        {
            try
            {
                //Uri uri = new Uri(selectedTrailerItem.TrailerUrl);  // Trailer itmes need to be updated with a url for this to work
                //await Browser.Default.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);

                // Deselect the item to remove the highlight
                newsListView.SelectedItem = null;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Unable to open the url", "ok");
            }
        }
    }
}
