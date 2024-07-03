using EvoltingStore.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace EvoltingStore.Pages.Management.Orders
{
    public class DetailModel : PageModel
    {

        private EvoltingStoreContext context = new EvoltingStoreContext();
        public IActionResult OnGet(string OrderId)
        {
            try {
                int id = Int32.Parse(OrderId);
                if (id == 0) {
                    throw new Exception();
                }

                Order order = context.Orders
                    .Include(o => o.OrderDetails)
                    .Include(o => o.User)
                    .FirstOrDefault(o => o.OrderId == id);
                IQueryable<OrderDetail> orderDetails = order.OrderDetails.AsQueryable();
                orderDetails.Include(o => o.Game);


                ViewData["order"] = order;
                ViewData["orderDetails"] = orderDetails.ToList();


            } catch (Exception ex) {
                Debug.WriteLine("Hello" + ex);
                return Redirect("/Error");
            }


            return Page();
        }
    }
}
