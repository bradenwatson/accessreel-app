using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccessReelApp
{
    public static class CarouselModel
    {
        static Dictionary<string, string>[] movieImageDictionaries;
        static ImageButton[] ButtonCollection = new ImageButton[1];

        public static void SetImageSource(string source)
        {
            ButtonCollection[0] = new ImageButton { Source = source };
        }

        static void AddMovieTitleSource(MovieModel movieModel, MovieImageModel movieImageModel)
        {
            const int NUMBER_MOVIES = 10;

            if (movieImageDictionaries == null)
            {
                movieImageDictionaries = new Dictionary<string, string>[NUMBER_MOVIES];
            }
            int currentIndex = Array.FindIndex(movieImageDictionaries, dict => dict == null);
            if (currentIndex >= 0 && currentIndex < NUMBER_MOVIES)
            {
                movieImageDictionaries[currentIndex] = new Dictionary<string, string>
                {
                    { movieModel.Title, movieImageModel.Source }
                };
            }
        }
    }
}
