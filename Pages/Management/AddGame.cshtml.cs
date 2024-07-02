using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using dotenv.net;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using EvoltingStore.Entity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace EvoltingStore.Pages.Management
{
    public class AddGameModel : PageModel
    {
        private EvoltingStoreContext context = new EvoltingStoreContext();

        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _environment;
        public AddGameModel(Microsoft.AspNetCore.Hosting.IHostingEnvironment environment)
        {
            _environment = environment;
        }

        public IActionResult OnGet()
        {

            String userJSON = (String)HttpContext.Session.GetString("user");
            User user = (User)Newtonsoft.Json.JsonConvert.DeserializeObject<User>(userJSON);
            if(user.RoleId == 1)
            {
                List<Genre> allGenres = context.Genres.ToList();

                string jsonGenre = Newtonsoft.Json.JsonConvert.SerializeObject(allGenres);

                ViewData["genres"] = jsonGenre;

                return Page();
            }

            return Redirect("/Error");
        }


        public async Task<IActionResult> OnPost(IFormFile gameImg, Game game, List<int> genres)
        {
            var context = new EvoltingStoreContext();
            List<Genre> allGenres = context.Genres.ToList();

            if (gameImg != null)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                var filePath = Path.Combine(uploadsFolder, gameImg.FileName);
                Directory.CreateDirectory(uploadsFolder);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await gameImg.CopyToAsync(fileStream);
                }
                game.Image = filePath;
            }


            game.UpdateDate = DateTime.Now;

            foreach(var g in genres)
            {
                game.Genres.Add(allGenres[g-1]);
            }

            context.Games.Add(game);
            context.SaveChanges();

            context = new EvoltingStoreContext();

            return Redirect("/Management/Games");
        }
    }
}
