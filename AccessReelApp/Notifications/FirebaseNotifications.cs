﻿/**************************************************************
 *                  FIREBASE NOTIFICATIONS                   *
 **************************************************************/

/*
  Author: Tony Bui
  Last Updated: 13/11/23
  Class Name: Local Notifications
  Purpose: 
    Create and set customised push notifications based 
    on the user's notification preferences for Android devices. 
    It also features scheduling, ID and badgenumbers.

    This class itself is not an object, and should be called
    as trigger to alert the user or inform the user's interests.

    It should only be used for direct communication to Android
    devices or for new content.

  Notes:
    * Package Requriements:
        * FirebaseAdmin
        * Xamarain.Firebase.Messaging
        * Xamarin.GooglePlayServices.Base
        * Xamarin.Google.Dagger
        * Xamarin.Kotlin.StdLib.Jdk8
        * Xamarin.AndroidX.AppCompat
        * Xamarin.AndroidX.Media
        * Xamarin.ANdroidX.Preference
*/
using CommunityToolkit.Mvvm.Messaging.Messages;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using System.Diagnostics;

namespace AccessReelApp.Notifications
{
    public enum Pages       //Turn into dictionary
    {
        MainPage,
        Page1,
        Page2,
        //NewsPage,
        //ReviewsPage,
        //InterviewPage,
        //SignUpLogin,
        //Movies,
        //Competitions,
        //Settings,
        //Accounts,
    }

    

    // The main class that encapsulates the functionality
    public class NotificationManager
    {
        // Private fields can be declared here
        private string deviceToken;
        private List<Message> messages;
        internal static readonly Dictionary<string, string> PageRoute = new Dictionary<string, string>
        {
            {"General", "MainPage" },
            //{"General", "Page1" },
            //{"General", "Page2" },
            //{"General", "NewsPage" },
            //{"General", "ReviewsPage" },
            //{"General", "InterviewPage" },
            //{"General", "SignUpLogin" },
            //{"General", "Movies" },
            //{"General", "Competitions" },
            //{"General", "Settings" },
            //{"General", "Accounts" },
        };

        // Constructor can be used to initialize class-level variables
        public NotificationManager()
        {
            messages = new List<Message>();
            if(Preferences.ContainsKey("DeviceToken"))
            {
                deviceToken = Preferences.Get("DeviceToken", "");
            }
        }

        // Method to read Firebase Admin SDK
        public static async void ReadFireBaseAdminSDK()
        {
            var stream = await FileSystem.OpenAppPackageFileAsync("Platforms\\Android\\admin_sdk.json");
            var reader = new StreamReader(stream);

            var jsonContent = reader.ReadToEnd();

            if (FirebaseMessaging.DefaultInstance == null)
            {
                FirebaseApp.Create(new AppOptions()
                {
                    Credential = GoogleCredential.FromJson(jsonContent)
                });
            }
        }

        /*
        public static void SwitchByNotification()
        {
            if (Preferences.ContainsKey("NavigationID"))
            {
                string id = Preferences.Get("NavigationID", "");
                if (id == "1")
                {
                    AppShell.Current.GoToAsync(nameof(Page1));
                }
                if (id == "2")
                {
                    AppShell.Current.GoToAsync(nameof(Page2));
                }
                //ADD MORE PAGES
            }
            Preferences.Remove("NavigationID");
        }
        */

        //WORKING (Now needs on notifiication press)
        private void SwitchByNotification()         //Redirects to page on notification interaction     
        {
            if (Preferences.ContainsKey("NavigationID"))
            {
                string id = Preferences.Get("NavigationID", "");
                if (Enum.TryParse(id, out Pages page))
                {
                    NavigateToPage(page);
                }
                else
                {
                    Debug.WriteLine("*******************************");
                    Debug.WriteLine("No matching ID");
                    Debug.WriteLine("*******************************");

                    NavigateToPage(Pages.MainPage);
                }
                Preferences.Remove("NavigationID");
            }
            else
            {
                Debug.WriteLine("*******************************");
                Debug.WriteLine("Key does not exist");
                Debug.WriteLine("*******************************");
            }

        }

        //WOKRING
        private void NavigateToPage(Pages page)
        {
            if (Enum.IsDefined(typeof(Pages), page))
            {
                AppShell.Current.GoToAsync(page.ToString());
                Debug.WriteLine("*******************************");
                Debug.WriteLine($"Navigate to page {page}");
                Debug.WriteLine("*******************************");
            }
            else
            {
                AppShell.Current.GoToAsync(Pages.MainPage.ToString());
                Debug.WriteLine("*******************************");
                Debug.WriteLine($"Page does not exist");
                Debug.WriteLine("*******************************");
            }

        }

        // Method to handle button click event
        /*
        public async void HandleButtonClick()
        {
            var androidNotificationObject = new Dictionary<string, string>();
            androidNotificationObject.Add("NavigationID", "Page1");         //This redirects to app page by its index

            // Create a list of messages
            var messageList = new List<Message>();

            var obj = new Message
            {
                Topic = "",
                Token = deviceToken,
                Notification = new Notification
                {
                    Title = "Go to page 1",
                    Body = "See Title",
                },
                Data = androidNotificationObject,
                // Include additional configurations if needed
            };

            messageList.Add(obj);

            // Send push notification
            var response = await FirebaseMessaging.DefaultInstance.SendAllAsync(messageList);

            //SwitchByNotification();       //WORKING!
        }
        */

        public void SendFCMNotification(object sender, EventArgs e)
        {
            CreateMessage("New method", "Should work");
            SendMessage();
        }

        //WIP
        public void CreateMessageContainer() { messages = new List<Message>(); }


        private void SendToChannel()
        { }

        public void CreateMessage(string title, string body, string topic = "")
        {
            var androidNotificationObject = new Dictionary<string, string>();
            androidNotificationObject.Add("NavigationID", topic);

            if (messages == null) { CreateMessageContainer(); }
            var obj = new Message
            {
                Token = deviceToken,
                Notification = new Notification
                {
                    Title = title,
                    Body = body,
                },
                Data = androidNotificationObject,
            };
            messages.Add(obj);
            //var response = await FirebaseMessaging.DefaultInstance.SendAllAsync(messages);
        }

        public void SendMessage()
        {
            SendAsyncMessages();

            if (FirebaseMessaging.DefaultInstance != null) 
            {
                messages.Clear();
                if (messages.Count == 0)
                {
                    Debug.WriteLine("*******************************");
                    Debug.WriteLine($"{messages.Count} messages remaining.");
                    Debug.WriteLine("*******************************");
                }
                else
                {
                    Debug.WriteLine("*******************************");
                    Debug.WriteLine("Failed to send messages");
                    Debug.WriteLine("*******************************");
                }
            }
            else
            {
                Debug.WriteLine("*******************************");
                Debug.WriteLine("No messages detected");
                Debug.WriteLine("*******************************");
            }
            
        }
        private async void SendAsyncMessages()
        {
            Debug.WriteLine("*******************************");
            Debug.WriteLine($"Sending ({messages.Count}) messages.");
            Debug.WriteLine("*******************************");
            await FirebaseMessaging.DefaultInstance.SendAllAsync(messages);
        } 

    }


    //public class PushNotificationReceived : ValueChangedMessage<string>
    //{
    //    public PushNotificationReceived(string message) : base(message) { }
    //}

    //public class PushNotificationRequest
    //{
    //    //public string notificationID { get; set; }
    //    public List<string> registration_ids { get; set; } = new List<string>();
    //    public NotificationMessageBody notification { get; set; }
    //    public object data { get; set; }
    //}

    //public class NotificationMessageBody
    //{
    //    public string title { get; set; }
    //    public string body { get; set; }

    //    //public string subtitle {  get; set; }
    //}

    //private string _deviceToken;

    //private void DeviceToken()
    //{
    //    if (Preferences.ContainsKey("DeviceToken"))
    //    {
    //        _deviceToken = Preferences.Get("DeviceToken", "");
    //    }
    //}


    //private async void ReadFireBaseAdminSDK()
    //{
    //    var stream = await FileSystem.OpenAppPackageFileAsync("Platforms\\Android\\admin_sdk.json");
    //    var reader = new StreamReader(stream);

    //    var jsonContent = reader.ReadToEnd();

    //    if (FirebaseMessaging.DefaultInstance == null)
    //    {
    //        FirebaseApp.Create(new AppOptions()
    //        {
    //            Credential = GoogleCredential.FromJson(jsonContent)
    //        });
    //    }
    //}




    //private async void Button_Clicked_1(object sender, EventArgs e)
    //{
    //    //Setup authorisation https://firebase.google.com/docs/cloud-messaging/migrate-v1

    //    var androidNotificationObject = new Dictionary<string, string>();
    //    androidNotificationObject.Add("NavigationID", "2");


    //    var pushNotificationRequest = new PushNotificationRequest
    //    {
    //        notification = new NotificationMessageBody
    //        {
    //            title = "Notification Title",
    //            body = "Notification body"
    //        },
    //        data = androidNotificationObject,
    //        registration_ids = new List<string> { _deviceToken }
    //    };



    //    var messageList = new List<Message>();

    //    var obj = new Message
    //    {
    //        Token = _deviceToken,
    //        Notification = new Notification
    //        {
    //            Title = "Tilte",
    //            Body = "message body"
    //        },
    //        Data = androidNotificationObject,
    //        //Apns = new ApnsConfig()
    //        //{
    //        //    Aps = new Aps
    //        //    {
    //        //        Badge = 15,
    //        //        //CustomData = iosNotificationObject,
    //        //    }
    //        //}
    //    };

    //    messageList.Add(obj);

    //    var response = await FirebaseMessaging.DefaultInstance.SendAllAsync(messageList);
    //}


}
