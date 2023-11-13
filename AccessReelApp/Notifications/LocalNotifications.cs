/**************************************************************
 *                      LOCAL NOTIFICATIONS                   *
 **************************************************************/

/*
  Author: Tony Bui, Thomas Chen
  Last Updated: 13/11/23
  Class Name: Local Notifications
  Purpose: 
    Create and set customised local push notifications based 
    on the user's notification preferences for Android and 
    iOS devices. It also features scheduling, ID and 
    badgenumbers.

    This class itself is not an object, and should be called
    as trigger to alert the user or inform the user's interests.

  Notes:
    * Requires "Plugin.LocalNotification" package
*/

using CommunityToolkit.Mvvm.Messaging.Messages;
using Plugin.LocalNotification;

namespace AccessReelApp.Notifications
{
    //private void Button_Clicked(object sender, EventArgs e)
    //{

    //    //DOES THIS PLUGIN WORK FOR FIREBASE
    //    var request = new NotificationRequest
    //    {
    //        NotificationId = 1337,
    //        Title = "Hello World",
    //        Subtitle = "Test",
    //        Description = "Working",
    //        BadgeNumber = 42,
    //        Schedule = new NotificationRequestSchedule
    //        {
    //            NotifyTime = DateTime.Now.AddSeconds(5),
    //            NotifyRepeatInterval = TimeSpan.FromDays(1),
    //        }
    //    };
    //    LocalNotificationCenter.Current.Show(request);
    //}

    //WeakReferenceMessenger.Default.Register<PushNotificationReceived>(this, (r, m) =>
    //        {
    //            string msg = m.Value;
    //});
}
