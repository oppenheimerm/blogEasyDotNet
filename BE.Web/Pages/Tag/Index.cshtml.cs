using BE.Core;
using BE.DataStore.EFCore.Utilities;
using BE.UseCases.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BE.Web.Pages.Tag
{
    public class IndexModel : PageModel
    {
		public string Tag { get; set; }
		public int? PageIndex { get; set; }
		public IViewBlogEntriesByTag ViewBlogEntriesByTag { get; }
		public ILogger<IndexModel> Logger { get; }
		public IConfiguration Configuration { get; }
		public PaginatedList<Post> Posts { get; set; }

		public IndexModel(
			IViewBlogEntriesByTag viewBlogEntriesByTag,
			ILogger<IndexModel> logger,
			IConfiguration configuration
			)
		{
			ViewBlogEntriesByTag = viewBlogEntriesByTag;
			Logger = logger;
			Configuration = configuration;
		}
		public async Task OnGet(string tag, int? pageIndex)
		{
			Tag = tag;
			PageIndex = pageIndex;

			await GetDataAsync(tag);
		}

		private async Task GetDataAsync(string tageNameEncoded)
		{
			if (PageIndex == null || !PageIndex.HasValue)
			{
				PageIndex = 1;
			}

			{
				var pageSize = Configuration.GetValue("pageSize", 12);
				Posts = await PaginatedList<Post>.CreateAsync(
					ViewBlogEntriesByTag.ExecuteAsync(tageNameEncoded).PostsEntries.AsNoTracking(),
					PageIndex ?? 1, pageSize
					);
			}
		}
	}
}
