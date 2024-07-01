using EvoltingStore.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace EvoltingStore.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        private EvoltingStoreContext context = new EvoltingStoreContext();

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            List<Game> topFavourites = context.Games.Include(g => g.Genres).Include(g => g.Comments).Include(g => g.Users).OrderByDescending(g => g.Users.Count).Take(6).ToList();
            List<Game> latestRelease = context.Games.Include(g => g.Genres).Include(g => g.Comments).Include(g => g.Users).OrderByDescending(g => g.ReleaseDate).Take(6).ToList();
            List<Game> latestUpdate = context.Games.Include(g => g.Genres).Include(g => g.Comments).Include(g => g.Users).OrderByDescending(g => g.UpdateDate).Take(6).ToList();
            List<Comment> comments = context.Comments.OrderByDescending(c => c.PostTime).ToList();

            List<Game> latestCommment = comments.Select(c => c.Game).ToList();
            latestCommment = latestCommment.GroupBy(g => g.GameId).Select(g => g.First()).Take(6).ToList();

            ViewData["topFavourites"] = topFavourites;
            ViewData["latestRelease"] = latestRelease;
            ViewData["latestUpdate"] = latestUpdate;
            ViewData["latestCommment"] = latestCommment;
            String jsonUser = HttpContext.Session.GetString("user");
            User user = null;
            if (jsonUser != null)
            {
                user = Newtonsoft.Json.JsonConvert.DeserializeObject<User>(jsonUser);
            }

            ViewData["user"] = user;
        }
    }
}