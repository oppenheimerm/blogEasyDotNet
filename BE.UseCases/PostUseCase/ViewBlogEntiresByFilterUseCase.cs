
using BE.UseCases.DataStoreInterfaces;
using BE.UseCases.Interfaces;
using BE.UseCases.Response.PostResponse;

namespace BE.UseCases.PostUseCase
{
	public class ViewBlogEntiresByFilterUseCase : IViewBlogEntiresByFilterUseCase
	{
		private readonly IPostsRepository postsRepository;

		public ViewBlogEntiresByFilterUseCase(IPostsRepository postsRepository)
		{
			this.postsRepository = postsRepository;
		}

		public PostQueryResponse Execute()
		{
			return postsRepository.GetAllPosts();
		}
	}
}
