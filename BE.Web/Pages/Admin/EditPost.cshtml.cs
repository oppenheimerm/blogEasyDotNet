using BE.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BE.Web.Pages.Admin
{
    public class EditPostModel : PageModel
    {
		[BindProperty]
		public Post Post { get; set; }
		public void OnGet()
        {
        }
    }
}
