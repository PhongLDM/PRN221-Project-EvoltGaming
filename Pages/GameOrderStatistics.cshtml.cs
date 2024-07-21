using EvoltingStore.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace EvoltingStore.Pages
{
    public class GameOrderStatisticsModel : PageModel
    {
        private readonly EvoltingStoreContext _context;
        private static readonly int ROLE_USER = 1;
        private static readonly DateTime Today = DateTime.UtcNow;

        public GameOrderStatisticsModel(EvoltingStoreContext context)
        {
            _context = context;
        }

        [BindProperty(SupportsGet = true)]
        public User? CurrentUser { get; set; } = null;

        public int IsAdmin()
        {
            string userJSON = HttpContext.Session.GetString("user");

            if (string.IsNullOrEmpty(userJSON))
            {
                return 0;
            }

            User user = Newtonsoft.Json.JsonConvert.DeserializeObject<User>(userJSON);
            CurrentUser = _context.Users
                .Include(u => u.UserDetail)
                .Include(u => u.Cart)
                .FirstOrDefault(u => u.UserId == user.UserId);

            if (CurrentUser == null || CurrentUser.RoleId != ROLE_USER)
            {
                return 1;
            }

            return 2;
        }

        public async Task<IActionResult> OnGetAvailableFiltersAsync()
        {
            var isAdmin = IsAdmin();

            if (isAdmin == 0)
            {
                return RedirectToPage("/Login");
            }
            else if (isAdmin == 1)
            {
                return RedirectToPage("/Index");
            }

            var months = await _context.Orders
                .Select(o => o.OrderDate.Month)
                .Distinct()
                .OrderByDescending(m => m)
                .ToListAsync();

            var years = await _context.Orders
                .Select(o => o.OrderDate.Year)
                .Distinct()
                .OrderByDescending(y => y)
                .ToListAsync();

            var filters = new
            {
                Months = months,
                Years = years
            };

            return new JsonResult(filters);
        }

        public async Task<IActionResult> OnGetGameOrderStatsAsync(int? month = null, int? year = null)
        {
            month ??= Today.Month;
            year ??= Today.Year;

            if (month < 1 || month > 12 || year < 1900 || year > Today.Year)
            {
                return BadRequest("Invalid month or year.");
            }

            var gameOrderStats = await _context.Games
                .Include(g => g.OrderDetails)
                .Where(g => g.OrderDetails.Any(od =>
                    od.Order.OrderDate.Month == month &&
                    od.Order.OrderDate.Year == year &&
                    od.Order.Status == true))
                .Select(g => new
                {
                    GameName = g.Name,
                    OrderCount = g.OrderDetails.Count(od =>
                        od.Order.OrderDate.Month == month &&
                        od.Order.OrderDate.Year == year)
                })
                .OrderByDescending(g => g.OrderCount)
                .ToListAsync();

            return new JsonResult(gameOrderStats);
        }
    }
}
