
using BE.Core;
using BE.UseCases.Response.PostResponse;

namespace BE.UseCases.Interfaces
{
	public interface ICreatePostUseCase
	{
		Task<(Post? PostEntry, bool Success, string ErrorMessage)> ExecuteAsync(Post post);

    }	
}
