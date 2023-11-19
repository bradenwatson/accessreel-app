using Android.App;
using Android.Content;
using Android.Graphics;
using AndroidX.Core.App;
using AndroidX.Core.Graphics.Drawable;
using Firebase.Messaging;
using System;
using System.Collections.Generic;
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

            SendNotification(notification.Body, notification.Title, message.Data);
        }

        private void SendNotification(string messageBody, string title, IDictionary<string, string> data) 
        {
            //Check content to set channel or just have 1 channel

            var intent = new Intent(this,typeof(MainActivity));
            intent.AddFlags(ActivityFlags.ClearTop);

            foreach(var key in data.Keys)
            {
                string value = data[key];
                intent.PutExtra(key, value);
            }


            var pendingIntent = PendingIntent.GetActivity(this, MainActivity.NotificationID, intent, PendingIntentFlags.Mutable);   //Messages can be editted

            
            var notificationBuilder = new NotificationCompat.Builder(this, MainActivity.Channel_ID)
                .SetContentTitle(title)
                .SetSmallIcon(Resource.Mipmap.appicon)
                .SetContentText(messageBody)
                //.SetChannelId(MainActivity.Channel_IDs)
                .SetChannelId(MainActivity.Channel_ID)
                .SetContentIntent(pendingIntent)
                .SetPriority(NotificationCompat.PriorityDefault);

            var notificationManager = NotificationManagerCompat.From(this);
            notificationManager.Notify(MainActivity.NotificationID, notificationBuilder.Build());
        }



    }
}
