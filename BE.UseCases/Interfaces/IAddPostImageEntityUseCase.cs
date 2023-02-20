
using BE.Core;
using BE.UseCases.Response.PhotoResponse;

namespace BE.UseCases.Interfaces
{
	public interface IAddPostImageEntityUseCase
	{
		Task<PostImageEntityAddResponse> ExecuteAsync(PostImage postImage);
	}
}
