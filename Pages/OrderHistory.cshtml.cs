using EvoltingStore.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace EvoltingStore.Pages
{
    public class OrderHistoryModel : PageModel
    {
        private readonly EvoltingStoreContext _context;

        public OrderHistoryModel(EvoltingStoreContext context)
        {
            _context = context;
        }

        public IList<Order> Orders { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {

            String userJSON = HttpContext.Session.GetString("user");

            if (String.IsNullOrEmpty(userJSON))
            {
                return RedirectToPage("/Login");
            }

            User user = Newtonsoft.Json.JsonConvert.DeserializeObject<User>(userJSON);

            if (user != null)
            {
                if (user.RoleId == 2)
                {
                    int userId = user.UserId;

                    Orders = await _context.Orders
                        .Where(o => o.UserId == userId)
                        .Include(o => o.OrderDetails)
                        .ThenInclude(od => od.Game)
                        .ToListAsync();
                }
            }
            else
            {
                return RedirectToPage("/Login");
            }

            return Page();
        }
    }
}
