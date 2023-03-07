
using BE.UseCases.Interfaces;
using BE.UseCases.Interfaces.DataStore;
using BE.UseCases.Response.PostResponse;

namespace BE.UseCases.UseCase.Post
{
	public class DeletePostUseCase : IDeletePostUseCase
	{
		private readonly IPostsRepository postsRepository;

		public DeletePostUseCase(IPostsRepository postsRepository)
		{
			this.postsRepository = postsRepository;
		}

		public async Task<PostDeleteResponse> ExecuteAsync(int Id)
		{
			var status = await postsRepository.PostDelete(Id);
			return status;
		}
	}
}
