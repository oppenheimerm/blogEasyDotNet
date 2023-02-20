
using BE.UseCases.Response.PhotoResponse;
using Microsoft.AspNetCore.Http;

namespace BE.UseCases.Interfaces
{
	public interface IAddCoverPhotoUseCase
	{
        /// <summary>
        /// Saves an instance of <see cref="IFormFile"/> (imgage) to image folder
        /// for <see cref="Post"/>
        /// </summary>
        /// <param name="CoverImage"></param>
        /// <param name="basePath"></param>
        /// <returns></returns>
        Task<AddPhotoResponse> ExecuteAsync(IFormFile CoverImage, string basePath);

    }
}
