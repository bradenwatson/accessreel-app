using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    public static class NotificationSettings
    {
        private const string NotificationEnabledKey = "Notification Enabled";       //This is to check globally
        private const string FrequencyKey = "Frequency";
        private const string RemindMeInKey = "RemindMeIn";
        private const string InterestsKey = "Interest";

        

        public static bool EnableNotification
        {
            get => Preferences.Default.Get(NotificationEnabledKey, true);
            set => Preferences.Default.Set(NotificationEnabledKey, value);
        }

        public static Frequency SelectedFrequency
        {
            get => Preferences.Default.Get(FrequencyKey, Frequency.Immediate);
            set => Preferences.Default.Set(FrequencyKey, value);
        }

        public static RemindMeIn SelectedRemindMeIn     //Should this be a global setting or should user define for each movie? Should also be used for compeititons
        {
            get => Preferences.Default.Get(RemindMeInKey, RemindMeIn.One_Hour);
            set => Preferences.Default.Set(RemindMeInKey, value);
        }

        public static Dictionary<string, bool> Interests
        {
            get 
            {
                Dictionary<string, bool> tmp = null;
                var result = Preferences.Default.Get(InterestsKey, tmp);
                if (result != null) 
                {
                    result = new Dictionary<string, bool>
                    {
                        {"All", true},
                        {"News", true},
                        {"Reviews", true},
                        {"Interviews", true},
                        {"Movies", true},
                        {"Competitions", true},
                    };
                }
                
                if (result["All"])
                {
                    foreach(var entry in result)
                    {
                        if (!entry.Value)
                        {
                            result[entry.Key] = true;
                        }
                    }
                }
                return result;
            }

            set => Preferences.Default.Set(InterestsKey, value);
        }
    }


}
