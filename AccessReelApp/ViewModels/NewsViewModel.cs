using AccessReelApp.Models;
using AccessReelApp.Prototypes;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AccessReelApp.ViewModels
{
    public partial class NewsViewModel : ObservableObject
    {
        // e
        [ObservableProperty] ObservableCollection<ImageButton> buttonCollection;
        [ObservableProperty] ObservableCollection<NewsItem> newsCollection;
        [ObservableProperty] ObservableCollection<TrailerItem> trailersCollection;
        [ObservableProperty] ObservableCollection<ReviewCell> newsInfo;

        public TmdbApiClient movieClient = new("aea36407a9c725c8f82390f7f30064a1");
        void Initialise()
        {
            ButtonCollection ??= new ObservableCollection<ImageButton>();
        }


        public NewsViewModel()
        {
            Initialise();


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
                    ImageSource = "turtles.jpg",
                    Title = "Sample News Title 1",
                    Description = "Sample news description...",
                    Author = "Authors",
                    Date = DateTime.Now.ToString("dd MMMM, yyyy")
                },
                // Add more news items as needed...
            };

            TrailersCollection = new ObservableCollection<TrailerItem>
            {
                new TrailerItem
                {
                    ImageSource = "barbie.png",
                    Title = "New Barbie Movie"
                },
                // Add more trailer items as needed...
            };
        }


    }

    public class NewsItem
    {
        public string ImageSource { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public string Date { get; set; }
    }

    public class TrailerItem
    {
        public string ImageSource { get; set; }
        public string Title { get; set; }
    }
}
