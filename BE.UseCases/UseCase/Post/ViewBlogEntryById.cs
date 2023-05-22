
using BE.UseCases.Interfaces;
using BE.UseCases.Interfaces.DataStore;

namespace BE.UseCases.UseCase.Post
{
	public class ViewBlogEntryById : IViewBlogEntryById
	{
		private readonly IPostsRepository postsRepository;

		public ViewBlogEntryById(IPostsRepository postsRepository)
		{
			this.postsRepository = postsRepository;
		}

        public async Task<(Core.Post, bool Success, string ErrorMessage)> ExecuteAsync(int id)
        {
            var postEntry = await postsRepository.GetPostById(id);
            return postEntry;
        }
    }
}
