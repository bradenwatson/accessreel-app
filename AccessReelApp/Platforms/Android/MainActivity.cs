using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Microsoft.Maui.Controls.Compatibility.Platform.Android;
using System.Diagnostics;
using Debug = System.Diagnostics.Debug;

namespace AccessReelApp;

//Fix app from crashing when tapping notiification while app in foreground: https://github.com/dotnet/maui/issues/10776

[Activity(  Theme = "@style/Maui.SplashTheme", 
            MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density,
            LaunchMode = LaunchMode.SingleTop)]
public class MainActivity : MauiAppCompatActivity
{
    public enum Pages       //Turn into dictionary
    {
        MainPage,
        //Page1,
        //Page2,
        NewsPage,
        ReviewsPage,
        InterviewPage,
        SignUpLogin,
        Movies,
        Competitions,
        //Settings,
        Accounts,
    }

#pragma warning disable CA1416 // Validate platform compatibility
    internal static readonly IList<NotificationChannel> Channels = new List<NotificationChannel> //Can rename ID to page name, but keep name to channel name
    {
        new NotificationChannel(Pages.MainPage.ToString(), "General", NotificationImportance.Default),
        new NotificationChannel(Pages.NewsPage.ToString(), "News", NotificationImportance.Default),
        new NotificationChannel(Pages.ReviewsPage.ToString(), "Reviews", NotificationImportance.Default),
        new NotificationChannel(Pages.InterviewPage.ToString(), "Interviews", NotificationImportance.Default),
        new NotificationChannel(Pages.SignUpLogin.ToString(), "Sign Up/Login", NotificationImportance.Default),
        new NotificationChannel(Pages.Movies.ToString(), "Movies", NotificationImportance.Default),
        new NotificationChannel(Pages.Competitions.ToString(), "Competitions", NotificationImportance.Default),
        //new NotificationChannel(Pages.Settings.ToString(), "Settings", NotificationImportance.Default),
        new NotificationChannel(Pages.Accounts.ToString(), "Accounts", NotificationImportance.Default),
    };
#pragma warning restore CA1416 // Validate platform compatibility

    protected override void OnNewIntent(Intent intent)
    {
        base.OnNewIntent(intent);
        Debug.WriteLine("*******************************");
        Debug.WriteLine($"Intent = {intent.Data}");
        Debug.WriteLine("*******************************");
        SwitchByNotification();
    }

    private void SwitchByNotification()         //Redirects to page on notification interaction     
    {
        if(Preferences.ContainsKey("NavigationID"))
        {
            string id = Preferences.Get("NavigationID", Pages.MainPage.ToString());
            Debug.WriteLine("*******************************");
            Debug.WriteLine($"ID switch = {id}");
            Debug.WriteLine("*******************************");
            if (Enum.TryParse(id, out Pages page))
            {
                NavigateToPage(page);
            }
            else
            {
                Debug.WriteLine("*******************************");
                Debug.WriteLine("No matching ID");
                Debug.WriteLine("*******************************");

                NavigateToPage(Pages.MainPage);
            }
            Preferences.Remove("NavigationID");
        }
        

        /*
        //if (Preferences.ContainsKey("NavigationID"))        //Key missing, may need to getfrom inent
        //{
        //    string id = Preferences.Get("NavigationID", "");
        //    //Debug.WriteLine("*******************************");
        //    //Debug.WriteLine($"ID switch = {id}");
        //    //Debug.WriteLine("*******************************");
        //    if (Enum.TryParse(id, out Pages page))
        //    {
        //        NavigateToPage(page);
        //    }
        //    else
        //    {
        //        Debug.WriteLine("*******************************");
        //        Debug.WriteLine("No matching ID");
        //        Debug.WriteLine("*******************************");

        //        NavigateToPage(Pages.MainPage);
        //    }
        //    Preferences.Remove("NavigationID");
        //}
        //else
        //{
        //    Debug.WriteLine("*******************************");
        //    Debug.WriteLine("Key does not exist");
        //    Debug.WriteLine("*******************************");
        //}
        */
    }

    //WOKRING
    private void NavigateToPage(Pages page)
    {
        if (Enum.IsDefined(typeof(Pages), page))
        {
            AppShell.Current.GoToAsync(page.ToString());
            Debug.WriteLine("*******************************");
            Debug.WriteLine($"Navigate to page {page}");
            Debug.WriteLine("*******************************");
        }
        else
        {
            AppShell.Current.GoToAsync(Pages.MainPage.ToString());
            Debug.WriteLine("*******************************");
            Debug.WriteLine($"Page does not exist");
            Debug.WriteLine("*******************************");
        }

    }





    protected override void OnCreate(Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        if (Intent.Extras != null)
        {
            foreach (var key in Intent.Extras.KeySet())
            {
                if (key == "NavigationID")
                {
                    string idValue = Intent.Extras.GetString(key);
                    Debug.WriteLine("*******************************");
                    Debug.WriteLine($"ID on create = {idValue}");
                    Debug.WriteLine("*******************************");
                    if (Preferences.ContainsKey("NavigationID"))
                    {
                        Preferences.Remove("NavigationID");
                        Debug.WriteLine("*******************************");
                        Debug.WriteLine($"Removed NavigatinID preference");
                        Debug.WriteLine("*******************************");
                    }
                    Preferences.Set("NavigationID", idValue);
                    Debug.WriteLine("*****************************");
                    Debug.WriteLine($"ID create = {Preferences.Get("NavigationID", "")}");
                    Debug.WriteLine("*****************************");
                }
            }
        }
        else
        {
            var key = Intent.Extras;
            System.Diagnostics.Debug.WriteLine("**************************************************************************");
            System.Diagnostics.Debug.WriteLine($"Key: ({key})");
        }


        bool channelCreated = Preferences.Get("NotificationChannel", false);
        if (!channelCreated)
        {
            CreateNotificationChannel();
            Preferences.Default.Set("NotificationChannel", true);
        }
        else
        {
            List<NotificationChannel> sysChannels = GetAllNotificationChannels();
            foreach (NotificationChannel channel in sysChannels)
            {
                Debug.WriteLine("*****************************");
                Debug.WriteLine($"{channel}");
                Debug.WriteLine("*****************************");
            }
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
        else
        {
            throw new NotSupportedException();
        }
        
    }

    private void CreateNotificationChannel()        
    {
        if (OperatingSystem.IsOSPlatformVersionAtLeast("android", 26))
        {
            var notificationManager = (NotificationManager)GetSystemService(Android.Content.Context.NotificationService);
            notificationManager.CreateNotificationChannels(Channels);
        }
    }
}
