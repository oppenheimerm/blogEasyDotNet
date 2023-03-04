
using BE.Core;
using BE.UseCases.Response.PhotoResponse;

namespace BE.UseCases.Interfaces.DataStore
{
	public interface IFolderEntityRepository
	{
		/// <summary>
		/// Adds an instance of a <see cref="ImageFolder"/> entity.
		/// </summary>
		/// <param name="imageFolder"></param>
		/// <returns></returns>
		Task<FolderEntityAddResponse> FolderEntityAdd(ImageFolder imageFolder);
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
		Task<FolderEntityGetResponse> GetFolderById(int id);
	}
}
