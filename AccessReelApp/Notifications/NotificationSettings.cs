using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * * Settings:
        * tableview:
            * "Enable notifications"
            * "Enable sound notifications"
            * "Enable reminders (for outside functionality)"
        * picker:
            *  "Frequency"
                * Daily
                * Weekly
                * Fortnightly
                * Monthly (at the beginning of the month)
            * "Remind me every"
                * x minutes before
                * in x days
                * x days before event ends
            * Genre Preference
            * Select movie language
            * Movie age ratings
        * radio button:
            * "Preferred topics"   
                * All
                * Deals
                * Compeitions
                * Interviews
                * Showing near you
                * News
                * Trending
 */

namespace AccessReelApp.Notifications
{
    public class GenrePref
    {
        public enum MovieGenre
        {
            Action,
            Comedy,
            Drama,
            Fantasy,
            Horror,
            Mystery,
            Romance,
            SciFi,
            Thriller,
            Other
        }

        private List<GenreSwitch> genres;

        //Create a list with with bool attached to each one
        private class GenreSwitch
        {
            MovieGenre MovieGenre { get; set; }
            bool selected;

            public GenreSwitch(MovieGenre genre, bool enabled)
            {
                MovieGenre = genre;
                selected = enabled;
            }
        }

        public GenrePref()
        {
            genres = new List<GenreSwitch>();
        }

        public void setDefault()
        { }

    }

    public class LanguagePref
    { 
        public enum MovieLanguage
        {
            All,
            English,
            Spanish,
            French,
            German,
            Chinese,
            Japanese,
            Korean,
            Other
        }
    }

    public class AgePref
    {
        public enum MovieAgeRating
        {
            All,
            G,
            PG,
            PG13,
            R,
            NC17,
            Other
        }
    }

    public class TopicPref
    {
        public enum PreferredTopic  //Optional
        {
            All,
            Deals,
            Competitions,
            Interviews,
            ShowingNearYou,
            News,   //Anything new
            Trending,   //Anything popular
        }
    }


    public class NotificationSettings
    {
        public enum Frequency
        {
            Hourly,
            Daily,
            Weekly,
            Fortnightly,
            Monthly
        }

        private string chosenFrequency;
        public string ChosenFrequency
        {
            get { return chosenFrequency; }
            set { chosenFrequency = value; }
        }

        public NotificationSettings()
        {
            chosenFrequency = Frequency.Daily.ToString();

        }
    }
}
