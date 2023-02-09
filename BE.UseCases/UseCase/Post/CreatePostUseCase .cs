

using _Post = BE.Core.Post;
using BE.UseCases.Interfaces.DataStore;
using BE.UseCases.Interfaces;
using BE.UseCases.Response.PostResponse;

namespace BE.UseCases.UseCase.Post
{
	public class CreatePostUseCase : ICreatePostUseCase
	{
		private readonly IPostsRepository postsRepository;

		public CreatePostUseCase(IPostsRepository postsRepository)
		{
			this.postsRepository = postsRepository;
		}

		public async Task<PostAddResponse> ExecuteAsync(_Post post)
		{
			var _post = await postsRepository.PostAdd(post);
			return _post;
		}
	}
}
