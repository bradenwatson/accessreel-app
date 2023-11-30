using AccessReelApp.Models;
using AccessReelApp.Prototypes;
using AccessReelApp.Notifications;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Diagnostics;

namespace AccessReelApp.ViewModels
{
    public partial class NewsViewModel : ObservableObject
    {
        [ObservableProperty] ObservableCollection<ImageButton> buttonCollection;
        [ObservableProperty] ObservableCollection<NewsItem> newsCollection;
        [ObservableProperty] ObservableCollection<TrailerItem> trailersCollection;
        [ObservableProperty] ObservableCollection<ReviewCell> newsInfo;

        public TmdbApiClient movieClient = new("aea36407a9c725c8f82390f7f30064a1");
        void Initialise()
        {
            ButtonCollection ??= new ObservableCollection<ImageButton>();
        }

        async void SendStartNotification()
        {
            try
            {
                await NotificationManager.ReadFireBaseAdminSDK();
                NotificationManager notificationManager = new();
                notificationManager.CreateMessage("Welcome!", "This is an on start notification.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ERROR: {ex.Message}");
            }
        }

        public NewsViewModel()
        {
            Initialise();
            SendStartNotification();

            movieClient.ReviewFetched += (review) =>
            {
                Application.Current.Dispatcher.Dispatch(() =>
                {
                    NewsInfo.Add(review);
                });
            };

            NewsCollection = new ObservableCollection<NewsItem>
            {
                new NewsItem
                {
                    ImageSource = "https://accessreel.com/app/uploads/2023/11/324931425_2145663592287699_1477355275060355012_n-250x130.jpg",
                    NewsUrl ="https://accessreel.com/article/nominees-revealed-for-the-2023-wa-screen-culture-awards/",
                    Title = "Nominees Revealed for the 2023 WA Screen Culture Awards",
                    Description = "Celebrating the achievement, innovation and ambition of our local industry, " +
                    "the WA Screen Culture Awards (WASCAs) embrace all forms from new, establis...",
                    Author = "AccessReel",
                    Date = DateTime.Now.ToString("dd MMMM, yyyy")
                },
                new NewsItem
                {
                    ImageSource = "https://accessreel.com/app/uploads/2023/11/unnamed-1-250x130.jpg",
                    NewsUrl ="https://accessreel.com/article/daisy-ridley-set-to-star-in-zak-hilditchs-new-film-we-bury-the-dead-in-wa/",
                    Title = "Daisy Ridley set to star in Zak Hilditch’s new film We Bury the Dead in WA",
                    Description = "Screenwest and Screen Australia are pleased to announce Daisy Ridley (Star Wars sequel trilogy) " +
                    "is set to star in Zak Hilditch’s new survival-thriller...",
                    Author = "AccessReel",
                    Date = DateTime.Now.ToString("dd MMMM, yyyy")
                },
                new NewsItem
                {
                    ImageSource = "https://accessreel.com/app/uploads/2023/10/The-Healing-News_03-e1698526962618-250x130.jpg",
                    NewsUrl ="https://accessreel.com/article/healing-doc-about-to-drop/",
                    Title = "Healing Doc About To Drop",
                    Description = "THE HEALING is a documentary about to make its way around Australia.  " +
                    "It tells an inspiring tale of recovery from two very different worlds. Set again...",
                    Author = "AccessReel",
                    Date = DateTime.Now.ToString("dd MMMM, yyyy")
                },
                new NewsItem
                {
                    ImageSource = "https://accessreel.com/app/uploads/2023/10/Jai-Courtney-to-play-Bryan-Shearer-Celeste-Barber-to-play-Susie-Shearer-in-Perth-preparing-to-shoot-RUNT-_-photo-credit-Finlay-MacKay-3-250x130.jpg",
                    NewsUrl ="https://accessreel.com/article/craig-silveys-best-selling-novel-runt-kicks-off-production-in-wa-with-jai-courtney-leading-the-cast/",
                    Title = "Craig Silvey’s best-selling novel RUNT kicks off production in WA with Jai Courtney Leading the Cast",
                    Description = "Craig Silvey’s best-selling novel RUNT is being made into a feature film in Western Australia with an incredible " +
                    "all-star Australian cast including Ja...",
                    Author = "AccessReel",
                    Date = DateTime.Now.ToString("dd MMMM, yyyy")
                },
                // Add more news items as needed...
            };

            TrailersCollection = new ObservableCollection<TrailerItem>
            {
                new TrailerItem
                {
                    ImageSource = "https://accessreel.com/app/uploads/2023/09/EXT_INTL_ONE_SHEET_CHURCH_SAFE_AUS-Digital_HiRes-1-1-200x297.jpg",
                    TrailerUrl = "https://accessreel.com/article/the-exorcist-believer-trailer/",
                    Title = "The Exorcist: Believer Trailer"
                },
                new TrailerItem
                {
                    ImageSource = "https://accessreel.com/app/uploads/2023/10/NapoleonPoster2-1-200x297.jpg",
                    TrailerUrl = "https://accessreel.com/article/napoleon-trailer/",
                    Title = "Napolean Trailer"
                },
                new TrailerItem
                {
                    ImageSource = "https://accessreel.com/app/uploads/2023/09/FNF_crt.InFeed_crn.Payoff_siz.1080x1350_cta.ReleaseDate_cou.AU-en-1-200x297.jpg",
                    TrailerUrl = "https://accessreel.com/article/five-nights-at-freddys-trailer/",
                    Title = "Five Nights at Freddy’s Trailer"
                },
                // Add more trailer items as needed...
            };
        }

    }

    public class NewsItem
    {
        public string ImageSource { get; set; }
        public string NewsUrl {  get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public string Date { get; set; }
    }

    public class TrailerItem
    {
        public string ImageSource { get; set; }
        public string TrailerUrl { get; set; }
        public string Title { get; set; }
    }
}
