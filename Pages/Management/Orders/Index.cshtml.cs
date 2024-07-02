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

                ICollection<Order> orders = new List<Order>();
                Order order = new Order();
                order.OrderId = 1;
                order.Status = "Shipping";
                order.TotalPrice = 12f;
                order.OrderDate = DateTime.Now;
                order.Payment = "COD";
                order.UserId = 1;
                order.OrderDetails = new List<OrderDetail>();
                OrderDetail orderDetail = new OrderDetail();
                orderDetail.OrderId = 1;
                orderDetail.Order = order;
                orderDetail.GameId = 1;
                order.OrderDetails.Add(orderDetail);
                orders.Add(order);




                ViewData["orders"] = orders;

                return Page();
            }

            return Redirect("/Error");
        }
    }
}
