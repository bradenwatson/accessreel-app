/**************************************************************
 *                      LOCAL NOTIFICATIONS                   *
 **************************************************************/

/*
  Author: Tony Bui
  Last Updated: 13/11/23
  Class Name: Local Notifications
  Purpose: 
    Create and set customised local push notifications based 
    on the user's notification preferences for Android and 
    iOS devices. It also features scheduling, ID and 
    badgenumbers.

    This class itself is not an object, and should be called
    as trigger to alert the user or inform the user's interests.

    It should only be used for notifying user when they to be
    reminded for an upcoming event (and if possible update 
    when new content is delivered in the background)

  Notes:
    * Requires "Plugin.LocalNotification" package
*/

using Microsoft.Maui.Controls.PlatformConfiguration;
using Plugin.LocalNotification;
using System.Diagnostics;

namespace AccessReelApp.Notifications
{
    public class NotificationHandler
    {
        //When a notification request is created, it is stored within a Local Notification Center by a unique identifier which can be retireved.
        public static string GenerateNotificationID()
        {
            string time = DateTime.Now.ToLocalTime().ToString("MMddHHmmss");
            //return int.Parse(time);
            return time;
        }

        /*
        //May not need since using preferences
        public static async Task CheckNotificationPermission()
        {
            NotificationPermission notificationPermission = null;
            bool check = await LocalNotificationCenter.Current.AreNotificationsEnabled();
            if (!check)
            {
                //Create permissions if not made yet
                notificationPermission = new NotificationPermission();
                if(NotificationSettings.CheckNotificationEnabled())
                {
                    notificationPermission.AskPermission = true;
                }
                else
                {
                    notificationPermission.AskPermission = false;
                }
            }
            //else
            //{
            //    notificationPermission.AskPermission = NotificationSettings.CheckNotificationEnabled();
            //}

            await DebugPermissiions();

        }

        private static async Task DebugPermissiions()
        {
            bool check = await LocalNotificationCenter.Current.AreNotificationsEnabled();
            Debug.WriteLine($"Local Notifications enabled = {check}");
        }
        */

        //Create an object manager that controls the flow, retrieval, cancellation and deletion of messages
        public static async Task<NotificationRequest> CreateReminder(int notificationID, DateTime airing, string title, string body, string subtitle = "")      //date ending
        {
            var currBadges = await LocalNotificationCenter.Current.GetDeliveredNotificationList();
            //Apply global settings
            var request = new NotificationRequest
            {
                NotificationId = notificationID,      //Unique for each  movie so that if they need to change reminder for it is possible, attached to bell button id
                Title = title,
                Subtitle = subtitle,
                Description = body,
                BadgeNumber = currBadges.Count,
                Schedule = new NotificationRequestSchedule
                {
                    NotifyTime = airing,      //Make function -get movie date and time then set
                    //NotifyRepeatInterval = TimeSpan.FromDays(1),     //Useful for already updated movie list   //Timespan as method to based on setting
                }
            };

            if (request != null)
            {
                Debug.WriteLine($"Created Notification ({notificationID})");
            }

            return request;

            //await LocalNotificationCenter.Current.Show(request);
        }


        //Create a toggle for bell button WORKING!
        public static async void BellButton(object sender, EventArgs e)
        {
            //Generate ID on button press or on movie update
            if (sender is Button button)
            {
                if (string.IsNullOrEmpty(button.AutomationId))
                {
                    //button.AutomationId = GenerateNotificationID().ToString();
                    button.AutomationId = GenerateNotificationID();             //Create and set unique notification ID specific to the button ONCE!
                    Debug.WriteLine($"New Button ID: {button.AutomationId}");
                }

                if (int.TryParse(((Button)sender).AutomationId, out int buttonID))
                {
                    var pendingRequests = await LocalNotificationCenter.Current.GetPendingNotificationList();
                    var request = pendingRequests.FirstOrDefault(x => x.NotificationId == buttonID);
                    if (request != null)
                    {
                        var sending = await LocalNotificationCenter.Current.Show(request);
                        if (sending)
                        {
                            LocalNotificationCenter.Current.Cancel(buttonID);
                        }
                        else if (LocalNotificationCenter.Current.Cancel(buttonID)) // If cancelled re-enable message
                        {
                            SendNotification(sender, e);
                        }
                    }
                    else
                    {
                        SendNotification(sender, e);
                    }
                }
            }

        }


        public static async void SendNotification(object sender, EventArgs e)
        {
            //await CheckNotificationPermission();

            if (int.TryParse(((Button)sender).AutomationId, out int buttonID))
            {
                var pendingRequests = await LocalNotificationCenter.Current.GetPendingNotificationList();
                var request = pendingRequests.FirstOrDefault(x => x.NotificationId == buttonID);
                //Check if notification ID exists otherwise create one
                if (request == null)
                {
                    request = await CreateReminder(buttonID, DateTime.Now.ToLocalTime(), "Hello", "Working");
                }

                if (NotificationSettings.CheckNotificationEnabled())
                {
                    await LocalNotificationCenter.Current.Show(request);
                }
                else
                {
                    Debug.WriteLine("*******************************");
                    Debug.WriteLine($"User has disabled notifications");
                    Debug.WriteLine("*******************************");
                }

            }
            else
            {
                Debug.WriteLine("Failed to generate notification ID.");
            }
        }


        ////Delete notification when movie expires
        //public static void DeleteButton()
        //{
        //    if (LocalNotificationCenter.Current.Clear(0))
        //    {
        //        Debug.WriteLine($"Deleted Notification ()");
        //    }
        //    else
        //    {
        //        Debug.WriteLine($"Failed to delete Notification ()");
        //    }
        //}
    }
}
