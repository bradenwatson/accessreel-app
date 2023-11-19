using Android.App;
using Android.Content.PM;
using Android.Media;
using Android.OS;
using System.Threading.Channels;
using System.Diagnostics;

namespace AccessReelApp;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
    internal static readonly string ChannelID = "General_Channel";
    internal static readonly int NotificationID = 0;
    internal static readonly Dictionary<string, ChannelInfo> Channels = new Dictionary<string, ChannelInfo>
    {
        {"General_Channel", new ChannelInfo("General_Channel", "General")},                                             //Intended for universal content
        //{"Competitions_Channel", new ChannelInfo("Competitions_Channel", "Competitions")},                            //Intended for competitions
        //{"MovieUpdates_Channel", new ChannelInfo("MovieUpdates_Channel", "Movie Updates")},                           //Intended for movie upodates
        //{"News_Channel", new ChannelInfo("News_Channel", "News")},                                                    //Intended for social media news
        //{"Reivews_Channell", new ChannelInfo("Reviews_Channel", "Reviews")},                                          //Intended for discussion in the comments
        //{"Account_Channel", new ChannelInfo("Account_Channel", "Account")},                                           //Intended for security purposes
        //{"AdminCommunications_Channel", new ChannelInfo("AdminCommunications_Channel", "Admin Communications")}       //Intended for only urgent communication (eg update on tc's)
    };

    public class ChannelInfo
    {
        public string ChannelID { get; }
        public string DisplayName { get; }

        public ChannelInfo(string channelId, string displayName)
        {
            ChannelID = channelId;
            DisplayName = displayName;
        }
    }

    


    protected override void OnCreate(Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        bool channelCreated = Preferences.Get("NotificationChannel", false);
        if(!channelCreated)
        {
            CreateNotificationChannel();
            Preferences.Default.Set("NotificationChannel", true);
        }
        else
        {
            foreach (var (channelId, channel) in Channels)
            {
                System.Diagnostics.Debug.WriteLine("**************************************************************************");
                System.Diagnostics.Debug.WriteLine($"Created channel: {channel.DisplayName}");
            }
        }

        if (Intent.Extras != null)
        {
            foreach (var key in Intent.Extras.KeySet())
            {
                if (key == "NavigationID")
                {
                    string idValue = Intent.Extras.GetString(key);
                    
                    if (Preferences.ContainsKey("NavigationID"))
                    {
                        Preferences.Remove("NavigationID");
                    }
                    Preferences.Set("NavigationID", idValue);
                }
            }
        }
        else
        {
            var key = Intent.Extras;
            System.Diagnostics.Debug.WriteLine("**************************************************************************");
            System.Diagnostics.Debug.WriteLine($"Key: ({key})");
        }
      
        //CreateNotificationChannel();
    }

    private void CreateNotificationChannel()
    {
        if (OperatingSystem.IsOSPlatformVersionAtLeast("android", 26))
        {
            var notificationManager = (NotificationManager)GetSystemService(Android.Content.Context.NotificationService);

            //foreach (var (channelId, channel) in Channels)
            //{
            //    var newChannel = new NotificationChannel(channelId, channel.DisplayName, NotificationImportance.Default);
            //    notificationManager.CreateNotificationChannel(newChannel);
            //}

            var newChannel1 = new NotificationChannel(ChannelID, ChannelID, NotificationImportance.Default);
            notificationManager.CreateNotificationChannel(newChannel1);
        }
    }
}
