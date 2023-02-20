
using BE.Core;
using BE.UseCases.Response.PhotoResponse;

namespace BE.UseCases.Interfaces
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
	}
}
