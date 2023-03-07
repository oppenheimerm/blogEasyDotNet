
using BE.UseCases.Response.PhotoResponse;
using Microsoft.AspNetCore.Http;

namespace BE.UseCases.Interfaces.DataStore
{
	public interface IPhotoFileRepository
	{
		/// <summary>
		/// Upload a cover photo for an instance of a <see cref="Core.Post"/>.  Must specify full path.
		/// </summary>
		/// <param name="cover"></param>
		/// <param name="path"></param>
		/// <returns></returns>
		Task<AddPhotoResponse> UploadCoverPhotoAsync(IFormFile cover, string pathPrefix);
		/// <summary>
		/// Delete a cover photo (physical file).  Requires the full path for image
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

		/// <summary>
		/// Persistance of a photo file for post
		/// </summary>
		/// <param name="image"></param>
		/// <param name="path"></param>
		/// <returns></returns>
		Task<AddPhotoResponse> UploadPhotoAsync(IFormFile image, string path);
		/// <summary>
		/// Deletes all files associated with a particular <see cref="MO.Core.Post"/>.
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		Task<PurgePostFilesResponse> PurgePostFiles(string path);
	}
}
