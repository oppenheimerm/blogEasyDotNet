
using BE.UseCases.Response.PhotoResponse;

namespace BE.UseCases.Interfaces
{
	public interface IDeleteCoverPhotoUseCase
	{
		Task<DeletePhotoResponse> ExecuteAsync(string filename);
	}
}
