using EvoltingStore.Entity;
using EvoltingStore.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PayPalCheckoutSdk.Orders;

namespace EvoltingStore.Pages
{
    public class PaypalCheckoutModel : PageModel
    {
        private readonly EvoltingStoreContext _context = new EvoltingStoreContext();

        public async Task<IActionResult> OnGetCreateOrder()
        {
            var request = new OrdersCreateRequest();
            request.Headers.Add("prefer", "return=representation");
            request.RequestBody(await BuildRequestBody());

            var response = await PayPalClient.Client().Execute(request);
            var statusCode = response.StatusCode;
            var result = response.Result<PayPalCheckoutSdk.Orders.Order>();

            return new JsonResult(new { id = result.Id });
        }

        public async Task<IActionResult> OnGetCaptureOrder([FromQuery] string orderID)
        {
            var captureRequest = new OrdersCaptureRequest(orderID);
            captureRequest.RequestBody(new OrderActionRequest());

            var response = await PayPalClient.Client().Execute(captureRequest);
            var statusCode = response.StatusCode;
            var result = response.Result<PayPalCheckoutSdk.Orders.Order>();

            return new JsonResult(result);
        }

        private async Task<OrderRequest> BuildRequestBody()
        {
            String userJSON = HttpContext.Session.GetString("user");

            User user = Newtonsoft.Json.JsonConvert.DeserializeObject<User>(userJSON);

            var userCart = await  _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Game)
                .FirstOrDefaultAsync(c => c.UserId == user.UserId);
            return new OrderRequest()
            {

                CheckoutPaymentIntent = "CAPTURE",
                PurchaseUnits = new List<PurchaseUnitRequest>()
                {
                    new PurchaseUnitRequest()
                    {
                        AmountWithBreakdown = new AmountWithBreakdown()
                        {
                            CurrencyCode = "USD",
                            Value = userCart.CartItems.Sum(ci => ci.Game.Price).ToString()
                        }
                    }
                },
                ApplicationContext = new ApplicationContext()
                {
                    ReturnUrl = "https://example.com/return",
                    CancelUrl = "https://example.com/cancel"
                }
            };
        }

        public class CaptureOrderRequest
        {
            public string OrderID { get; set; }
        }
    }
}
