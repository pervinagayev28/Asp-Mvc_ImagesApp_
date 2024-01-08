using System.Text.Json;
using WebApplication1.Models;

namespace WebApplication1.ConnectionApi
{
    public static class GetImages
    {

        public static async Task<List<IImage>> GetImageFromUnsplash(string searchQuery)
        {
            List<IImage> imageUrls = new();
            using (var httpClient = new HttpClient())
            {
                string unsplashUrl = $"https://api.unsplash.com/photos/random?query={searchQuery}&count=10&client_id=v1JB5ll3WklUri_rrosq1YQOFbvsIrJKRjRwnhCdNVw";
                HttpResponseMessage unsplashResponse = await httpClient.GetAsync(unsplashUrl);
                if (unsplashResponse.IsSuccessStatusCode)
                {
                    JsonDocument responseJson = JsonDocument.Parse(await unsplashResponse.Content.ReadAsStringAsync());

                    foreach (var photo in responseJson.RootElement.EnumerateArray())
                    {
                        string imageUrl = photo.GetProperty("urls").GetProperty("regular").GetString();
                        imageUrls.Add(new Image() { Url = imageUrl, State = true }) ;
                    }
                }
            }

            return imageUrls;

        }
    }
}

