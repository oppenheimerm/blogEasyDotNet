

using BE.Core;

namespace BE.UseCases.Interfaces
{
	public interface IEditPostUseCase
	{
        Task<(Post Post, bool Success, string ErrorMessage)> ExecuteAsync(Post post);

    }
}
