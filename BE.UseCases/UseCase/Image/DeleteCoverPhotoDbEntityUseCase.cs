
using BE.Core;
using BE.UseCases.Interfaces;
using BE.UseCases.Interfaces.DataStore;
using BE.UseCases.Response.PhotoResponse;

namespace BE.UseCases.UseCase.Image
{
	public class DeleteCoverPhotoDbEntityUseCase : IDeleteCoverPhotoDbEntityUseCase
	{
		private readonly IPostImageEntityRepository PostImageEntityRepository;

		public DeleteCoverPhotoDbEntityUseCase(IPostImageEntityRepository postImageEntityRepository)
		{
			PostImageEntityRepository = postImageEntityRepository;
		}

		public async Task<DeletePhotoEntityResponse> ExecuteAsync(PostImage postImage)
		{
			var fileName = await PostImageEntityRepository.DeletePhotoEntityAsync(postImage);
			return fileName;
		}
	}
}
