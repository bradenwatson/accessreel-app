﻿using Android.App;
using Android.Content.PM;
using Android.OS;

namespace AccessReelApp;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
    internal static readonly string Channel_ID = "NewChannel";
    internal static readonly int NotificationID = 0; 
    protected override void OnCreate(Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        if(Intent.Extras != null)
        {
            foreach(var key in Intent.Extras.KeySet())
            {
                if(key == "NavigationID")
                {
                    string idValue = Intent.Extras.GetString(key);
                    if(Preferences.ContainsKey("NavigationID"))
                    {
                        Preferences.Remove("NavigationID");
                    }
                    Preferences.Set("NavigationID", idValue);
                }
            }
        }

        CreateNotificationChannel();
    }

    private void CreateNotificationChannel()        //Need to create first on app start, not on button press. This is due to android 8
    {
        if (OperatingSystem.IsOSPlatformVersionAtLeast("android", 26))
        {
            var channel = new NotificationChannel(Channel_ID, "New Notification Channel", NotificationImportance.Default);
            var notificationManager = (NotificationManager)GetSystemService(Android.Content.Context.NotificationService);
            notificationManager.CreateNotificationChannel(channel); 
        }
    }
}
