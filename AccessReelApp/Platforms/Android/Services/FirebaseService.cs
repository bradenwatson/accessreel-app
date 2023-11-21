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
            //intent.AddFlags(ActivityFlags.NewTask);

            foreach(var key in data.Keys)
            {
                string value = data[key];
                intent.PutExtra(key, value);
                Debug.WriteLine("*******************************");
                Debug.WriteLine($"Key = {key}\tValue = {value}");
                Debug.WriteLine("*******************************");
            }

            int notificationID = GenerateNotificationID();

            var pendingIntent = PendingIntent.GetActivity(this, notificationID, intent, PendingIntentFlags.Mutable); //Make a mutable one
            Debug.WriteLine("*******************************");
            Debug.WriteLine($"Intent = {pendingIntent}");
            Debug.WriteLine("*******************************");

            var notificationBuilder = new NotificationCompat.Builder(this, MainActivity.Channel_ID)
                .SetContentTitle(title)
                .SetSmallIcon(Resource.Mipmap.appicon)
                .SetContentText(messageBody)
                .SetChannelId(MainActivity.Channel_ID)
                .SetContentIntent(pendingIntent)
                .SetPriority(NotificationCompat.PriorityDefault)
                
                .SetAutoCancel(true);

            var notificationManager = NotificationManagerCompat.From(this);
            notificationManager.Notify(notificationID, notificationBuilder.Build());
            
            /*
            var pendingIntent = PendingIntent.GetActivity(this, MainActivity.NotificationID, intent, PendingIntentFlags.Mutable); //Make a mutable one

            var notificationBuilder = new NotificationCompat.Builder(this, MainActivity.Channel_ID)
                .SetContentTitle(title)
                .SetSmallIcon(Resource.Mipmap.appicon)
                .SetContentText(messageBody)
                .SetChannelId(MainActivity.Channel_ID)
                .SetContentIntent(pendingIntent)
                .SetPriority(NotificationCompat.PriorityDefault);

            var notificationManager = NotificationManagerCompat.From(this);
            notificationManager.Notify(MainActivity.NotificationID, notificationBuilder.Build());
            */
        }

        public static int GenerateNotificationID()
        {
            string time = DateTime.Now.ToLocalTime().ToString("MMddHHmmss");
            return int.Parse(time);
        }

    }
}
