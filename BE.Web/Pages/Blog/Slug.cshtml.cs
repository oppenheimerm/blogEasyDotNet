using BE.Core;
using BE.UseCases.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BE.Web.Pages.Blog
{
    public class SlugModel : PageModel
    {
        public string Host { get; private set; }
        private IViewBlogEntryBySlug? ViewBlogEntryBySlug { get; }
        public Post? Post { get; set; }
        public string ImageFolderBase { get; set; }

		public SlugModel(IViewBlogEntryBySlug? viewBlogEntryBySlug)
        {
            ViewBlogEntryBySlug = viewBlogEntryBySlug;
		}

        public async Task<ActionResult> OnGetAsync(string slug)
        {
            var post = await ViewBlogEntryBySlug.ExecuteAsync(slug);
            if (post.Post != null && post.Success == true)
            {
                Post = post.Post;
                Host = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}" + post.Post.GetLink();
                ImageFolderBase = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/";
                return Page();
            }
            else
            {
                return NotFound();
            }
        }
    }
}
