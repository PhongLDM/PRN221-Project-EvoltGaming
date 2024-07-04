using EvoltingStore.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace EvoltingStore.Pages
{
    public class OrderDetailsModel : PageModel
    {
        private readonly EvoltingStoreContext _context;

        public OrderDetailsModel(EvoltingStoreContext context)
        {
            _context = context;
        }

        public Order Order { get; set; }

        public async Task<IActionResult> OnGetAsync(int orderId)
        {
            Order = await _context.Orders
                .Where(o => o.OrderId == orderId)
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Game)
                .FirstOrDefaultAsync();

            if (Order == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}
