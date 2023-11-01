/**************************************************************
 *                      NOTIFICATIONS              *
 **************************************************************/

/*
  Author: Tony Bui, Thomas Chen
  Date: 1/11/23
  Class Name: Notifications
  Purpose: 
    Handles the delivery and reception of local and server
    based notifications for the following platforms: iOS,
    Android, Web.   

    This class uses Firebase Cloud Messaging which is a free, 
    scalable google based service for handling notifiications
    from targeted to a wider audience.
           
  Notes:
    *
*/

/**************************************************************
                        TO DO LIST
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
            * "Enable sound notifications"
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
            * Genre Preference
            * Select movie language
            * Movie age ratings
            * Movies rated above (3-5 starts)
        * radio button:
            * "Preferred topics"   
                * All
                * Deals
                * Compeitions
                * Interviews
                * Showing near you
                * News
                * Trending
 
 * SERVER:
    * Software update
    * Refresh movie list
    * Refresh compeitions
    * Refresh deals
**************************************************************/
using Plugin.LocalNotification;

namespace AccessReelApp
{
    internal class Notifications
    {

    }
}
