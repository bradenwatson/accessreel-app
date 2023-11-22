using AccessReelApp.ViewModels;
using Plugin.LocalNotification;
using FirebaseAdmin.Messaging;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using CommunityToolkit.Mvvm.Messaging;
using System.Diagnostics;
using AccessReelApp.Prototypes;
using AccessReelApp.Notifications;

namespace AccessReelApp
{
    public partial class MainPage : ContentPage
    {

        bool isApiKeyValid;
        public TmdbApiClient movieClient = new("aea36407a9c725c8f82390f7f30064a1");
        DatabaseControl databaseControl = new DatabaseControl();
        NotificationManager notificationManager = new NotificationManager();
        

        public MainPage(MainViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;

            //DeviceToken();
            NotificationManager.ReadFireBaseAdminSDK();

            //Preferences.Clear("NavigationID");
        }


        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (BindingContext is MainViewModel vm)
            {
                vm.Text = "Changed!";
            }
            // movie tests
            IsAPIValidCheck();
            //GetPopularFilms();
            //GetFilmByName("The Matrix");
            //GetReviewsByName("Five Nights at Freddy's");
            //GetPopularFilmReviews();
        }

        // Testing methods ------>

        // Check if the key is valid before we do anything else
        private async void IsAPIValidCheck()
        {
            isApiKeyValid = await movieClient.IsApiKeyValid();
            Debug.WriteLine($"Is API Valid: {isApiKeyValid}");
        }

        // Get the most popular movies atm

        private async void GetPopularFilms()
        {
            List<Movie> popularMovies = await movieClient.GetPopularMovies();
            if (popularMovies != null)
            {
                Debug.WriteLine("Popular Movies:");
                foreach (var movie in popularMovies)
                {
                    Debug.WriteLine($"Title: {movie.Title}");
                    Debug.WriteLine($"Overview: {movie.Overview}");
                    Debug.WriteLine($"Release Date: {movie.ReleaseDate}");
                    Debug.WriteLine("--------------");
                }
            }
            else
            {
                Debug.WriteLine("Failed to retrieve popular movies.");
            }
        }

        // search a film by name (if you need to)

        private async void GetFilmByName(string name)
        {
            Movie movieDetails = await movieClient.GetMovieDetailsByName(name);
            if (movieDetails != null)
            {
                Debug.WriteLine("Movie Title: " + movieDetails.Title);
                Debug.WriteLine("Overview: " + movieDetails.Overview);
            }
            else
            {
                Debug.WriteLine("Movie not found or an error occurred.");
            }
        }

        // get reviews from movie by its name 


        // get reviews from most recent/popular movies

        private async void GetPopularFilmReviews()
        {
            await movieClient.GetReviewsForPopularMovies(1);
        }





        private void Button_Clicked(object sender, EventArgs e)
        {
            NotificationHandler.BellButton(sender,e);

        }

        // Button click event for the second notification (using NotificationHandler)
        private void Button_Clicked_1(object sender, EventArgs e)
        {
            //notificationManager.HandleButtonClick();
            notificationManager.SendFCMNotification(sender, e);
        }

    } 
}

