using EvoltingStore.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EvoltingStore.Pages.Management
{
    public class GamesModel : PageModel
    {
        private EvoltingStoreContext context = new EvoltingStoreContext();

        public IActionResult OnGet()
        {
            String userJSON = (String)HttpContext.Session.GetString("user");
            User user = (User)Newtonsoft.Json.JsonConvert.DeserializeObject<User>(userJSON);
            if (user.RoleId == 1)
            {
                List<Game> games = context.Games.ToList();

                ViewData["games"] = games;

                return Page();
            }

            return Redirect("/Error");
        }

        public IActionResult OnPostRemove(int gameId)
        {
            String userJSON = (String)HttpContext.Session.GetString("user");
            User user = (User)Newtonsoft.Json.JsonConvert.DeserializeObject<User>(userJSON);
            if (user.RoleId == 1)
            {
                Game g = context.Games.FirstOrDefault(g => g.GameId == gameId);

                context.Remove(g);
                context.SaveChanges();

                context = new EvoltingStoreContext();

                List<Game> games = context.Games.ToList();

                ViewData["games"] = games;

                return Page();
            }

            return Redirect("/Error");
        }
    }
}
