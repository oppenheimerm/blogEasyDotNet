

using BE.Core;
using BE.UseCases.Response.PostResponse;

namespace BE.UseCases.Interfaces
{
	public interface IEditPostUseCase
	{
		Task<PostEditResponse> ExecuteAsync(Post post);
	}
}
