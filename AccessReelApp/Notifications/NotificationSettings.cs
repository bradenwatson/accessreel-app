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
    public enum Frequency
    {
        Immediate,
        Hourly,
        Daily,
        Weekly,
        Fortnightly,
        Monthly
    }

    public enum RemindMeIn
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
        public const string RemindMeInKey = "RemindMeIn";
        public const string InterestsKey = "Interest";
    }

    public partial class NotificationSettings : ObservableObject
    {
        bool notificationEnabled;

        public bool NotificationEnabled
        {
            get
            {
                if(!Preferences.Default.ContainsKey(PreferenceKeys.NotificationEnabledKey))
                {
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

    }

    //public partial class NotificationSettings : INotifyPropertyChanged
    //{
    //    public event PropertyChangedEventHandler PropertyChanged;

    //    protected virtual void OnPropertyChanged(string propertyName)
    //    {
    //        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    //    }


    //    bool enabledNotifications;
    //    public bool EnableNotifications
    //    {
    //        get
    //        {
    //            if(!Preferences.ContainsKey(PreferenceKeys.NotificationEnabledKey))
    //            {
    //                Preferences.Set(PreferenceKeys.NotificationEnabledKey, true);
    //            }

    //            enabledNotifications = Preferences.Default.Get(PreferenceKeys.NotificationEnabledKey, true);
    //            //Preferences.Get(PreferenceKeys.NotificationEnabledKey, true);
    //            Debug.WriteLine("*************************************");
    //            Debug.WriteLine($"Preferences = {enabledNotifications}");
    //            //Debug.WriteLine($"Preferences = {Preferences.Get(PreferenceKeys.NotificationEnabledKey, true)}");
    //            Debug.WriteLine("*************************************");
    //            return enabledNotifications;
    //            //return Preferences.Get(PreferenceKeys.NotificationEnabledKey, true);
    //        }
    //        set
    //        {
    //            enabledNotifications = value;
    //            Preferences.Default.Set(PreferenceKeys.NotificationEnabledKey, enabledNotifications);
    //            //Preferences.Set(PreferenceKeys.NotificationEnabledKey, value);
    //            Debug.WriteLine("*************************************");
    //            Debug.WriteLine($"New Value = {value}");
    //            Debug.WriteLine("*************************************");
    //            OnPropertyChanged(nameof(EnableNotifications));
    //            //OnPropertyChanged();
    //            //SetProperty(ref enabledNotifications, enabledNotifications);
    //        }
    //    }
    //}
}

