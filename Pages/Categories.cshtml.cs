using EvoltingStore.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace EvoltingStore.Pages
{
    public class CategoriesModel : PageModel
    {
        private EvoltingStoreContext context = new EvoltingStoreContext();

        public void OnGet(String orderBy)
        {
            List<Game> games = context.Games.Include(g => g.Genres).Include(g => g.Comments).Include(g => g.Users).OrderBy(g => g.Name).ToList();
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

        public void OnPostFilter(List<int> genre, string searchName, string orderBy)
        {
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

                foreach (var game in context.Games.Include(g => g.Genres).Include(g => g.Comments).Include(g => g.Users).ToList())
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
                games = context.Games.Include(g => g.Genres).Include(g => g.Comments).Include(g => g.Users).ToList();

                for (int i = 1; i <= genres.Count; i++)
                {
                    selected.Add(false);
                }
            }

            if(searchName != null && searchName.Length > 0)
            {
                games = (from g in games
                         where g.Name.Contains(searchName)
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
