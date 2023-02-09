
using BE.Core;

namespace BE.UseCases.Response.PostTagResponse
{
	public class PostTagUpdateResponse : BaseUseCaseResponse
	{
		public IEnumerable<PostTag>? PostTags { get; set; }
	}
}
