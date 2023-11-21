using Android.App;
using Android.Content.PM;
using Android.OS;
using System.Diagnostics;
using Debug = System.Diagnostics.Debug;

namespace AccessReelApp;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
    internal static readonly string Channel_ID = "NewChannel";
    internal static readonly int NotificationID = 0;


#pragma warning disable CA1416 // Validate platform compatibility
    internal static readonly IList<NotificationChannel> Channels = new List<NotificationChannel> 
    {
        new NotificationChannel("General", "General", NotificationImportance.Default),
        new NotificationChannel("Movies", "Movies", NotificationImportance.Default),
        new NotificationChannel("News", "News", NotificationImportance.Default),
        new NotificationChannel("Interviews", "Interviews", NotificationImportance.Default),
        new NotificationChannel("Accounts", "Accounts", NotificationImportance.Default),
        new NotificationChannel("Competitions", "Competitions", NotificationImportance.Default),
        new NotificationChannel("Communcations", "Commincations", NotificationImportance.High),
    };
#pragma warning restore CA1416 // Validate platform compatibility
    protected override void OnCreate(Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        bool channelCreated = Preferences.Get("NotificationChannel", false);
        if (!channelCreated)
        {
            CreateNotificationChannel();
            Preferences.Default.Set("NotificationChannel", true);
        }
        else
        {
            List<NotificationChannel> sysChannels = GetAllNotificationChannels();
            foreach( NotificationChannel channel in sysChannels)
            {
                Debug.WriteLine("*****************************");
                Debug.WriteLine($"{channel}");
                Debug.WriteLine("*****************************");
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

    private List<NotificationChannel> GetAllNotificationChannels()
    {
        if (OperatingSystem.IsOSPlatformVersionAtLeast("android", 26))
        {
            var notificationManager = (NotificationManager)GetSystemService(Android.Content.Context.NotificationService);
            return notificationManager.NotificationChannels.ToList();
        }

        return new List<NotificationChannel>();
    }


    private void CreateNotificationChannel()        //Need to create first on app start, not on button press. This is due to android 8
    {
        if (OperatingSystem.IsOSPlatformVersionAtLeast("android", 26))
        {
            var notificationManager = (NotificationManager)GetSystemService(Android.Content.Context.NotificationService);
            notificationManager.CreateNotificationChannels(Channels);
            
        }
    }
}
