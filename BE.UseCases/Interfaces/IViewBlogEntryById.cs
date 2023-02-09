
using BE.UseCases.Response.PostResponse;

namespace BE.UseCases.Interfaces
{
	public interface IViewBlogEntryById
	{
		Task<PostEntryResponse> ExecuteAsync(int? id);
	}
}
