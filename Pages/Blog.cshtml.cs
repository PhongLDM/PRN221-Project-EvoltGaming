using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using EvoltingStore.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EvoltingStore.Pages
{
    public class BlogModel : PageModel
    {
        private readonly EvoltingStoreContext _context;

        public BlogModel(ILogger<BlogModel> logger, EvoltingStoreContext context)
        {
            _context = context;
        }

        public List<Blog> Blogs { get; set; }
        public int PageIndex { get; set; }
        public int TotalPages { get; set; }

        public async Task OnGetAsync(int pageIndex = 1)
        {
            int pageSize = 6;
            var query = _context.Blogs.Include(b => b.Genre).OrderByDescending(b => b.Id).AsNoTracking();
            var count = await query.CountAsync();
            TotalPages = (int)System.Math.Ceiling(count / (double)pageSize);
            Blogs = await query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            PageIndex = pageIndex;
        }
    }
}
