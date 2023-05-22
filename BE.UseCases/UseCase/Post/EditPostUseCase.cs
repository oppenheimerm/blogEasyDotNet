

using _Post = BE.Core.Post;
using BE.UseCases.Interfaces.DataStore;
using BE.UseCases.Response.PostResponse;
using BE.UseCases.Interfaces;

namespace BE.UseCases.UseCase.Post
{
	public class EditPostUseCase : IEditPostUseCase
	{
		private readonly IPostsRepository postsRepository;

		public EditPostUseCase(IPostsRepository postsRepository)
		{
			this.postsRepository = postsRepository;
		}

		public async Task<(_Post Post, bool Success, string ErrorMessage)> ExecuteAsync(_Post post)
        {
			var _post = await postsRepository.PostEdit(post);
			return _post;
		}
	}
}
