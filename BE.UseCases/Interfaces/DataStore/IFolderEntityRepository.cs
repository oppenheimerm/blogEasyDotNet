
using BE.Core;
using BE.UseCases.Response.PhotoResponse;

namespace BE.UseCases.Interfaces.DataStore
{
	public interface IFolderEntityRepository
	{
        /// <summary>
        /// Return and tupe of <see cref="ImageFolder"/>
        /// </summary>
        /// <param name="imageFolder"></param>
        /// <returns></returns>
        Task<(ImageFolder Folder, bool Success, string ErrorMessage)> FolderEntityAdd(ImageFolder imageFolder);
        /// <summary>
        /// Removes an instance of a <see cref="ImageFolder"/> entity.
        /// </summary>
        /// <param name="imageFolder"></param>
        /// <returns></returns>
        Task<FolderEntityRemoveResponse> FolderEntityDelete(ImageFolder imageFolder);
        /// <summary>
        /// Get a <see cref="ImageFolder"/> by Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<(ImageFolder FolderEntity, bool Success, string ErrorMessage)> GetFolderById(int id);
	}
}
