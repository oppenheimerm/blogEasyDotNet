

using BE.Core;
using BE.UseCases.Interfaces;
using BE.UseCases.Interfaces.DataStore;
using BE.UseCases.Response.PhotoResponse;

namespace BE.UseCases.UseCase.Image
{
    public class AddPostImageEntityUseCase : IAddPostImageEntityUseCase
	{
		private readonly IPostImageEntityRepository PostImageRepository;
		public AddPostImageEntityUseCase(IPostImageEntityRepository postImageRepository)
		{
			PostImageRepository = postImageRepository;
		}

		public async Task<AddPhotoEntityResponse> ExecuteAsync(PostImage postImage)
		{
			var photo = await PostImageRepository.AddPhotoEntityAsync(postImage);
			return photo;
		}
	}
}
