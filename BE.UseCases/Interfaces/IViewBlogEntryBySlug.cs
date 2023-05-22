
using BE.UseCases.Response.PostResponse;

namespace BE.UseCases.Interfaces
{
	public interface IViewBlogEntryBySlug
	{
        Task<(Core.Post Post, bool Success, string ErrorMessage)> ExecuteAsync(string slug);
	}
}
