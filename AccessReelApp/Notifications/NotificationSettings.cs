/**************************************************************
 *                      NOTIFICATION SETTINGS                   *
 **************************************************************/

/*
  Author: Tony Bui
  Last Updated: 27/11/23
  Class Name: Notifications Settings
  Purpose: 
    Create and store notification preference settings to the device

  Notes:
    * Setting frequency does not do anything and is incomplete.
*/

using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using Microsoft.Maui.Storage;
using System.ComponentModel;


namespace AccessReelApp.Notifications
{
    public enum Frequencies
    {
        Immediate,
        Hourly,
        Daily,
        Weekly,
        Fortnightly,
        Monthly
    }

    public class PreferenceKeys
    {
        public const string NotificationEnabledKey = "Notification Enabled";       //This is to check globally
        public const string FrequencyKey = "Frequency";
        public const string ReminderOptionKey = "ReminderOption";
        public const string InterestsKey = "Interest";
    }

    public partial class NotificationSettings : ObservableObject
    {
        bool notificationEnabled;
        string frequency;
        string reminder;
    
        public static readonly string[] ReminderOptions = new string[]
        { 
            "5 mins",
            "10 mins",
            "15 mins",
            "30 mins",
            "1 hr",
            "6 hrs",
            "12 hrs",
            "1 day",
            "2 days",
            "1 week",
        };


        public bool NotificationEnabled
        {
            get
            {
                if (!Preferences.Default.ContainsKey(PreferenceKeys.NotificationEnabledKey))
                {
                    Preferences.Default.Set(PreferenceKeys.NotificationEnabledKey, true);
                    Debug.WriteLine("*************************************");
                    Debug.WriteLine($"Entry ({PreferenceKeys.NotificationEnabledKey} = {Preferences.Default.ContainsKey(PreferenceKeys.NotificationEnabledKey)})");
                    Debug.WriteLine("*************************************");
                }

                notificationEnabled = Preferences.Default.Get(PreferenceKeys.NotificationEnabledKey, true);
                return notificationEnabled;
            }
            set
            {
                notificationEnabled = value;
                Preferences.Default.Set(PreferenceKeys.NotificationEnabledKey, notificationEnabled);
                SetProperty(ref notificationEnabled, notificationEnabled);
            }
        }

        public string Frequency //For general news only
        {
            get
            {
                if (!Preferences.Default.ContainsKey(PreferenceKeys.FrequencyKey))
                {
                    Preferences.Default.Set(PreferenceKeys.FrequencyKey, Frequencies.Immediate.ToString()); //FIX TO STRING NOT SUPPORTED TYPE
                    Debug.WriteLine("*************************************");
                    Debug.WriteLine($"Entry ({PreferenceKeys.FrequencyKey} = {Preferences.Default.ContainsKey(PreferenceKeys.FrequencyKey)})");
                    Debug.WriteLine("*************************************");
                }

                frequency = Preferences.Default.Get(PreferenceKeys.FrequencyKey, Frequencies.Immediate.ToString());
                if(Enum.IsDefined(typeof(Frequencies), frequency)) 
                {
                    return frequency;
                }

                //Otherwise
                frequency = Frequencies.Immediate.ToString();
                return frequency;
            }

            set
            {
                if (Enum.IsDefined(typeof(Frequencies), value))
                {
                    frequency = value;
                    Preferences.Default.Set(PreferenceKeys.FrequencyKey, frequency);
                }
                else
                {
                    //Otherwise
                    frequency = Frequencies.Immediate.ToString();
                }
                SetProperty(ref frequency, frequency);
            }
        }

        public string Reminder //For Bell reminders only
        {
            get
            {
                if (!Preferences.Default.ContainsKey(PreferenceKeys.ReminderOptionKey))
                {
                    Preferences.Default.Set(PreferenceKeys.ReminderOptionKey, ReminderOptions[2]);
                    Debug.WriteLine("*************************************");
                    Debug.WriteLine($"Entry ({PreferenceKeys.ReminderOptionKey} = {Preferences.Default.ContainsKey(PreferenceKeys.ReminderOptionKey)})");
                    Debug.WriteLine("*************************************");
                }

                reminder = Preferences.Default.Get(PreferenceKeys.ReminderOptionKey, ReminderOptions[2]);
                if(ReminderOptions.Contains(reminder))
                {
                    return reminder;
                }

                //Otherwise
                reminder = ReminderOptions[2];
                return reminder;
            }

            set
            {
                if(ReminderOptions.Contains(value))
                {
                    reminder = value;
                    Preferences.Default.Set(PreferenceKeys.ReminderOptionKey, reminder);
                }
                else
                {
                    //Otherwise
                    reminder = ReminderOptions[2];
                }
                SetProperty(ref reminder, reminder);
            }
        }

        public static bool CheckNotificationEnabled()
        {
            bool enabled = false;
            if (!Preferences.Default.ContainsKey(PreferenceKeys.NotificationEnabledKey))
            {
                Preferences.Default.Set(PreferenceKeys.NotificationEnabledKey, true);
            }
            enabled = Preferences.Default.Get(PreferenceKeys.NotificationEnabledKey, true);
            return enabled;
        }


        public static TimeSpan TimeToDeduct()
        {
            TimeSpan timeToDeduct = TimeSpan.Zero;
            if (!Preferences.Default.ContainsKey(PreferenceKeys.ReminderOptionKey))
            {
                Preferences.Default.Set(PreferenceKeys.ReminderOptionKey, ReminderOptions[2]);
            }
            string value = Preferences.Default.Get(PreferenceKeys.ReminderOptionKey, ReminderOptions[2]);
            switch(value)
            {
                case "5 mins":
                    timeToDeduct = timeToDeduct.Add(new TimeSpan(0,5,0));
                    break;
                case "10 mins":
                    timeToDeduct = timeToDeduct.Add(new TimeSpan(0, 10, 0));
                    break;
                case "15 mins":
                    timeToDeduct = timeToDeduct.Add(new TimeSpan(0, 15, 0));
                    break;
                case "30 mins":
                    timeToDeduct = timeToDeduct.Add(new TimeSpan(0, 30, 0));
                    break;
                case "1 hr":
                    timeToDeduct = timeToDeduct.Add(new TimeSpan(1, 0, 0));
                    break;
                case "6 hrs":
                    timeToDeduct = timeToDeduct.Add(new TimeSpan(6, 0, 0));
                    break;
                case "12 hrs":
                    timeToDeduct = timeToDeduct.Add(new TimeSpan(12, 0, 0));
                    break;
                case "1 day":
                    timeToDeduct = timeToDeduct.Add(new TimeSpan(24, 0, 0));
                    break;
                case "2 days":
                    timeToDeduct = timeToDeduct.Add(new TimeSpan(2*24, 0, 0));
                    break;
                case "1 week":
                    timeToDeduct = timeToDeduct.Add(new TimeSpan(7*24, 0, 0));
                    break;
            }
            return timeToDeduct;
        }

        public static long SetTimeToDisplay()
        {
            if (!Preferences.Default.ContainsKey(PreferenceKeys.FrequencyKey))
            {
                Preferences.Default.Set(PreferenceKeys.FrequencyKey, Frequencies.Immediate.ToString());
            }
            string value = Preferences.Default.Get(PreferenceKeys.FrequencyKey, Frequencies.Immediate.ToString());

            DateTime clock = DateTime.Now;
            long scheduledTime = 0;
            switch(Enum.Parse(typeof(Frequencies), value))
            {
                case Frequencies.Immediate:
                    scheduledTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                    break;
                case Frequencies.Hourly:
                    //DateTime roundedDate = 
                    break;
                case Frequencies.Daily:
                    break;
                case Frequencies.Weekly:
                    break;
                case Frequencies.Fortnightly:
                    break;
                case Frequencies.Monthly:
                    break;
            }
            return 0;
        }

        private static DateTime roundtoHr(DateTime time)
        {
            return new DateTime(time.Year, time.Month, time.Day, time.Hour, 0, 0);
        }


    }
}

