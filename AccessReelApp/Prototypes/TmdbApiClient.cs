using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using AccessReelApp.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace AccessReelApp.Prototypes
{

    public class TmdbApiClient
    {
        private readonly string apiKey;
        private readonly RestClient restClient;
        public event Action<ReviewCell> ReviewFetched;

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

        public async Task GetReviewsForMoviesByName(string movieName, int maxReviewsPerMovie)
        {
            var searchClient = new RestClient("https://api.themoviedb.org/3/search/movie");
            var searchRequest = new RestRequest("");
            searchRequest.AddHeader("accept", "application/json");
            searchRequest.AddHeader("Authorization", "Bearer eyJhbGciOiJIUzI1NiJ9.eyJhdWQiOiJhZWEzNjQwN2E5YzcyNWM4ZjgyMzkwZjdmMzAwNjRhMSIsInN1YiI6IjY1MDFlMGMwNmEyMjI3MDEzNzJkZTI2MCIsInNjb3BlcyI6WyJhcGlfcmVhZCJdLCJ2ZXJzaW9uIjoxfQ.BWcS2MB1VOSfsN5-cQgmAV288mJSzZpxxj20jlfc5SE"); // Replace with your API key
            searchRequest.AddParameter("language", "en-US");
            searchRequest.AddParameter("query", movieName);

            var searchResponse = await searchClient.GetAsync(searchRequest);

            if (searchResponse.IsSuccessful)
            {
                JObject searchResults = JObject.Parse(searchResponse.Content);

                if (searchResults.ContainsKey("results") && searchResults["results"].HasValues)
                {
                    var result = searchResults["results"][0]; // Assuming the first result is the one you want
                    int movieId = result.Value<int>("id");
                    string movieTitle = result.Value<string>("title");
                    string posterPath = result.Value<string>("poster_path");

                    string posterBaseUrl = "https://image.tmdb.org/t/p/w500"; // Adjust the size if needed
                    string posterUrl = posterPath != null ? posterBaseUrl + posterPath : "No poster available";

                    // Retrieve movie details and reviews for this movie by ID
                    await GetMovieDetailsAndReviews(movieId, movieTitle, posterUrl, maxReviewsPerMovie);
                }
            }
        }

        public async Task GetMovieDetailsAndReviews(int movieId, string movieTitle, string posterUrl, int maxReviewsPerMovie)
        {
            // Fetch movie details here if needed, similar to the previous example
            // You can make an additional API call to get more details about the movie

            // Now, retrieve movie reviews for this movie by ID
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

        // Access Reel Methods

        public static async Task PullAccessReelData()
        {
            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync("https://accessreel.com/wp-json/api/v1/trailers?posts_per_page=-1&exclude_hidden=1");

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    var posts = JsonConvert.DeserializeObject<List<Post>>(json);

                    foreach (var post in posts)
                    {
                        Debug.WriteLine($"Post ID: {post.ID}");
                        Debug.WriteLine($"Post Title: {post.post_title}");
                        Debug.WriteLine($"Post Date: {post.post_date}");

                        if (post.review_info != null)
                        {
                            foreach (var review in post.review_info)
                            {
                                Debug.WriteLine($"Review ID: {review.ID}");
                                Debug.WriteLine($"Review Title: {review.post_title}");
                                Debug.WriteLine($"Review Date: {review.post_date}");

                            }
                        }

                        // Add a separator line for better readability
                        Debug.WriteLine(new string('-', 50));
                    }
                }
                else
                {
                    Console.WriteLine($"Error: {response.StatusCode}");
                }
            }
        }
    }
}

// TMDB Client Classes
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

// AccessReelJSON Classes

public class Img
{
    public string thumbnail { get; set; }
    public string medium { get; set; }
    public string full { get; set; }
}

public class Banner
{
    public string url { get; set; }
    public int id { get; set; }
    public int height { get; set; }
    public int width { get; set; }
    public string thumbnail { get; set; }
}

public class Poster
{
    public string thumbnail { get; set; }
    public string medium { get; set; }
    public string full { get; set; }
}

public class FilmInfo
{
    public int film_id { get; set; }
    public string us_release_date { get; set; }
    public string au_release_date { get; set; }
    public string studio { get; set; }
    public IList<string> genre { get; set; }
    public IList<string> director { get; set; }
    public IList<string> cast { get; set; }
    public string homepage { get; set; }
    public string imdb_id { get; set; }
    public string imdb_vote_avg { get; set; }
    public string imdb_vote_count { get; set; }
    public Banner banner { get; set; }
    public Poster poster { get; set; }
}

public class ReviewInfo
{
    public int ID { get; set; }
    public string post_author { get; set; }
    public string post_date { get; set; }
    public string post_date_gmt { get; set; }
    public string post_content { get; set; }
    public string post_title { get; set; }
    public string post_excerpt { get; set; }
    public string post_status { get; set; }
    public string comment_status { get; set; }
    public string ping_status { get; set; }
    public string post_password { get; set; }
    public string post_name { get; set; }
    public string to_ping { get; set; }
    public string pinged { get; set; }
    public string post_modified { get; set; }
    public string post_modified_gmt { get; set; }
    public string post_content_filtered { get; set; }
    public int post_parent { get; set; }
    public string guid { get; set; }
    public int menu_order { get; set; }
    public string post_type { get; set; }
    public string post_mime_type { get; set; }
    public string comment_count { get; set; }
    public string filter { get; set; }
    public int film { get; set; }
    public int thumbnail_id { get; set; }
    public Img img { get; set; }
    public Banner banner { get; set; }
    public IList<string> rating { get; set; }
    public IList<string> good_points { get; set; }
    public IList<string> bad_points { get; set; }
    public string summary { get; set; }
}

public class Post
{
    public int ID { get; set; }
    public string post_author { get; set; }
    public string post_date { get; set; }
    public string post_date_gmt { get; set; }
    public string post_content { get; set; }
    public string post_title { get; set; }
    public string post_excerpt { get; set; }
    public string post_status { get; set; }
    public string comment_status { get; set; }
    public string ping_status { get; set; }
    public string post_password { get; set; }
    public string post_name { get; set; }
    public string to_ping { get; set; }
    public string pinged { get; set; }
    public string post_modified { get; set; }
    public string post_modified_gmt { get; set; }
    public string post_content_filtered { get; set; }
    public int post_parent { get; set; }
    public string guid { get; set; }
    public int menu_order { get; set; }
    public string post_type { get; set; }
    public string post_mime_type { get; set; }
    public string comment_count { get; set; }
    public string filter { get; set; }
    public int film { get; set; }
    public int thumbnail_id { get; set; }
    public Img img { get; set; }
    public string vimeo { get; set; }
    public FilmInfo film_info { get; set; }
    public IList<ReviewInfo> review_info { get; set; }
    public string hidden { get; set; }
}




