
using BE.UseCases.Interfaces.DataStore;
using BE.UseCases.Interfaces;
using BE.UseCases.Response.PostResponse;

namespace BE.UseCases.UseCase.Post
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
