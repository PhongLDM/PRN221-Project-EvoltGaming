using EvoltingStore.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace EvoltingStore.Pages
{
    public class FavouriteModel : PageModel
    {
        private EvoltingStoreContext context = new EvoltingStoreContext();

        private static int ROLE_USER = 2;

        [BindProperty(SupportsGet = true)]
        public User? CurrentUser { get; set; } = null;

        public async Task<IActionResult> OnGetAsync(String orderBy)
        {
            String userJSON = HttpContext.Session.GetString("user");

            if (String.IsNullOrEmpty(userJSON))
            {
                return RedirectToPage("/Login");
            }

            User user = Newtonsoft.Json.JsonConvert.DeserializeObject<User>(userJSON);
            CurrentUser = await context.Users
                .Include(u => u.UserDetail)
                .Include(u => u.Cart)
                .FirstOrDefaultAsync(u => u.UserId == user.UserId);

            if (CurrentUser != null)
            {
                if (CurrentUser.RoleId == ROLE_USER)
                {
                    List<Game> games = context.Games
                                        .Include(g => g.Genres)
                                        .Include(g => g.Comments)
                                        .Include(g => g.Users)
                                        .Where(g => g.Users.Any(u => u.UserId == CurrentUser.UserId))
                                        .ToList();

                    List<Genre> genres = context.Genres.ToList();
                    List<Boolean> selected = new List<Boolean>();
                    for (int i = 1; i <= genres.Count; i++)
                    {
                        selected.Add(false);
                    }

                    if (orderBy != null && orderBy.Length > 0)
                    {
                        switch (orderBy)
                        {
                            case "Name":
                                games = games.OrderByDescending(g => g.Name).ToList();
                                break;
                            case "UpdateDate":
                                games = games.OrderByDescending(g => g.UpdateDate).ToList();
                                break;
                            case "ReleaseDate":
                                games = games.OrderByDescending(g => g.ReleaseDate).ToList();
                                break;
                            case "Favourites":
                                games = games.OrderByDescending(g => g.Users.Count).ToList();
                                break;
                        }

                        ViewData["orderBy"] = orderBy;
                    }

                    ViewData["games"] = games;
                    ViewData["genres"] = genres;
                    ViewData["selected"] = selected;
                }
                else
                {
                    return RedirectToPage("/Index");
                }
            }
            else
            {
                return RedirectToPage("/Login");
            }

            return Page();
        }

        public async Task OnPostFavouriteAsync(int gameId)
        {
            List<Game> games = context.Games
                    .Include(g => g.Genres)
                    .Include(g => g.Comments)
                    .Include(g => g.Users)
                    .Where(g => g.Users.Any(u => u.UserId == CurrentUser.UserId))
                    .ToList();

            List<Genre> genres = context.Genres.ToList();
            List<Boolean> selected = new List<Boolean>();
            for (int i = 1; i <= genres.Count; i++)
            {
                selected.Add(false);
            }

            Game game = context.Games.Include(g => g.Genres).Include(g => g.Comments).ThenInclude(c => c.User).Include(g => g.Users).FirstOrDefault(g => g.GameId == gameId);

            String userJSON = HttpContext.Session.GetString("user");
            if (userJSON != null)
            {
                User us = Newtonsoft.Json.JsonConvert.DeserializeObject<User>(userJSON);

                User user = context.Users.FirstOrDefault(u => u.UserId == us.UserId);


                if (game.Users.Contains(user))
                {
                    game.Users.Remove(user);
                }
                else
                {
                    game.Users.Add(user);
                }
            }

            context.Games.Update(game);
            context.SaveChanges();

            ViewData["games"] = games;
            ViewData["genres"] = genres;
            ViewData["selected"] = selected;
        }

        public void OnPostFilter(List<int> genre, string searchName, string orderBy)
        {
            String userJSON = HttpContext.Session.GetString("user");

            User user = Newtonsoft.Json.JsonConvert.DeserializeObject<User>(userJSON);
            CurrentUser = context.Users
                .Include(u => u.UserDetail)
                .Include(u => u.Cart)
                .FirstOrDefault(u => u.UserId == user.UserId);

            List<Game> games = new List<Game>();
            List<Boolean> selected = new List<Boolean>();
            List<Genre> genres = context.Genres.ToList();

            if (genre.Count > 0)
            {
                List<Genre> selectedGenre = new List<Genre>();

                foreach (var genreId in genre)
                {
                    selectedGenre.Add(genres[genreId - 1]);
                }

                foreach (var game in context.Games
                                        .Include(g => g.Genres)
                                        .Include(g => g.Comments)
                                        .Include(g => g.Users)
                                        .Where(g => g.Users.Any(u => u.UserId == CurrentUser.UserId))
                                        .ToList())
                {
                    HashSet<Genre> common = new HashSet<Genre>(game.Genres);
                    common.IntersectWith(selectedGenre);
                    if (common.Count == selectedGenre.Count)
                    {
                        games.Add(game);
                    }
                }

                for (int i = 1; i <= genres.Count; i++)
                {
                    if (genre.Contains(i)) selected.Add(true);
                    else selected.Add(false);
                }
            }
            else
            {
                games = context.Games
                    .Include(g => g.Genres)
                    .Include(g => g.Comments)
                    .Include(g => g.Users)
                    .Where(g => g.Users.Any(u => u.UserId == CurrentUser.UserId))
                    .ToList(); ;

                for (int i = 1; i <= genres.Count; i++)
                {
                    selected.Add(false);
                }
            }

            if (searchName != null && searchName.Length > 0)
            {
                games = (from g in games
                         where g.Name.ToLower().Contains(searchName.ToLower())
                         select g).ToList();
            }

            switch (orderBy)
            {
                case "Name":
                    games = games.OrderByDescending(g => g.Name).ToList();
                    break;
                case "UpdateDate":
                    games = games.OrderByDescending(g => g.UpdateDate).ToList();
                    break;
                case "ReleaseDate":
                    games = games.OrderByDescending(g => g.ReleaseDate).ToList();
                    break;
                case "Favourites":
                    games = games.OrderByDescending(g => g.Users.Count).ToList();
                    break;
            }

            ViewData["games"] = games;
            ViewData["genres"] = genres;
            ViewData["selected"] = selected;

            ViewData["orderBy"] = orderBy;
            ViewData["searchName"] = searchName;
        }
    }
}