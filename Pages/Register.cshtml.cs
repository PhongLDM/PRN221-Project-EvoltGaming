using EvoltingStore.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EvoltingStore.Pages
{
    public class RegisterModel : PageModel
    {
        EvoltingStoreContext context = new EvoltingStoreContext();

        public void OnGet()
        {
        }

        public IActionResult OnPost(String username, String password, String firstName, String lastName, String email)
        {
            UserDetail ud = new UserDetail();
            ud.FirstName = firstName;
            ud.LastName = lastName;
            ud.Email = email;
            ud.CreatedDate = DateTime.Now;

            User u = new User();
            u.Username = username;
            u.Password = password;
            u.RoleId = 2;
            u.IsActive = true;
            u.UserDetail = ud;

            context.Users.Add(u);
            context.SaveChanges();

            TempData["message"] = "Login with your registered account";
            return Redirect("/Login");
        }
    }
}
