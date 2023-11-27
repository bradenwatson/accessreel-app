/**************************************************************
 *                      FIREBASE SERVICE                   *
 **************************************************************/

/*
  Author: Tony Bui
  Last Updated: 27/11/23
  Class Name: Firebase Service
  Purpose: 
    Controls how the notifications are sent to the FCM server
    to the device.

  Notes:
    * 
*/

using AccessReelApp.Notifications;
using AccessReelApp.ViewModels;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using AndroidX.Core.App;
using Firebase.Messaging;
using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Platform;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccessReelApp.Platforms.Android.Services
{
    [Service(Exported = true)]
    [IntentFilter(new[] {"com.google.firebase.MESSAGING_EVENT"})]
    public class FirebaseService: FirebaseMessagingService
    {
        public override void OnNewToken(string token)
        {
            base.OnNewToken(token);
            if(Preferences.ContainsKey("DeviceToken"))
            {
                Preferences.Remove("DeviceToken");
            }
            Preferences.Set("DeviceToken", token);
        }

        public override void OnMessageReceived(RemoteMessage message)
        {
            base.OnMessageReceived(message);
            var notification = message.GetNotification();
            Debug.WriteLine("*******************************");
            Debug.WriteLine($"Notification = {notification}");
            Debug.WriteLine($"Message = {message.Data}");
            Debug.WriteLine("*******************************");


            

            //Messages 
            if (NotificationSettings.CheckNotificationEnabled())
            {
                SendNotification(message.Data, notification.Title, notification.Body);
                
            }
            else
            {
                //Notifications will not be stored and will miss out
                Debug.WriteLine("*******************************");
                Debug.WriteLine($"User has disabled notifications");
                Debug.WriteLine("*******************************");
            }

        }


        public static int GenerateNotificationID()
        {
            string time = DateTime.Now.ToLocalTime().ToString("MMddHHmmss");
            return int.Parse(time);
        }

        private void SendNotification(IDictionary<string, string> data, string title, string messageBody) 
        {
            var intent = new Intent(this,typeof(MainActivity));
            intent.AddFlags(ActivityFlags.ClearTop);

            string page = string.Empty;
            foreach(var key in data.Keys)
            {
                string value = data[key];
                var selectedChannel = MainActivity.Channels.SingleOrDefault(x => x.Id == value);
                if(selectedChannel != null) 
                {
                    page = selectedChannel.Id.ToString();  //Default to first index if not found
                                           //Check if key in dictionay channel
                    intent.PutExtra(key, page);
                }
                else
                {
                    intent.PutExtra(key, MainActivity.Pages.MainPage.ToString());   //Failsafe if prior calls fail
                }
                
                Debug.WriteLine("*******************************");
                Debug.WriteLine($"Key = {key}\tValue = {value}\tChannel ID = {page}");
                Debug.WriteLine("*******************************");
            }



            int notificationID = GenerateNotificationID();
            Debug.WriteLine("*******************************");
            Debug.WriteLine($"Msg ID = {notificationID}");
            Debug.WriteLine("*******************************");


            var pendingIntent = PendingIntent.GetActivity(this, notificationID, intent, PendingIntentFlags.Mutable); //Make a mutable one
            Debug.WriteLine("*******************************");
            Debug.WriteLine($"Intent = {pendingIntent}");
            Debug.WriteLine("*******************************");

            var notificationBuilder = new NotificationCompat.Builder(this, page)
                .SetContentTitle(title)
                .SetSmallIcon(Resource.Mipmap.appicon)
                .SetContentText(messageBody)
                .SetChannelId(page)
                .SetContentIntent(pendingIntent)
                .SetPriority(NotificationCompat.PriorityDefault)
                //.SetWhen()        //Should be set by current time plus the frequency
                .SetAutoCancel(true);

            var notificationManager = NotificationManagerCompat.From(this);
            notificationManager.Notify(notificationID, notificationBuilder.Build());
        }
        
    }
}
