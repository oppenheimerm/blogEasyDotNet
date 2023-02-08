

using BE.UseCases.Response.PhotoResponse;
using Microsoft.AspNetCore.Http;

namespace BE.UseCases.DataStoreInterfaces
{
	public interface IPhotoRepository
	{
		Task<AddPhotoResponse> UploadCoverPhotoAsync(IFormFile cover);
	}
}
