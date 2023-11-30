using AccessReelApp.Models;
using AccessReelApp.Prototypes;
using AccessReelApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AccessReelApp
{
    public static class CarouselModel
    {
        const int NUMBER_MOVIES = 10;
        private static Dictionary<string, string> movieImageDictionary = new();
        private static Dictionary<string, string>[] movieDictionaries;
        private static ImageButton[] movieImageButtons;

        public static void SetImageSource(string key, string source)
        {
            movieImageDictionary[key] = source;
        }

        static void ReviewFetchedHandler(ReviewCell reviewCell)
        {
            NewsViewModel.Instance.ReviewCells.Add(reviewCell);
        }

        static async void Initialize()
        {
            var tmdbClient = new TmdbApiClient("");
            tmdbClient.ReviewFetched += ReviewFetchedHandler;
            await tmdbClient.GetReviewsForPopularMovies(NUMBER_MOVIES);
            InitializeArrays(out movieDictionaries, out movieImageButtons);
            PopulateMovies("", AddMovieInfo);
        }

        static void InitializeArrays(out Dictionary<string, string>[] dictionaries, out ImageButton[] buttons)
        {
            dictionaries = new Dictionary<string, string>[NUMBER_MOVIES];
            buttons = new ImageButton[NUMBER_MOVIES];
        }

        static void AddMovieInfo(string key, int index)
        {
            movieDictionaries[index] = CreateMovieEntity(key);
            var reviewCell = CreateReviewCell(key);
            movieImageButtons[index] = new ImageButton { Source = movieImageDictionary[key] };
            NewsViewModel.Instance.ReviewFetchedHandler(key, movieImageDictionary[key], reviewCell);
        }

        static void PopulateMovies(string filter, Action<string, int> handler)
        {
            var keys = movieImageDictionary.Keys.ToList();
            var selectedKeys = FilterKeys(filter, keys).Take(NUMBER_MOVIES).ToList();

            IterateSelectedKeys(selectedKeys, handler);
        }

        static void IterateSelectedKeys(List<string> selectedKeys, Action<string, int> handler)
        {
            for (int i = 0; i < NUMBER_MOVIES; i++)
            {
                string key = selectedKeys.ElementAtOrDefault(i);
                handler(key, i);
            }
        }

        static Dictionary<string, string> CreateMovieEntity(string key)
        {
            return new Dictionary<string, string> { { key, movieImageDictionary[key] } };
        }

        static ReviewCell CreateReviewCell(string key)
        {
            return new ReviewCell { MovieTitle = key, PosterUrl = movieImageDictionary[key] };
        }

        static List<string> FilterKeys(string filter, List<string> keys)
        {
            return string.IsNullOrEmpty(filter) ? keys : keys.Where(key => key.Contains(filter)).ToList();
        }
    }
}