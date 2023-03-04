
using BE.UseCases.Interfaces.DataStore;
using BE.UseCases.Interfaces;
using BE.UseCases.Response.PhotoResponse;

namespace BE.UseCases.UseCase.Image
{
	public class DeleteCoverPhotoFileUseCase : IDeleteCoverPhotoFileUseCase
	{
		private readonly IPhotoFileRepository PhotoRepository;
		public DeleteCoverPhotoFileUseCase(IPhotoFileRepository photoRepository)
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
