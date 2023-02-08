using BE.Core;
using BE.DataStore.EFCore.Utilities;
using Microsoft.AspNetCore.Mvc;



namespace BE.Web.ViewComponents
{
    //  This is the code part of the BlogGridViewComponent.It makes use of
    //  the ViewPostsUseCase via dependency injection system to resolve
    //  the implementation of IViewPostsUseCase which provides dependency
    //  injection of IPostRepository and is injected into the constructor
    //  of the view component class. The InvokeAsync method obtains a
    //  List<Post> from the service and passes it to the view.
    [ViewComponent]
	public class BlogGridViewComponent : ViewComponent
	{
		private readonly ILogger<BlogGridViewComponent>? _logger;
		public PaginatedList<Post>? Posts { get; set; }
		public int? PageIndex { get; set; }

		public BlogGridViewComponent(ILogger<BlogGridViewComponent> logger)
		{
			_logger = logger;
		}

		public IViewComponentResult Invoke(
			int? pageIndex,
			PaginatedList<Post> posts
			)
		{
			PageIndex = pageIndex;
			Posts = posts;

			return View(Posts);
		}
	}
}
