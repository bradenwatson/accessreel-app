using AccessReelApp.ViewModels;
﻿using AccessReelApp.database_structures;
using Plugin.LocalNotification;
using FirebaseAdmin.Messaging;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using System.Diagnostics;

//Firebase https://firebase.google.com/docs/reference/admin
//Android Setup https://firebase.google.com/docs/cloud-messaging/android/client
//Messaging https://firebase.google.com/docs/cloud-messaging/send-message

namespace AccessReelApp
{
    public class PushNotificationReceived : ValueChangedMessage<string>
    {
        public PushNotificationReceived(string message) : base(message) { }
    }

    public class PushNotificationRequest
    {
        public List<string> registration_ids { get; set; } = new List<string>();
        public NotificationMessageBody notification { get; set; }
        public object data { get; set; }
    }

    public class NotificationMessageBody
    {
        public string title { get; set; }
        public string body { get; set; }
    }

    public partial class MainPage : ContentPage
	{
		DatabaseControl databaseControl = new DatabaseControl();
		int count = 0;
		private string _deviceToken;

		public MainPage(MainViewModel vm)
		{
			InitializeComponent();
			BindingContext = vm;

			WeakReferenceMessenger.Default.Register<PushNotificationReceived>(this, (r, m) =>
			{
				string msg = m.Value;
			});

			if(Preferences.ContainsKey("DeviceToken"))
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
            ReadFireBaseAdminSDK();
        }

		protected override void OnAppearing()
		{
			base.OnAppearing();
			if (BindingContext is MainViewModel vm)
			{
				vm.Text = "Changed!";
			}
		}

        //THIS WORKS!!!!
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
            {
                var androidNotificationObject = new Dictionary<string, string>();
                androidNotificationObject.Add("NavigationID", "2");

                var iosNotificationObject = new Dictionary<string, object>();
                iosNotificationObject.Add("NavigationID", "2");

                var pushNotificationRequest = new PushNotificationRequest
                {
                    notification = new NotificationMessageBody
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
}

