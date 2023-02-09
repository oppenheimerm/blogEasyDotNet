
using BE.UseCases.Response.PhotoResponse;
using Microsoft.AspNetCore.Http;

namespace BE.UseCases.Interfaces
{
	public interface IAddCoverPhotoUseCase
	{
		Task<AddPhotoResponse> ExecuteAsync(IFormFile CoverImage);
	}
}
