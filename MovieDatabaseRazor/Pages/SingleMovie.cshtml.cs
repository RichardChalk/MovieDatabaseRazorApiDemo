using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MovieDatabaseRazor.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace MovieDatabaseRazor.Pages
{
    public class SingleMovieModel : PageModel
    {
        public MovieSingleResponse Movie { get; set; }
        public async Task<IActionResult> OnGet(string movieId)
        {
            using var client = new HttpClient();
            client.BaseAddress = new Uri("https://moviesdatabase.p.rapidapi.com");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("X-RapidAPI-Key", "7306dc1932msh2d45ff9ccce401ap193e40jsn0c2d7d3d328b");

            HttpResponseMessage response = await client.GetAsync($"/titles/{movieId}");
            if (response.IsSuccessStatusCode)
            {
                // G�r om responsen till en str�ng
                var responseBody = await response.Content.ReadAsStringAsync();
                // G�r om str�ngen till v�r egen skapade datatyp - KrisInfoResponse
                Movie = JsonConvert.DeserializeObject<MovieSingleResponse>(responseBody);

                if (Movie.Results.PrimaryImage==null)
                {
                    Movie.Results.PrimaryImage = new PrimaryImage();
                    Movie.Results.PrimaryImage.URL = "NoMovieImage.jpg";

                    Movie.Results.PrimaryImage.Caption = new Caption();
                    Movie.Results.PrimaryImage.Caption.PlainText = "Filmbeskrivning saknas";
                }
            }
            return Page();
        }
    }
}
