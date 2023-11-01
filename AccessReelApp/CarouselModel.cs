using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccessReelApp
{
    public class CarouselModel
    {
        public Dictionary<MovieModel, MovieImageModel> movieImageDictionary;
        int number_movies = 10;
        public Dictionary<MovieModel, MovieImageModel>[] movieImageDictionaries;
    }
}
