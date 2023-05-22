
using BE.Core;
using BE.UseCases.Response.PhotoResponse;

namespace BE.UseCases.Interfaces
{
	public interface IGetFolderEntityUseCase
	{
        Task<(ImageFolder FolderEntity, bool Success, string ErrorMessage)> ExecuteAsync(int id);
	}
}
