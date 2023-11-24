﻿using System;
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
        static void InitialiseDictionariesAndImageButtons()
        {
            List<string> keys = ListDictionaryKeys();
            PopulateDictionaries(keys, TRAILER, trailersDictionary, trailerImageButtons);
            PopulateDictionaries(keys, string.Empty, otherDictionary, otherImageButtons);
        }

        static bool IsTrailerInTitle(string title)
        {
            return title.Contains(TRAILER);
        }

        // Gets a list of keys from the movieImageDictionary
        static List<string> ListDictionaryKeys()
        {
            return new List<string>(movieImageDictionary.Keys);
        }

        // Finds keys in the list containing the specified key and returns a sublist.
        static List<string> FindKeysContainingSubstring(string key, List<string> keys)
        {
            return keys.FindAll(item => item.Contains(key)).Take(NUMBER_MOVIES).ToList();
        }

        // Populates dictionaries and ImageButtons based on a filter (TRAILER or other)
        static void PopulateDictionaries(List<string> keys, string filter, Dictionary<string, string>[] dictionaries, ImageButton[] buttons)
        {
            // Selects keys based on the filter
            List<string> selectedKeys = (filter == TRAILER) ? FindKeysContainingSubstring(TRAILER, keys) : keys.Except(FindKeysContainingSubstring(TRAILER, keys)).Take(NUMBER_MOVIES).ToList();

            // Populates dictionaries and image buttons based on selected keys
            for (int i = 0; i < NUMBER_MOVIES; i++)
            {
                string key = selectedKeys.ElementAtOrDefault(i);
                string value = movieImageDictionary[key];
                dictionaries[i] = CreateDictionary(key, value);
                buttons[i] = CreateImageButton(key);
            }
        }

        // Creates an array of dictionaries for a filtered subset of keys.
        static Dictionary<string, string>[] CreateDictionaryArray(List<string> keys, string filter)
        {
            return FindKeysContainingSubstring(filter, keys).Select(key => CreateDictionary(key, movieImageDictionary[key])).ToArray();
        }

        // Creates an array of image buttons based on the list's elements.
        static ImageButton[] CreateImageButtonArray(List<string> keys)
        {
            return keys.Select(CreateImageButton).ToArray();
        }

        // Returns an array of dictionaries based on the parameter's value.
        static Dictionary<string, string>[] GetArrayDictionaries(string title)
        {
            return IsTrailerInTitle(title) ? trailersDictionary : otherDictionary;
        }

        // Returns an array of image buttons based on the parameter's value.
        static ImageButton[] GetSelectedImageButtons(string title)
        {
            return IsTrailerInTitle(title) ? trailerImageButtons : otherImageButtons;
        }

        public static void UpdateDictionariesAndArrayImageButtons(ReviewCell reviewCell, int index)
        {
            // Concatenates "Movie_" with the value of variable "index" 
            string key = $"Movie_{index}";

            // Associates a movie poster's URL with a key in the movieImageDictionary
            movieImageDictionary[key] = reviewCell.PosterUrl;

            // Gets the selected dictionaries and ImageButtons based on the reviewCell.MovieTitle
            Dictionary<string, string>[] selectedDictionary = GetArrayDictionaries(reviewCell.MovieTitle);
            selectedDictionary[index] = CreateDictionary(reviewCell.MovieTitle, reviewCell.PosterUrl);

            GetSelectedImageButtons(reviewCell.MovieTitle)[index] = CreateImageButton(reviewCell.PosterUrl);
        }

        static Dictionary<string, string> CreateDictionary(string key, string value) => new () { { key, value } };

        static ImageButton CreateImageButton(string key) => new () { Source = movieImageDictionary[key] };
    }
}
