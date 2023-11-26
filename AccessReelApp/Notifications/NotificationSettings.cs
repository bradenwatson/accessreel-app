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

    public enum RemindTimer
    {
        Fifteen,
        Thirty,
        One_Hour,
        Six_Hours,
        Twelve_Hours,
        One_Day,
        Two_Days,
        One_Week
    }

    public class PreferenceKeys
    {
        public const string NotificationEnabledKey = "Notification Enabled";       //This is to check globally
        public const string FrequencyKey = "Frequency";
        public const string RemindTimerKey = "RemindTimer";
        public const string InterestsKey = "Interest";
    }

    public partial class NotificationSettings : ObservableObject
    {
        bool notificationEnabled;
        Frequencies frequency;
        RemindTimer reminder;
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

        public Frequencies Frequency 
        {
            get
            {
                if (!Preferences.Default.ContainsKey(PreferenceKeys.FrequencyKey))
                {
                    Preferences.Default.Set(PreferenceKeys.FrequencyKey, Frequencies.Immediate);
                    Debug.WriteLine("*************************************");
                    Debug.WriteLine($"Entry ({PreferenceKeys.FrequencyKey} = {Preferences.Default.ContainsKey(PreferenceKeys.FrequencyKey)})");
                    Debug.WriteLine("*************************************");
                }

                frequency = Preferences.Default.Get(PreferenceKeys.NotificationEnabledKey, Frequencies.Immediate);
                return frequency;
            }
            set
            {
                frequency = value;
                Preferences.Default.Set(PreferenceKeys.FrequencyKey, frequency);
                SetProperty(ref frequency, frequency);
            }
        }

        public RemindTimer Reminder
        {
            get
            {
                if (!Preferences.Default.ContainsKey(PreferenceKeys.RemindTimerKey))
                {
                    Preferences.Default.Set(PreferenceKeys.RemindTimerKey, RemindTimer.One_Hour);
                    Debug.WriteLine("*************************************");
                    Debug.WriteLine($"Entry ({PreferenceKeys.RemindTimerKey} = {Preferences.Default.ContainsKey(PreferenceKeys.RemindTimerKey)})");
                    Debug.WriteLine("*************************************");
                }

                reminder = Preferences.Default.Get(PreferenceKeys.RemindTimerKey, RemindTimer.One_Hour);
                return reminder;
            }
            set
            {
                reminder = value;
                Preferences.Default.Set(PreferenceKeys.RemindTimerKey, reminder);
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

