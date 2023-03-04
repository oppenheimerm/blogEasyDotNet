using BE.Core;
using BE.DataStore.EFCore.Utilities;
using BE.UseCases.Interfaces;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BE.Web.Pages.Admin
{
    public class AllPostsModel : PageModel
    {
        private readonly ILogger<AllPostsModel> Logger;
        private IViewBlogEntiresByFilterUseCase? ViewBlogEntriesByFilterUseCase { get; }
        public int? PageIndex { get; set; }
        public PaginatedList<Post>? Posts { get; set; }
        private IConfiguration Configuration { get; }

        public AllPostsModel(ILogger<AllPostsModel> logger,
            IViewBlogEntiresByFilterUseCase? viewBlogEntriesByFilterUseCase,
            IConfiguration configuration)
        {
            Logger = logger;
            ViewBlogEntriesByFilterUseCase = viewBlogEntriesByFilterUseCase;
            Configuration = configuration;
        }

        public async Task OnGetAsync(int? pageIndex)
        {
            PageIndex = pageIndex;
            if (PageIndex == null || !PageIndex.HasValue)
            {
                PageIndex = 1;
            }

            var pageSize = Configuration.GetValue("pageSize", 12);
            Posts = await PaginatedList<Post>.CreateAsync(
                ViewBlogEntriesByFilterUseCase.Execute().PostsEntries
                .AsNoTracking(),
                PageIndex ?? 1, pageSize
                );
        }
    }
}
