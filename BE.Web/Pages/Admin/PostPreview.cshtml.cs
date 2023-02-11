using BE.Core;
using BE.UseCases.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BE.Web.Pages.Admin
{
    public class PostPreviewModel : PageModel
    {
		private IViewBlogEntryById ViewBlogEntryById { get; }
		public Post Post { get; set; }

		public PostPreviewModel(IViewBlogEntryById viewBlogEntryById)
		{
			ViewBlogEntryById= viewBlogEntryById;
		}

		public async Task<IActionResult> OnGetAsync(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			//  When you don't have to include related data, FindAsync is more efficient.
			var response = await ViewBlogEntryById.ExecuteAsync(id);
			Post = response.PostEntry;

			if (response.Success)
			{
				return Page();
			}
			else
			{
				return NotFound();
			};
		}
	}
}
