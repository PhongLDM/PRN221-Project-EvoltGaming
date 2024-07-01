using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EvoltingStore.Pages
{
    public class LogoutModel : PageModel
    {
        public IActionResult OnGet()
        {
            HttpContext.Session.Remove("user");
            HttpContext.Session.Remove("userDetail");

            return Redirect("/Index");
        }
    }
}
