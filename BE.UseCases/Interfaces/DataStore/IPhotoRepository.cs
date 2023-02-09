using BE.UseCases.Response.PhotoResponse;
using Microsoft.AspNetCore.Http;

namespace BE.UseCases.Interfaces.DataStore
{
    public interface IPhotoRepository
    {
        Task<AddPhotoResponse> UploadCoverPhotoAsync(IFormFile cover);
    }
}
