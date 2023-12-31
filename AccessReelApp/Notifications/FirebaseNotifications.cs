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
using CommunityToolkit.Maui.Converters;
using CommunityToolkit.Mvvm.Messaging.Messages;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace AccessReelApp.Notifications
{
    // The main class that encapsulates the functionality
    public class NotificationManager
    {
        // Private fields can be declared here
        private string deviceToken;
        //private List<Message> messages;

        // Constructor can be used to initialize class-level variables
        public NotificationManager()
        {
            //messages = new List<Message>();


            if (Preferences.ContainsKey("DeviceToken"))
            {
                deviceToken = Preferences.Get("DeviceToken", "");
            }
        }

        // Method to read Firebase Admin SDK
        public static async Task ReadFireBaseAdminSDK()
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

        public void SendFCMNotification(object sender, EventArgs e)
        {
            CreateMessage("New method", "Should work");
            //SendMessage();
        }


        //public void CreateMessageContainer() { messages = new List<Message>(); }

        public async void CreateMessage(string title, string body, string key = "NewsPage")       //Must have a channel otherwise fails
        {
            var androidNotificationObject = new Dictionary<string, string>();
            androidNotificationObject.Add("NavigationID", key);

            if (!String.IsNullOrEmpty(deviceToken))
            {
                var messages = new List<Message>();
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
                Debug.WriteLine(messages);

                //Notifications should still be send and received regardless of preferrence.
                //You can only deny popup only.
                await FirebaseMessaging.DefaultInstance.SendAllAsync(messages);
            }
        }
    }

}
