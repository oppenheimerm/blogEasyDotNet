using BE.Core;
using BE.UseCases.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;

namespace BE.Web.Pages.Blog
{
    public class SlugModel : PageModel
    {
        public string Host { get; private set; }
        private IViewBlogEntryBySlug? ViewBlogEntryBySlug { get; }
        public Post? Post { get; set; }
        public string OGLogo { get; set; }

		public SlugModel(IViewBlogEntryBySlug? viewBlogEntryBySlug)
        {
            ViewBlogEntryBySlug = viewBlogEntryBySlug;
		}

        public async Task<ActionResult> OnGetAsync(string? slug)
        {
            if (!slug.IsNullOrEmpty())
            {
                var post = await ViewBlogEntryBySlug.ExecuteAsync(slug);
                if (post != null && post.Success == true)
                {
                    Post = post.PostEntry;
					Host = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}" + post.PostEntry.GetLink();
                    OGLogo = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/";
					return Page();
                }
                else
                {
                    return NotFound();
                }
            }
            else
            {
                return NotFound();
            }
        }
    }
}
