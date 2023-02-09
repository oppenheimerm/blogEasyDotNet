
using BE.Core;
using BE.UseCases.Response.PostResponse;

namespace BE.UseCases.Interfaces
{
	public interface ICreatePostUseCase
	{
		Task<PostAddResponse> ExecuteAsync(Post post);
	}	
}
