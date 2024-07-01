using EvoltingStore.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace EvoltingStore.Pages
{
    public class BlogModel : PageModel
    {
        public async Task OnGetAsync()
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://videogames-news2.p.rapidapi.com/videogames_news/search_news?query=game"),
                Headers =
                {
                    { "X-RapidAPI-Key", "a183a6bf1cmshc0ecd4f7ec60cdep1ecbc5jsn50d81c06026d" },
                    { "X-RapidAPI-Host", "videogames-news2.p.rapidapi.com" },
                },
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                
                List<News> news = JsonConvert.DeserializeObject<List<News>>(body);

                ViewData["news"] = news;
            }
        }

        public async Task OnPostAsync(String query)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://videogames-news2.p.rapidapi.com/videogames_news/search_news?query="+query),
                Headers =
                {
                    { "X-RapidAPI-Key", "a183a6bf1cmshc0ecd4f7ec60cdep1ecbc5jsn50d81c06026d" },
                    { "X-RapidAPI-Host", "videogames-news2.p.rapidapi.com" },
                },
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();

                List<News> news = JsonConvert.DeserializeObject<List<News>>(body);

                ViewData["news"] = news;
            }
        }
    }
}
