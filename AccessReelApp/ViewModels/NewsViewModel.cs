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
        public ObservableCollection<KeyValuePair<string, string>> MovieImageCollection { get; } = new ObservableCollection<KeyValuePair<string, string>>();
        [ObservableProperty] ObservableCollection<ImageButton> buttonCollection;
        [ObservableProperty] ObservableCollection<NewsItem> newsCollection;
        [ObservableProperty] ObservableCollection<TrailerItem> trailersCollection;
        void Initialise()
        {
            ButtonCollection ??= new ObservableCollection<ImageButton>();
        }

        public void UpdateButtonCollection(ImageButton imageButton)
        {
            ButtonCollection.Add(imageButton);
        }

        public void UpdateMovieImageCollection(KeyValuePair<string, string> item)
        {
            MovieImageCollection.Add(item);
        }

        public NewsViewModel()
        {
            Initialise();

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
