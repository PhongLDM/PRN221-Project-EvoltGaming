using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EvoltingStore.Entity;

namespace EvoltingStore.Pages.Management
{
    public class UserDetailsModel : PageModel
    {
        EvoltingStoreContext context = new EvoltingStoreContext();

        public IActionResult OnGet(int userId)
        {
            //var user = (String)HttpContext.Session.GetString("user");
            //var userData = (User)Newtonsoft.Json.JsonConvert.DeserializeObject<User>(user);
            //if (userData.RoleId != 1)
            //{
            //    return;
            //}
            String userJSON = (String)HttpContext.Session.GetString("user");
            User user = (User)Newtonsoft.Json.JsonConvert.DeserializeObject<User>(userJSON);
            if (user.RoleId == 1)
            {
                UserDetail userDetail = context.UserDetails.FirstOrDefault(ud => ud.UserId == userId);

                ViewData["profile"] = userDetail;

                return Page();
            }

            return Redirect("/Error");

        }
    }
}
