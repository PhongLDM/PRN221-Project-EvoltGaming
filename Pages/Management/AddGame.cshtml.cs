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
                var file = Path.Combine(_environment.ContentRootPath, "uploads", gameImg.FileName);
                using (var fileStream = new FileStream(file, FileMode.Create))
                {
                    await gameImg.CopyToAsync(fileStream);
                }
            }

            DotEnv.Load(options: new DotEnvOptions(probeForEnv: true));
            Cloudinary cloudinary = new Cloudinary(Environment.GetEnvironmentVariable("CLOUDINARY_URL"));
            cloudinary.Api.Secure = true;

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(Path.Combine(_environment.ContentRootPath, "uploads", gameImg.FileName)),
                UseFilename = true,
                UniqueFilename = false,
                Overwrite = true
            };
            var uploadResult = cloudinary.Upload(uploadParams);
            var myJsonString = uploadResult.JsonObj.ToString();
            var data = JObject.Parse(myJsonString);

            var url = data["url"].ToString();

            game.Image = url;
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
