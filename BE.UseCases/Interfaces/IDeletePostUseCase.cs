
using BE.UseCases.Response.PostResponse;

namespace BE.UseCases.Interfaces
{
	public interface IDeletePostUseCase
	{
		Task<PostDeleteResponse> ExecuteAsync(int Id);
	}
}
