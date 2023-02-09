

using BE.Core;
using BE.UseCases.Response.PostTagResponse;

namespace BE.UseCases.Interfaces
{
	public interface IDeletePostTagsUseCase
	{
		Task<PostTagsDeleteResponse> ExecuteAsync(List<PostTag> oldTags);
	}
}
