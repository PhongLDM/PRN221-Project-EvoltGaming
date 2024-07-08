using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EvoltingStore.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace EvoltingStore.Pages.Management
{
    public class EditBlogModel : PageModel
    {
        private readonly EvoltingStoreContext _context;

        public EditBlogModel(EvoltingStoreContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Blog Blog { get; set; }
        public List<Genre> Genres { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Blog = await _context.Blogs.FindAsync(id);

            if (Blog == null)
            {
                return NotFound();
            }

            Genres = await _context.Genres.ToListAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            Genres = await _context.Genres.ToListAsync();

            if (!ValidateBlog(Blog))
            {
                return Page();
            }

            var blogToUpdate = await _context.Blogs.FindAsync(id);

            if (blogToUpdate == null)
            {
                return NotFound();
            }

            blogToUpdate.Title = Blog.Title;
            blogToUpdate.Content = Blog.Content;
            blogToUpdate.GenreId = Blog.GenreId;

            await _context.SaveChangesAsync();

            return RedirectToPage("/BlogDetail", new { id = blogToUpdate.Id });
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
