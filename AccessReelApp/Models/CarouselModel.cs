using AccessReelApp.Models;
using AccessReelApp.ViewModels;
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
        public static Dictionary<string, string> MovieImageDictionary { get; } = new Dictionary<string, string>();
        static Dictionary<string, string>[] trailersDictionary = new Dictionary<string, string>[NUMBER_MOVIES];
        static Dictionary<string, string>[] otherDictionary = new Dictionary<string, string>[NUMBER_MOVIES];
        static ImageButton[] trailerImageButtons = new ImageButton[NUMBER_MOVIES];
        static ImageButton[] otherImageButtons = new ImageButton[NUMBER_MOVIES];


        public static void SetImageSource(string source)
        {
            //collection[0] = source;
        }

        // Need to get position image button and set indicator view index to that

        // Initialises the dictionaries and image buttons
        static void Initialise()
        {
            List<string> keys = ListKeys();
            PopulateDictionaries(keys, TRAILER, trailersDictionary, trailerImageButtons);
            PopulateDictionaries(keys, string.Empty, otherDictionary, otherImageButtons);
        }

        // Gets a list of keys from the movieImageDictionary
        static List<string> ListKeys()
        {
            return new List<string>(MovieImageDictionary.Keys);
        }

        // Populates dictionaries and ImageButtons based on a filter (TRAILER or other)
        static void PopulateDictionaries(List<string> keys, string filter, Dictionary<string, string>[] dictionaries, ImageButton[] buttons)
        {
            // Selects keys based on the filter
            List<string> selectedKeys = (filter == TRAILER) ? FindKeys(TRAILER, keys) : keys.Except(FindKeys(TRAILER, keys)).Take(NUMBER_MOVIES).ToList();

            // Populates dictionaries and image buttons based on selected keys
            for (int i = 0; i < NUMBER_MOVIES; i++)
            {
                string key = selectedKeys.ElementAtOrDefault(i);
                string value = MovieImageDictionary[key];
                dictionaries[i] = CreateDictionary(key, value);
                buttons[i] = CreateImageButton(key);
            }
        }

        // Finds keys in the list containing the specified key and returns a sublist.
        static List<string> FindKeys(string key, List<string> keys)
        {
            return keys.FindAll(item => item.Contains(key)).Take(NUMBER_MOVIES).ToList();
        }

        // Creates an array of dictionaries for a filtered subset of keys.
        static Dictionary<string, string>[] CreateDictionaryArray(List<string> keys, string filter)
        {
            return FindKeys(filter, keys).Select(key => CreateDictionary(key, MovieImageDictionary[key])).ToArray();
        }

        // Creates an array of image buttons based on the list's elements.
        static ImageButton[] CreateImageButtonArray(List<string> keys)
        {
            return keys.Select(CreateImageButton).ToArray();
        }

        // Returns an array of dictionaries based on the parameter's value.
        static Dictionary<string, string>[] GetSelectedDict(string filter)
        {
            return filter == TRAILER ? trailersDictionary : otherDictionary;
        }

        // Returns an array of image buttons based on the parameter's value.
        static ImageButton[] GetSelectedImageButtons(string filter)
        {
            return filter == TRAILER ? trailerImageButtons : otherImageButtons;
        }

        public static void AddMovieToDictionaries(ReviewCell reviewCell, int index)
        {
            string key = $"Movie_{index}";

            // Associates a movie poster's URL with a key in the movieImageDictionary
            MovieImageDictionary[key] = reviewCell.PosterUrl;

            // Notify NewsViewModel about the changes using ViewModelLocator
            ViewModelLocator.NewsViewModelInstance?.UpdateMovieImageCollection(new KeyValuePair<string, string>(key, reviewCell.PosterUrl));
        }

        public static void AddToViewModelButtonCollection(ReviewCell reviewCell, NewsViewModel viewModel)
        {
            ImageButton imageButton = CreateImageButton(reviewCell.PosterUrl);

            // Update the buttonCollection in the ViewModel
            viewModel.UpdateButtonCollection(imageButton);
        }

        static Dictionary<string, string> CreateDictionary(string key, string value) => new() { { key, value } };

        static ImageButton CreateImageButton(string key) => new() { Source = MovieImageDictionary[key] };
    }
}