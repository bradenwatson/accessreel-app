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
            SendNotification(message.Data, notification.Title, notification.Body);
        }

        
        //Sort Content here
        private void SendNotification(IDictionary<string, string> data, string title, string messageBody) 
        {
            var intent = new Intent(this,typeof(MainActivity));
            intent.AddFlags(ActivityFlags.ClearTop);

            string page = string.Empty;
            foreach (var key in data.Keys)           //Use to Navigate Pages by dictionary entry value
            {
                string value = data[key];
                var tmp = MainActivity.Channels.SingleOrDefault(x => x.Id == value);
                if(tmp != null) 
                {
                    page = tmp.Id.ToString();  //Default to first index if not found
                                           //Check if key in dictionay channel
                    intent.PutExtra(key, page);
                }
                else
                {
                    intent.PutExtra(key, value);
                }
                
                Debug.WriteLine("*******************************");
                Debug.WriteLine($"Key = {key}\tValue = {value}\tChannel ID = {page}");
                Debug.WriteLine("*******************************");
            }

            
            //Obtain specific dict entry value from the intent so that can select channel. If nothing, send to general


            //string channelID = MainActivity.Channels.FirstOrDefault(x => x.Id == "News").Id; //WIP  //This should match 
            //Debug.WriteLine("*******************************");
            //Debug.WriteLine($"Channel ID = {channelID}");
            //Debug.WriteLine("*******************************");


            int notificationID = GenerateNotificationID();
            Debug.WriteLine("*******************************");
            Debug.WriteLine($"Msg ID = {notificationID}");
            Debug.WriteLine("*******************************");


            var pendingIntent = PendingIntent.GetActivity(this, notificationID, intent, PendingIntentFlags.Mutable | PendingIntentFlags.UpdateCurrent); //Make a mutable one
            Debug.WriteLine("*******************************");
            Debug.WriteLine($"Intent = {pendingIntent}");
            Debug.WriteLine("*******************************");

            var notificationBuilder = new NotificationCompat.Builder(this, page)
                .SetContentTitle(title)
                .SetSmallIcon(Resource.Mipmap.appicon)
                .SetContentText(messageBody)
                //.SetChannelId(MainActivity.Channel_ID)
                .SetChannelId(page)
                .SetContentIntent(pendingIntent)
                .SetPriority(NotificationCompat.PriorityDefault)
                .SetAutoCancel(true);

            var notificationManager = NotificationManagerCompat.From(this);
            notificationManager.Notify(notificationID, notificationBuilder.Build());

        }

        public static int GenerateNotificationID()
        {
            string time = DateTime.Now.ToLocalTime().ToString("MMddHHmmss");
            return int.Parse(time);
        }


        
    }
}
