using BE.UseCases.Response.PhotoResponse;
using Microsoft.AspNetCore.Http;

namespace BE.UseCases.Interfaces.DataStore
{
    public interface IPhotoRepository
    {
        /// <summary>
        /// Upload a cover photo for an instance of a <see cref="Core.Post"/>.  Must specify full path.
        /// </summary>
        /// <param name="cover"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        Task<AddPhotoResponse> UploadCoverPhotoAsync(IFormFile cover, string pathPrefix);
		/// <summary>
		/// Delete a cover photo.  Requires the full path for image
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		Task<DeleteCoverPhotoResponse> DeleteCoverPostAsync(string path);
		/// <summary>
		/// Create a folder directory for <see cref="Core.Post"/>
		/// </summary>
		/// <param name="pathPrefix"></param>
		/// <returns></returns>
		Task<AddFolderResponse> CreatePostImageDirectoryAsync(string pathPrefix);
    }
}
