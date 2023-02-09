using BE.UseCases.Interfaces;
using BE.UseCases.Interfaces.DataStore;
using BE.UseCases.Response.PhotoResponse;
using Microsoft.AspNetCore.Http;

namespace BE.UseCases.UseCase.Image
{
    public class AddCoverPhotoUseCase : IAddCoverPhotoUseCase
    {
        private readonly IPhotoRepository PhotoRepository;
        public AddCoverPhotoUseCase(IPhotoRepository photoRepository)
        {
            PhotoRepository = photoRepository;
        }

        public async Task<AddPhotoResponse> ExecuteAsync(IFormFile CoverImage)
        {
            var fileName = await PhotoRepository.UploadCoverPhotoAsync(CoverImage);
            return fileName;
        }
    }
}
