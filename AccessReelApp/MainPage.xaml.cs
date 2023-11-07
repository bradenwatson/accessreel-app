using AccessReelApp.ViewModels;
﻿using AccessReelApp.database_structures;
using Plugin.LocalNotification;
using FirebaseAdmin.Messaging;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using System.Diagnostics;
using AccessReelApp.Prototypes;
using Newtonsoft.Json;
using RestSharp;
using Newtonsoft.Json.Linq;

//Firebase https://firebase.google.com/docs/reference/admin
//Android Setup https://firebase.google.com/docs/cloud-messaging/android/client
//Messaging https://firebase.google.com/docs/cloud-messaging/send-message

namespace AccessReelApp
{
    public partial class MainPage : ContentPage
	{
        // Unused?
        int count = 0;
        bool isApiKeyValid;
        public TmdbApiClient movieClient = new("aea36407a9c725c8f82390f7f30064a1");
        DatabaseControl databaseControl = new DatabaseControl();
		private string _deviceToken;

		public MainPage(MainViewModel vm)
		{
			InitializeComponent();
			BindingContext = vm;

			WeakReferenceMessenger.Default.Register<PushNotificationReceived>(this, (r, m) =>
			{
				string msg = m.Value;
			});

            RootTests();
            //ReadFireBaseAdminSDK();
        }

        private void RootTests() // seperates code a little bit
        {
            if (Preferences.ContainsKey("DeviceToken"))
            {
                _deviceToken = Preferences.Get("DeviceToken", "");
            }

            string rootDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string relativePath = Path.Combine("Platforms", "Android", "Resources", "admin_sdk.json");
            string fullPath = Path.Combine(rootDirectory, relativePath);

            Debug.WriteLine("**************************************************************");
            Debug.WriteLine($"root dir = {rootDirectory}");
            Debug.WriteLine($"rel path = {relativePath}");
            Debug.WriteLine($"full path = {fullPath}");
            Debug.WriteLine("**************************************************************");
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

        private async void GetReviewsByName(string name)
        {
            string movieName = name; // Replace with the movie name you want to search for
            await movieClient.GetMovieReviewsByName(movieName);
        }


        // get reviews from most recent/popular movies

        private async void GetPopularFilmReviews()
        {
            await movieClient.GetReviewsForPopularMovies(1);
        }

        private void Button_Clicked(object sender, EventArgs e)
        {

			//DOES THIS PLUGIN WORK FOR FIREBASE
			var request = new NotificationRequest
			{
				NotificationId = 1337,
				Title = "Hello World",
				Subtitle = "Test",
				Description = "Working",
				BadgeNumber = 42,
				Schedule = new NotificationRequestSchedule
				{
					NotifyTime = DateTime.Now.AddSeconds(5),
					NotifyRepeatInterval = TimeSpan.FromDays(1),
				}
			};
			LocalNotificationCenter.Current.Show(request);
        }

		private async void ReadFireBaseAdminSDK()
		{
            string relativePath = "Platforms/Android/admin_sdk.json";
            var stream = await FileSystem.OpenAppPackageFileAsync(relativePath);
			var reader = new StreamReader(stream);

			var jsonContent = reader.ReadToEnd();

			if(FirebaseMessaging.DefaultInstance == null)
			{
				FirebaseApp.Create(new AppOptions()
				{
					Credential = GoogleCredential.FromJson(jsonContent)
				}); 
			}
		}

        private async void Button_Clicked_1(object sender, EventArgs e)
        {
            var androidNotificationObject = new Dictionary<string, string>();
            androidNotificationObject.Add("NavigationID", "2");

            var iosNotificationObject = new Dictionary<string, object>();
            iosNotificationObject.Add("NavigationID", "2");

            var pushNotificationRequest = new PushNotificationRequest
            {
                notification = new NotificationMessageBody1
                {
                    title = "Notification Title",
                    body = "Notification body"
                },
                data = androidNotificationObject,
                registration_ids = new List<string> { _deviceToken }
            };

            var messageList = new List<Message>();

            var obj = new Message
            {
                Token = _deviceToken,
                Notification = new Notification
                {
                    Title = "Tilte",
                    Body = "message body"
                },
                Data = androidNotificationObject,
                Apns = new ApnsConfig()
                {
                    Aps = new Aps
                    {
                        Badge = 15,
                        CustomData = iosNotificationObject,
                    }
                }
            };

            messageList.Add(obj);

            var response = await FirebaseMessaging.DefaultInstance.SendAllAsync(messageList);
        }
    }
}

