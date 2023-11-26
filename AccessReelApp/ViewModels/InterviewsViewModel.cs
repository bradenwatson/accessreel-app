using AccessReelApp.Models;
using AccessReelApp.Prototypes;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Maui.Controls.Compatibility.Platform.Android.AppCompat;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccessReelApp.ViewModels
{
    public partial class InterviewsViewModel : ObservableObject
    {
        [ObservableProperty] ObservableCollection<ImageButton> buttonCollection;
        [ObservableProperty] ObservableCollection<InterviewsItem> interviewsCollection;
        [ObservableProperty] ObservableCollection<InterviewsCell> newsInfo;
        //[ObservableProperty] ObservableCollection<TrailerItem> trailersCollection;

        public TmdbApiClient movieClient = new("aea36407a9c725c8f82390f7f30064a1");
        void Initialise()
        {
            ButtonCollection ??= new ObservableCollection<ImageButton>();

        }


        public InterviewsViewModel()
        {
            Initialise();


            movieClient.ReviewFetched += (review) =>
            {
                Application.Current.Dispatcher.Dispatch(() =>
                {
                    //NewsInfo.Add(review);
                });
            };


            InterviewsCollection = new ObservableCollection<InterviewsItem>
            {
                new InterviewsItem
                {
                    ImageSource = "https://accessreel.com/app/uploads/2023/06/350652723_3472137829671330_4900757303798181498_n-1-250x130.jpg",
                    Title = "Supanova Comic Con & Gaming Perth Interview – Kevin Eastman",
                    InterviewsUrl = new Uri("https://accessreel.com/article/supanova-comic-con-gaming-perth-interview-kevin-eastman/"),
                    Description = "Born in Portland, Maine in 1962, Kevin Eastman began drawing at a very young age, " +
                    "copying children’s books and reading comics. Inspired by master stor...",
                    Author = "Darran",
                    Date = DateTime.Now.ToString("dd MMMM, yyyy")
                },
                new InterviewsItem
                {
                    ImageSource = "https://accessreel.com/app/uploads/2023/06/350652723_3472137829671330_4900757303798181498_n-1-250x130.jpg",
                    Title = "Supanova Comic Con & Gaming Perth Interview – Ian McElhinney",
                    InterviewsUrl = new Uri("https://accessreel.com/article/supanova-comic-con-gaming-perth-interview-ian-mcelhinney/"),
                    Description = "Ian McElhinney is a Northern Irish actor and director best known for his roles " +
                    "as General Dodonna in Rogue One: A Star Wars Story, Val-El in Krypton, ...",
                    Author = "AccessReel",
                    Date = DateTime.Now.ToString("dd MMMM, yyyy")
                },
                new InterviewsItem
                {
                    ImageSource = "https://accessreel.com/app/uploads/2023/04/1473826370-1-250x130.jpg",
                    Title = "Alyssa Sutherland and Lily Sullivan – Evil Dead Rise",
                    InterviewsUrl = new Uri("https://accessreel.com/article/alyssa-sutherland-and-lily-sullivan-evil-dead-rise/"),
                    Description = "EVIL DEAD RISE is the fifth instalment of the Evil Dead franchise created by Sam Raimi. " +
                    "The new film is written and directed by Irish filmmaker Lee Cr...",
                    Author = "Darran",
                    Date = DateTime.Now.ToString("dd MMMM, yyyy")
                },
                // Add more interviews items as needed...

            };
        }
    }

    public class InterviewsItem
    {
        public string ImageSource { get; set; }
        public Uri InterviewsUrl { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public string Date { get; set; }
    }
}