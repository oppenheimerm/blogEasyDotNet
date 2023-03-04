

using BE.UseCases.Response.PhotoResponse;

namespace BE.UseCases.Interfaces
{
	public interface IDeleteCoverPhotoFileUseCase
	{
		/// <summary>
		/// Delete cover photo.  Requires full path
		/// </summary>
		/// <param name="imagePath"></param>
		/// <returns></returns>
		Task<DeleteCoverPhotoResponse> ExecuteAsync(string imagePath);
	}
}
