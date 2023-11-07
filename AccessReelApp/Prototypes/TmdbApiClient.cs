﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace AccessReelApp.Prototypes
{
    public class TmdbApiClient
    {
        private readonly string apiKey;
        private readonly RestClient restClient;

        public TmdbApiClient(string apiKey)
        {
            this.apiKey = apiKey;
            this.restClient = new RestClient("https://api.themoviedb.org/3/");
        }

        public string GetApiKey()
        {
            return apiKey;
        }

        public async Task<bool> IsApiKeyValid()
        {
            var request = new RestRequest("movie/popular");
            request.AddParameter("api_key", apiKey);
            request.AddParameter("language", "en-US");
            request.AddParameter("page", 1);

            var response = await restClient.ExecuteGetAsync(request);

            return response.IsSuccessful;
        }

        public async Task<List<Movie>> GetPopularMovies()
        {
            var request = new RestRequest("movie/popular");
            request.AddParameter("api_key", apiKey);
            request.AddParameter("language", "en-US");
            request.AddParameter("page", 1);

            var response = await restClient.ExecuteGetAsync(request);

            if (response.IsSuccessful)
            {
                string json = response.Content;
                var popularMovies = JsonConvert.DeserializeObject<PopularMoviesResponse>(json);
                return popularMovies?.Results;
            }
            else
            {
                return null;
            }
        }

        public async Task<Movie> GetMovieDetailsByName(string movieName)
        {
            var searchRequest = new RestRequest("search/movie");
            searchRequest.AddParameter("api_key", apiKey);
            searchRequest.AddParameter("query", movieName);
            searchRequest.AddParameter("language", "en-US");
            searchRequest.AddParameter("page", 1);

            var searchResponse = await restClient.ExecuteGetAsync(searchRequest);

            if (searchResponse.IsSuccessful)
            {
                string searchJson = searchResponse.Content;
                var searchResults = JsonConvert.DeserializeObject<SearchResults>(searchJson);

                if (searchResults?.Results != null && searchResults.Results.Count > 0)
                {
                    int movieId = searchResults.Results[0].Id;
                    var detailsRequest = new RestRequest($"movie/{movieId}");
                    detailsRequest.AddParameter("api_key", apiKey);
                    detailsRequest.AddParameter("language", "en-US");

                    var detailsResponse = await restClient.ExecuteGetAsync(detailsRequest);

                    if (detailsResponse.IsSuccessful)
                    {
                        string detailsJson = detailsResponse.Content;
                        var movieDetails = JsonConvert.DeserializeObject<Movie>(detailsJson);
                        return movieDetails;
                    }
                }
            }

            return null; // No movie found or an error occurred
        }

        public async Task GetMovieReviewsByName(string movieName)
        {
            // Step 1: Search for the movie by name
            var searchClient = new RestClient("https://api.themoviedb.org/3/search/movie");
            var searchRequest = new RestRequest("");
            searchRequest.AddHeader("accept", "application/json");
            searchRequest.AddHeader("Authorization", "Bearer eyJhbGciOiJIUzI1NiJ9.eyJhdWQiOiJhZWEzNjQwN2E5YzcyNWM4ZjgyMzkwZjdmMzAwNjRhMSIsInN1YiI6IjY1MDFlMGMwNmEyMjI3MDEzNzJkZTI2MCIsInNjb3BlcyI6WyJhcGlfcmVhZCJdLCJ2ZXJzaW9uIjoxfQ.BWcS2MB1VOSfsN5-cQgmAV288mJSzZpxxj20jlfc5SE"); // Add the authorization header
            searchRequest.AddParameter("query", movieName);
            searchRequest.AddParameter("language", "en-US");
            searchRequest.AddParameter("page", 1);

            var searchResponse = await searchClient.GetAsync(searchRequest);

            if (searchResponse.IsSuccessful)
            {
                JObject searchJson = JObject.Parse(searchResponse.Content);

                if (searchJson.ContainsKey("results") && searchJson["results"].HasValues)
                {
                    // Step 2: Extract the movie ID
                    int movieId = searchJson["results"][0].Value<int>("id");

                    // Step 3: Retrieve movie reviews using the ID
                    var reviewsClient = new RestClient($"https://api.themoviedb.org/3/movie/{movieId}/reviews");
                    var reviewsRequest = new RestRequest("");
                    reviewsRequest.AddHeader("accept", "application/json");
                    reviewsRequest.AddHeader("Authorization", "Bearer eyJhbGciOiJIUzI1NiJ9.eyJhdWQiOiJhZWEzNjQwN2E5YzcyNWM4ZjgyMzkwZjdmMzAwNjRhMSIsInN1YiI6IjY1MDFlMGMwNmEyMjI3MDEzNzJkZTI2MCIsInNjb3BlcyI6WyJhcGlfcmVhZCJdLCJ2ZXJzaW9uIjoxfQ.BWcS2MB1VOSfsN5-cQgmAV288mJSzZpxxj20jlfc5SE"); // Add the authorization header
                    reviewsRequest.AddParameter("language", "en-US");
                    reviewsRequest.AddParameter("page", 1);

                    var reviewsResponse = await reviewsClient.GetAsync(reviewsRequest);

                    if (reviewsResponse.IsSuccessful)
                    {
                        JObject reviewsJson = JObject.Parse(reviewsResponse.Content);

                        // Step 4: Extract ratings and content from reviews
                        if (reviewsJson.ContainsKey("results") && reviewsJson["results"].HasValues)
                        {
                            var results = reviewsJson["results"];
                            foreach (var result in results)
                            {
                                var author = result.Value<string>("author");
                                var rating = result["author_details"]?["rating"];
                                var content = result.Value<string>("content");

                                if (rating != null)
                                {
                                    Debug.WriteLine($"-------------------------------------------------------");
                                    Debug.WriteLine($"Author: {author}, Rating: {rating}, Content: {content}");
                                    Debug.WriteLine($"-------------------------------------------------------");
                                }
                                else
                                {
                                    Debug.WriteLine($"Author: {author}, Content: {content}");
                                }
                            }
                        }
                    }
                }
            }
        }

        public event Action<ReviewCell> ReviewFetched;
        public async Task GetReviewsForPopularMovies(int maxReviewsPerMovie)
        {
            var popularMoviesClient = new RestClient("https://api.themoviedb.org/3/movie/popular");
            var popularMoviesRequest = new RestRequest("");
            popularMoviesRequest.AddHeader("accept", "application/json");
            popularMoviesRequest.AddHeader("Authorization", "Bearer eyJhbGciOiJIUzI1NiJ9.eyJhdWQiOiJhZWEzNjQwN2E5YzcyNWM4ZjgyMzkwZjdmMzAwNjRhMSIsInN1YiI6IjY1MDFlMGMwNmEyMjI3MDEzNzJkZTI2MCIsInNjb3BlcyI6WyJhcGlfcmVhZCJdLCJ2ZXJzaW9uIjoxfQ.BWcS2MB1VOSfsN5-cQgmAV288mJSzZpxxj20jlfc5SE"); // Replace with your API key
            popularMoviesRequest.AddParameter("language", "en-US");
            popularMoviesRequest.AddParameter("page", 1); // Start with page 1

            var popularMoviesResponse = await popularMoviesClient.GetAsync(popularMoviesRequest);

            if (popularMoviesResponse.IsSuccessful)
            {
                JObject popularMoviesJson = JObject.Parse(popularMoviesResponse.Content);

                if (popularMoviesJson.ContainsKey("results") && popularMoviesJson["results"].HasValues)
                {
                    var results = popularMoviesJson["results"];
                    foreach (var result in results)
                    {
                        int movieId = result.Value<int>("id");
                        string movieTitle = result.Value<string>("title");
                        string posterPath = result.Value<string>("poster_path");

                        string posterBaseUrl = "https://image.tmdb.org/t/p/w500"; // Adjust the size if needed
                        string posterUrl = posterPath != null ? posterBaseUrl + posterPath : "No poster available";

                        // Retrieve movie reviews
                        var reviewsClient = new RestClient($"https://api.themoviedb.org/3/movie/{movieId}/reviews");
                        int page = 1; // Start with page 1
                        int reviewsFetched = 0; // Track the number of reviews fetched for the movie

                        while (reviewsFetched < maxReviewsPerMovie)
                        {
                            var reviewsRequest = new RestRequest("");
                            reviewsRequest.AddHeader("accept", "application/json");
                            reviewsRequest.AddHeader("Authorization", "Bearer eyJhbGciOiJIUzI1NiJ9.eyJhdWQiOiJhZWEzNjQwN2E5YzcyNWM4ZjgyMzkwZjdmMzAwNjRhMSIsInN1YiI6IjY1MDFlMGMwNmEyMjI3MDEzNzJkZTI2MCIsInNjb3BlcyI6WyJhcGlfcmVhZCJdLCJ2ZXJzaW9uIjoxfQ.BWcS2MB1VOSfsN5-cQgmAV288mJSzZpxxj20jlfc5SE"); // Replace with your API key
                            reviewsRequest.AddParameter("language", "en-US");
                            reviewsRequest.AddParameter("page", page);

                            var reviewsResponse = await reviewsClient.GetAsync(reviewsRequest);

                            if (reviewsResponse.IsSuccessful)
                            {
                                JObject reviewsJson = JObject.Parse(reviewsResponse.Content);

                                if (reviewsJson.ContainsKey("results") && reviewsJson["results"].HasValues)
                                {
                                    var reviewResults = reviewsJson["results"];

                                    foreach (var reviewResult in reviewResults)
                                    {
                                        var author = reviewResult.Value<string>("author");
                                        var rating = reviewResult["author_details"]?["rating"];
                                        float? movieRating = null;
                                        var content = reviewResult.Value<string>("content");

                                        if (rating != null && float.TryParse(rating.ToString(), out float parsedRating))
                                        {
                                            movieRating = parsedRating;
                                        }

                                        var reviewCell = new ReviewCell
                                        {
                                            MovieTitle = movieTitle,
                                            MovieDescription = content,
                                            PosterUrl = posterUrl,
                                            MovieRating = movieRating,
                                        };

                                        ReviewFetched?.Invoke(reviewCell);

                                        Debug.WriteLine($"-------------------------------------------------------");
                                        Debug.WriteLine($"Movie: {movieTitle}");
                                        Debug.WriteLine($"Poster URL: {posterUrl}");
                                        Debug.WriteLine($"Author: {author}, Rating: {rating}, Content: {content}");
                                        Debug.WriteLine($"-------------------------------------------------------");

                                        reviewsFetched++;

                                        if (reviewsFetched >= maxReviewsPerMovie)
                                        {
                                            break; // Reached the maximum number of reviews for this movie
                                        }
                                    }

                                    // Check if there are more pages of reviews
                                    if (reviewsJson.ContainsKey("page") && reviewsJson.ContainsKey("total_pages"))
                                    {
                                        int currentPage = reviewsJson.Value<int>("page");
                                        int totalPages = reviewsJson.Value<int>("total_pages");

                                        if (currentPage < totalPages)
                                        {
                                            page++; // Move to the next page of reviews
                                        }
                                        else
                                        {
                                            break; // No more pages of reviews for this movie
                                        }
                                    }
                                    else
                                    {
                                        break; // Unable to determine pagination information, exit loop
                                    }
                                }
                                else
                                {
                                    break; // No reviews available for this movie, exit loop
                                }
                            }
                            else
                            {
                                // Handle the case where the reviews API request is not successful
                                break;
                            }
                        }
                    }
                }
            }
        }
    }
}

  
    public class PopularMoviesResponse
    {
        public List<Movie> Results { get; set; }
    }

    public class SearchResults
    {
        public List<Movie> Results { get; set; }
    }

    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Overview { get; set; }
        public DateOnly ReleaseDate { get; set; }
        public string PosterPath { get; set; }
    }
