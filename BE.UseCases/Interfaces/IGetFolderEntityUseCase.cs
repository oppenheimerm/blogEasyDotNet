
using BE.UseCases.Response.PhotoResponse;

namespace BE.UseCases.Interfaces
{
	public interface IGetFolderEntityUseCase
	{
		Task<FolderEntityGetResponse> ExecuteAsync(int id);
	}
}
