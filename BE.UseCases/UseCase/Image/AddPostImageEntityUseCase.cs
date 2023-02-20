

using BE.Core;
using BE.UseCases.Interfaces;
using BE.UseCases.Interfaces.DataStore;
using BE.UseCases.Response.PhotoResponse;

namespace BE.UseCases.UseCase.Image
{
    public class AddPostImageEntityUseCase : IAddPostImageEntityUseCase
	{
		private readonly IPostImageRepository PostImageRepository;
		public AddPostImageEntityUseCase(IPostImageRepository postImageRepository)
		{
			PostImageRepository = postImageRepository;
		}

		public async Task<PostImageEntityAddResponse> ExecuteAsync(PostImage postImage)
		{
			var photo = await PostImageRepository.FolderEntityAdd(postImage);
			return photo;
		}
	}
}
