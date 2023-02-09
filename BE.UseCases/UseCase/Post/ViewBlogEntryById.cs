
using BE.UseCases.Interfaces;
using BE.UseCases.Interfaces.DataStore;
using BE.UseCases.Response.PostResponse;

namespace BE.UseCases.UseCase.Post
{
	public class ViewBlogEntryById : IViewBlogEntryById
	{
		private readonly IPostsRepository postsRepository;

		public ViewBlogEntryById(IPostsRepository postsRepository)
		{
			this.postsRepository = postsRepository;
		}

		public async Task<PostEntryResponse> ExecuteAsync(int? id)
		{
			if (!id.HasValue)
			{
				PostEntryResponse postEntryReposnes = new();
				postEntryReposnes.Success = false;
				postEntryReposnes.ErrorMessage = "Id required";
				return postEntryReposnes;

			}
			else
			{
				var postEntry = await postsRepository.GetPostById(id.Value);
				return postEntry;
			}
		}
	}
}
