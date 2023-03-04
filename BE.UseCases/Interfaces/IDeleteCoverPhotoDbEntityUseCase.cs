
using BE.Core;
using BE.UseCases.Response.PhotoResponse;

namespace BE.UseCases.Interfaces
{
	public interface IDeleteCoverPhotoDbEntityUseCase
	{
		Task<DeletePhotoEntityResponse> ExecuteAsync(PostImage postImage);
	}
}
