using EvoltingStore.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EvoltingStore.Pages.Management.Orders
{
    public class IndexModel : PageModel
    {

        private EvoltingStoreContext context = new EvoltingStoreContext();
        public IActionResult OnGet() {
            String userJSON = (String)HttpContext.Session.GetString("user");
            User user = (User)Newtonsoft.Json.JsonConvert.DeserializeObject<User>(userJSON);
            if (user.RoleId == 1) {
                ViewData["orders"] = context.Orders.ToList();

                return Page();
            }

            return Redirect("/Error");
        }
    }
}
