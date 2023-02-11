using BE.Core;
using Microsoft.AspNetCore.Mvc;

namespace BE.Web.ViewComponents
{
	public class PostViewerViewComponent : ViewComponent
	{
		private readonly ILogger<PostViewerViewComponent>? Logger;

		public Post? Post { get; set; }

		public PostViewerViewComponent(ILogger<PostViewerViewComponent>? logger)
		{
			Logger = logger;
		}

		public IViewComponentResult Invoke(Post post)
		{
			Post = post;

			return View(Post);
		}
	}
}
