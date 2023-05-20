

using BE.Core;

namespace BE.UseCases.Interfaces
{
	public interface IEditPostUseCase
	{
        Task<(Post Post, bool success, string ErrorMessage)> ExecuteAsync(Post post);

    }
}
