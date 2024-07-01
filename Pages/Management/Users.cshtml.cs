using EvoltingStore.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace EvoltingStore.Pages.Management
{
    public class UsersModel : PageModel
    {
        private EvoltingStoreContext context = new EvoltingStoreContext();

        public IActionResult OnGet()
        {
            String userJSON = (String)HttpContext.Session.GetString("user");
            User user = (User)Newtonsoft.Json.JsonConvert.DeserializeObject<User>(userJSON);
            if (user.RoleId == 1)
            {
                List<User> users = context.Users.Include(u => u.Role).Include(u => u.UserDetail).ToList();

                ViewData["users"] = users;

                return Page();
            }

            return Redirect("/Error");
        }

        public void OnPostStatus(int userId)
        {
            User user = context.Users.FirstOrDefault(u => u.UserId == userId);
            user.IsActive = !user.IsActive;

            context.SaveChanges();

            List<User> users = context.Users.Include(u => u.Role).Include(u => u.UserDetail).ToList();

            ViewData["users"] = users;
        }
    }
}
