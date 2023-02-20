
using BE.UseCases.Interfaces;
using BE.UseCases.Interfaces.DataStore;
using BE.UseCases.Response.PhotoResponse;

namespace BE.UseCases.UseCase.Image
{
	public class DeleteCoverPhotoUseCase : IDeleteCoverPhotoUseCase
	{
		private readonly IPhotoRepository PhotoRepository;
		public DeleteCoverPhotoUseCase(IPhotoRepository photoRepository)
		{
			PhotoRepository = photoRepository;
		}

		public async Task<DeleteCoverPhotoResponse> ExecuteAsync(string imagePath)
		{
			var fileName = await PhotoRepository.DeleteCoverPostAsync(imagePath);
			return fileName;
		}
	}
}
