using EvoltingStore.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security;
using Newtonsoft.Json;

namespace EvoltingStore.Pages
{
    public class LoginModel : PageModel
    {
        private EvoltingStoreContext context = new EvoltingStoreContext();

        public void OnGet()
        {
            if (TempData.ContainsKey("message"))
            {
                ViewData["message"] = TempData["message"].ToString();
            }
            else
            {
                ViewData["message"] = "";
            }
        }

        public IActionResult OnPost(String username, String password)
        {
            User user = context.Users.FirstOrDefault(u => u.Username.Equals(username) && u.Password.Equals(password));

            if (user == null)
            {
                ViewData["message"] = "Wrong credential information";

                return Page();
            }
            else
            {
                if (user.IsActive)
                {
                    String jsonUser = Newtonsoft.Json.JsonConvert.SerializeObject(user);
                    HttpContext.Session.SetString("user", jsonUser);

                    UserDetail userDetail = context.UserDetails.FirstOrDefault(u => u.UserId == user.UserId);
                    String jsonUserDetail = Newtonsoft.Json.JsonConvert.SerializeObject(userDetail);
                    HttpContext.Session.SetString("userDetail", jsonUserDetail);

                    return Redirect("/Index");
                }
                else
                {
                    return Redirect("/Error");
                }
            }
        }
    }
}
