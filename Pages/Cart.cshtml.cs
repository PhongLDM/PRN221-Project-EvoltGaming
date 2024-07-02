using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EvoltingStore.Entity;
using Microsoft.EntityFrameworkCore;
using System.Runtime.ConstrainedExecution;

namespace EvoltingStore.Pages
{
    public class CartModel : PageModel
    {
        private static int ROLE_USER = 2;
        private readonly EvoltingStoreContext _context = new EvoltingStoreContext();

        [BindProperty(SupportsGet = true)]
        public User? CurrentUser { get; set; } = null;

        Cart Cart { get; set; }

        public IList<CartItem> CartItems { get; set; } = default!;

        public double TotalPrice => CartItems.Sum(item => item.Game.Price);

        public async Task<IActionResult> OnGetAsync()
        {
            String userJSON = HttpContext.Session.GetString("user");

            if (String.IsNullOrEmpty(userJSON))
            {
                return RedirectToPage("/Login");
            }

            User u = Newtonsoft.Json.JsonConvert.DeserializeObject<User>(userJSON);
            CurrentUser = await _context.Users
                .Include(u => u.UserDetail)
                .Include(u => u.Cart)
                .FirstOrDefaultAsync(u => u.UserId == u.UserId);

            if (CurrentUser != null)
            {
                if (CurrentUser.RoleId == ROLE_USER)
                {
                    Cart = await CheckCart();
                    CartItems = await GetCartItemsAsync();
                }
            }
            else
            {
                return RedirectToPage("/Login");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAddAsync(int gameId)
        {
            String userJSON = HttpContext.Session.GetString("user");

            if (String.IsNullOrEmpty(userJSON))
            {
                return RedirectToPage("/Login");
            }

            User u = Newtonsoft.Json.JsonConvert.DeserializeObject<User>(userJSON);
            CurrentUser = await _context.Users
                .Include(u => u.UserDetail)
                .Include(u => u.Cart)
                .FirstOrDefaultAsync(u => u.UserId == u.UserId);

            if (CurrentUser != null)
            {
                if (CurrentUser.RoleId == ROLE_USER)
                {
                    Cart = await CheckCart();
                    var cartItems = await GetCartItemsAsync();
                    var cartItem = cartItems.FirstOrDefault(ci => ci.GameId == gameId);

                    if (cartItem == null)
                    {
                        var game = await _context.Games.FirstOrDefaultAsync(g => g.GameId == gameId);
                        if (game != null)
                        {
                            await _context.CartItems.AddAsync(new CartItem
                            {
                                Cart = Cart,
                                Game = game,
                            });
                        }
                    }

                    await _context.SaveChangesAsync();
                }
                return RedirectToPage();
            }
            else
            {
                return RedirectToPage("/Login");
            }
        }

        public async Task<IActionResult> OnPostRemoveAsync(int cartItemId)
        {
            if (cartItemId != 0)
            {
                var cartItem = _context.CartItems.FirstOrDefault(ci => ci.Id == cartItemId);
                if (cartItem != null)
                {
                    _context.CartItems.Remove(cartItem);
                    await _context.SaveChangesAsync();
                }
            } 
            return RedirectToPage();
        }

        private async Task<List<CartItem>> GetCartItemsAsync()
        {
            return await _context.CartItems.Include(ci => ci.Game).Where(c => c.CartId == Cart.Id).ToListAsync();
        }

        private async Task<Cart> CheckCart()
        {
            
            if (CurrentUser.CartId == null)
            {
                Cart c = new Cart()
                {
                    UserId = CurrentUser.UserId
                };
                await _context.AddAsync(c);
                CurrentUser.Cart = c;
                await _context.SaveChangesAsync();
                return c;
            }
            else
            {
                return CurrentUser.Cart;
            }
        }
    }
}
