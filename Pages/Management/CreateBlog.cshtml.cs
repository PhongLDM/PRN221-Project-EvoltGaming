using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using EvoltingStore.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EvoltingStore.Pages.Management
{
    public class CreateBlogModel : PageModel
    {
        private readonly EvoltingStoreContext _context;

        public CreateBlogModel(EvoltingStoreContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Blog Blog { get; set; }

        public List<Genre> Genres { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Genres = await _context.Genres.ToListAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Genres = await _context.Genres.ToListAsync();

            if (!ValidateBlog(Blog))
            {
                return Page();
            }

            Blog.CreateDate = DateTime.Now;

            _context.Blogs.Add(Blog);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Blog");
        }

        private bool ValidateBlog(Blog blog)
        {
            bool isValid = true;

            if (string.IsNullOrEmpty(blog.Title))
            {
                ModelState.AddModelError("Blog.Title", "The Title field is required.");
                isValid = false;
            }

            if (string.IsNullOrEmpty(blog.Content))
            {
                ModelState.AddModelError("Blog.Content", "The Content field is required.");
                isValid = false;
            }

            if (blog.GenreId == 0)
            {
                ModelState.AddModelError("Blog.GenreId", "The Genre field is required.");
                isValid = false;
            }

            return isValid;
        }
    }
}
