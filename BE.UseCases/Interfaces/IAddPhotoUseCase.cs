using BE.UseCases.Response.PhotoResponse;
using Microsoft.AspNetCore.Http;

namespace BE.UseCases.Interfaces
{
    public interface IAddPhotoUseCase
    {
        /// <summary>
        /// Persist image file for post
        /// </summary>
        /// <param name="image"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        Task<(string FileName, bool Success, string ErrorMessage)> ExecuteAsync(IFormFile image, string path);
    }
}
