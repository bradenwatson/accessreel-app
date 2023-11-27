using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using Microsoft.Maui.Storage;
using System.ComponentModel;

/*
 * LOCAL:
    * A bell button for users to opt: 
        * for 'upcoming' movies
        * for live interviews
        * expiring movies, competitions in their local time
    * A tab view of all previous notifications
    * An indicator for all tab views, pages that are new
        * Mark all settings 
    * Settings:
        * tableview:
            * "Enable notifications"
            * "Enable reminders (for outside functionality)"
        * picker:
            *  "Frequency"
                * Daily
                * Weekly
                * Fortnightly
            * "Remind me every"
                * x minutes
                * in x days
                * x days before event ends
 */

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

    //public enum RemindTimer //Not useful
    //{
    //Fifteen,
    //    Thirty,
    //    One_Hour,
    //    Six_Hours,
    //    Twelve_Hours,
    //    One_Day,
    //    Two_Days,
    //    One_Week
    //}



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
    }
}

