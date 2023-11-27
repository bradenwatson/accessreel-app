using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccessReelApp
{
    public static class CarouselModel
    {
        const int NUMBER_MOVIES = 10;
        const string TRAILER = "Trailer";
        static Dictionary<string, string> movieImageDictionary = new();
        static Dictionary<string, string>[] trailersDictionary = new Dictionary<string, string>[NUMBER_MOVIES];
        static Dictionary<string, string>[] otherDictionary = new Dictionary<string, string>[NUMBER_MOVIES];
        static ImageButton[] trailerImageButtons = new ImageButton[NUMBER_MOVIES];
        static ImageButton[] otherImageButtons = new ImageButton[NUMBER_MOVIES];


        public static void SetImageSource(string source)
        {
            collection[0] = source;
        }

        // Need to get position image button and set indicator view index to that

        // Initialises the dictionaries and image buttons
        static void Initialise()
        {
            List<string> keys = ListKeys();
            PopulateEntities(keys, TRAILER, trailersDictionary, trailerImageButtons);
            PopulateEntities(keys, string.Empty, otherDictionary, otherImageButtons);
        }

        // Gets a list of keys from the movieImageDictionary
        static List<string> ListKeys() => new (movieImageDictionary.Keys);


        // Populates dictionaries and ImageButtons based on a filter (TRAILER or other)
        static void PopulateEntities(List<string> keys, string filter, Dictionary<string, string>[] dictionaries, ImageButton[] buttons)
        {
            List<string> selectedKeys = (filter == TRAILER) ? FindKeys(TRAILER, keys) : keys.Except(FindKeys(TRAILER, keys)).Take(NUMBER_MOVIES).ToList();

            for (int i = 0; i < NUMBER_MOVIES; i++)
            {
                string key = selectedKeys.ElementAtOrDefault(i);
                PopulateEntity(key, dictionaries, buttons, i);
            }
        }

        // Finds keys in the list containing the specified key and returns a sublist.
        static List<string> FindKeys(string key, List<string> keys) => keys.FindAll(item => item.Contains(key)).Take(NUMBER_MOVIES).ToList();

        // Populates a dictionary and an ImageButton at the specified index
        static void PopulateEntity(string key, Dictionary<string, string>[] dictionaries, ImageButton[] buttons, int index)
        {
            string value = movieImageDictionary[key];
            dictionaries[index] = CreateDictionary(key, value);
            buttons[index] = CreateImageButton(key);
        }

        public static void AddMovieToDictionaries(ReviewCell reviewCell, int index)
        {
            string key = $"Movie_{index}";
            movieImageDictionary[key] = reviewCell.PosterUrl;

            Dictionary<string, string>[] selectedDictionary = GetSelectedDictAndButtons(reviewCell.MovieTitle);
            PopulateEntities(new List<string> { key }, selectedDictionary, GetSelectedImageButtons(reviewCell.MovieTitle));
        }

        static Dictionary<string, string>[] CreateDictionaryArray(List<string> keys, string filter) => FindKeys(filter, keys).Select(key => CreateDictionary(key, movieImageDictionary[key])).ToArray();

        static ImageButton[] CreateImageButtonArray(List<string> keys) => keys.Select(CreateImageButton).ToArray();

        static Dictionary<string, string>[] GetSelectedDictAndButtons(string filter) => filter == TRAILER ? trailersDictionary : otherDictionary;

        static ImageButton[] GetSelectedImageButtons(string filter) => filter == TRAILER ? trailerImageButtons : otherImageButtons;

        static Dictionary<string, string> CreateDictionary(string key, string value) => new() { { key, value } };

        static ImageButton CreateImageButton(string key) => new() { Source = movieImageDictionary[key] };
    }
}
