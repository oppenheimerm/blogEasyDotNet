
using BE.UseCases.Interfaces;
using BE.UseCases.Interfaces.DataStore;
using BE.UseCases.Response.PhotoResponse;
using Microsoft.AspNetCore.Http;

namespace BE.UseCases.UseCase.Image
{
    public class AddPhotoUseCase : IAddPhotoUseCase
	{
		private readonly IPhotoFileRepository PhotoRepository;
		public AddPhotoUseCase(IPhotoFileRepository photoRepository)
		{
			PhotoRepository = photoRepository;
		}

		public async Task<AddPhotoResponse> ExecuteAsync(IFormFile image, string path)
		{
			var fileName = await PhotoRepository.UploadPhotoAsync(image, path);
			return fileName;
		}
	}
}
