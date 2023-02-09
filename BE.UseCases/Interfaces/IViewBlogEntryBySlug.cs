
using BE.UseCases.Response.PostResponse;

namespace BE.UseCases.Interfaces
{
	public interface IViewBlogEntryBySlug
	{
		Task<PostEntryResponse> ExecuteAsync(string? slug);
	}
}
