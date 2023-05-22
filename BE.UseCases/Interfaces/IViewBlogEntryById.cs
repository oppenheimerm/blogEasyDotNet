
using BE.UseCases.Response.PostResponse;

namespace BE.UseCases.Interfaces
{
	public interface IViewBlogEntryById
	{
        Task<(Core.Post, bool Success, string ErrorMessage)> ExecuteAsync(int id);
	}
}
