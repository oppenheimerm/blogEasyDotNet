
using BE.Core;
using BE.UseCases.Response.PostTagResponse;

namespace BE.UseCases.Interfaces
{
	public interface IAddPostTagsUseCase
	{
		Task<PostTagsAddResponse> ExecuteAsync(List<PostTag> tags);
	}
}
