using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccessReelApp
{
    public class CarouselModel
    {
        Dictionary<string, string>[] movieImageDictionaries;

        void AddMovieImage(MovieModel movieModel, MovieImageModel movieImageModel)
        {
            const int NUMBER_MOVIES = 10;
            int currentIndex = Array.FindIndex(movieImageDictionaries, dict => dict == null);
            if (currentIndex >= 0 && currentIndex < NUMBER_MOVIES)
            {
                movieImageDictionaries[currentIndex] = new Dictionary<string, string>
                {
                    { movieModel.Title, movieImageModel.Source }
                };
            }
        }

        public CarouselModel(MovieModel movieModel, MovieImageModel movieImageModel)
        {
            AddMovieImage(movieModel, movieImageModel);
        }
    }
}
