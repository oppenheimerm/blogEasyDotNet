
using BE.Core;
using BE.UseCases.Response.PostTagResponse;

namespace BE.UseCases.Interfaces
{
	public interface IUpdatePostTagsUseCase
	{
		Task<PostTagUpdateResponse> ExecuteAsync(List<PostTag> newTags, List<PostTag> oldTags);
	}
}
