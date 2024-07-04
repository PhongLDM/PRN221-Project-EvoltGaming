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
            String userJSON = (String)HttpContext.Session.GetString("user");
            User user = (User)Newtonsoft.Json.JsonConvert.DeserializeObject<User>(userJSON);
            if (user.RoleId == 1) {
                try {
                    int id = Int32.Parse(OrderId);
                    if (id == 0) {
                        throw new Exception();
                    }

                    Order order = context.Orders
                        .Include(o => o.OrderDetails)
                        .Include(o => o.User)
                        .FirstOrDefault(o => o.OrderId == id);
                    User userOrdered = context.Users
                        .Include(u => u.UserDetail)
                        .FirstOrDefault(o => o.UserId == order.UserId);
                    List<OrderDetail> orderDetails = new List<OrderDetail>();
                    foreach (var od in order.OrderDetails) {
                        orderDetails.Add(
                            (OrderDetail)context.OrderDetails
                            .Include(detail => detail.Game)
                            .FirstOrDefault(detail => detail.Id == od.Id)
                            );
                    }

                    ViewData["userOrdered"] = userOrdered;
                    ViewData["order"] = order;
                    ViewData["orderDetails"] = orderDetails;


                } catch (Exception ex) {
                    Debug.WriteLine("Hello" + ex);
                    return Redirect("/Error");
                }
                return Page();
            } else {
                return Redirect("/Error");
            }
        }

        public IActionResult OnPost(string OrderId) {
            String userJSON = (String)HttpContext.Session.GetString("user");
            User user = (User)Newtonsoft.Json.JsonConvert.DeserializeObject<User>(userJSON);
            if (user.RoleId == 1) {
                ViewData["orders"] = context.Orders.ToList();
                try {
                    int id = Int32.Parse(OrderId);
                    if (id == 0) {
                        throw new Exception();
                    }

                    Order order = context.Orders
                        .Include(o => o.OrderDetails)
                        .Include(o => o.User)
                        .FirstOrDefault(o => o.OrderId == id);
                    order.Status = true;
                    context.Orders.Update(order);
                    context.SaveChanges();

                } catch (Exception ex) {
                    Debug.WriteLine("Hello" + ex);
                    return Redirect("/Error");
                }
                return Redirect("Detail?OrderId=" + OrderId);
            }

            return Redirect("/Error");

        }
    }
}
