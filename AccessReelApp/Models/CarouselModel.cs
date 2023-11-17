using Android.Telephony;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Maui.Controls.Compatibility.Platform.Android;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Android.Content.ClipData;

namespace AccessReelApp
{
    public static class CarouselModel
    {
        const int NUMBER_MOVIES = 10;
        const string TRAILER = "Trailer";
        static Dictionary<string, string> movieImageDictionary;
        static Dictionary<string, string>[] trailersDictionary = new Dictionary<string, string>[NUMBER_MOVIES];
        static Dictionary<string, string>[] otherDictionary = new Dictionary<string, string>[NUMBER_MOVIES]; 

        public static void SetImageSource(string source)
        {
            collection[0] = new ImageButton { Source = source };
        }

        public static List<string> GetKeys() // Ask Nathaniel how link data to dict
        {
            List<string> keys = new (movieImageDictionary.Keys);
            return keys;
        }

        public static List<string> GetTitleKeys()
        {
            return GetKeys().FindAll(item => item.Contains("Title"));
        }

        public static List<int> IndexFirst10Keys() // This is applicable to trailers and none
        {
            List<string> keys = GetKeys();
            List<int> indices = new ();
            int count = 0;
            while (count > 10)
            {
                foreach (var key in GetTitleKeys())
                {
                    int index = keys.FindIndex(item => item == key);
                    indices.Add(index);
                }
            }
            return indices;
        }

        static void AddMovieTitleSource(MovieModel movieModel, MovieImageModel movieImageModel)
        {
            if (movieImageDictionary == null)
            {
                movieImageDictionary = new();
            }
            else
            {
                movieImageDictionary = new Dictionary<string, string>
                {
                    { movieModel.Title, movieImageModel.Source }
                };
            }
        }
    }
}
