using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using EvoltingStore.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EvoltingStore.Pages
{
    public class CartModel : PageModel
    {
        EvoltingStoreContext context = new EvoltingStoreContext();

        public List<CartItem> CartItems { get; set; }
        public decimal TotalPrice => CartItems.Sum(item => item.Game.Price * item.Quantity);

        public async Task OnGetAsync()
        {
            CartItems = await GetCartItemsAsync();
        }

        public async Task<IActionResult> OnPostAddAsync(int gameId, int quantity = 1)
        {
            var cartItems = await GetCartItemsAsync();
            var cartItem = cartItems.FirstOrDefault(ci => ci.Game.GameId == gameId);

            if (cartItem != null)
            {
                cartItem.Quantity += quantity;
            }
            else
            {
                var game = await _gameService.GetGameByIdAsync(gameId);
                if (game != null)
                {
                    cartItems.Add(new CartItem
                    {
                        Game = game,
                        Quantity = quantity
                    });
                }
            }

            await SaveCartItemsAsync(cartItems);
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostRemoveAsync(int gameId)
        {
            var cartItems = await GetCartItemsAsync();
            var cartItem = cartItems.FirstOrDefault(ci => ci.Game.GameId == gameId);

            if (cartItem != null)
            {
                cartItems.Remove(cartItem);
            }

            await SaveCartItemsAsync(cartItems);
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostUpdateAsync(int gameId, int quantity)
        {
            var cartItems = await GetCartItemsAsync();
            var cartItem = cartItems.FirstOrDefault(ci => ci.Game.GameId == gameId);

            if (cartItem != null)
            {
                if (quantity <= 0)
                {
                    cartItems.Remove(cartItem);
                }
                else
                {
                    cartItem.Quantity = quantity;
                }
            }

            await SaveCartItemsAsync(cartItems);
            return RedirectToPage();
        }

        private async Task<List<CartItem>> GetCartItemsAsync()
        {
            var cart = HttpContext.Session.GetString("cart");
            if (cart != null)
            {
                return await Task.FromResult(JsonConvert.DeserializeObject<List<CartItem>>(cart));
            }

            return new List<CartItem>();
        }

        private async Task SaveCartItemsAsync(List<CartItem> cartItems)
        {
            HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(cartItems));
            await Task.CompletedTask;
        }
    }
}
