
using BE.UseCases.Response.PostResponse;

namespace BE.UseCases.Interfaces
{
	public interface IViewBlogEntriesByTag
	{
		PostQueryResponse ExecuteAsync(string? tagNameEncoded);

	}
}
