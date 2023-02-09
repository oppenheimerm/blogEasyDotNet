

using BE.Core;

namespace BE.UseCases.Response.PostTagResponse
{
	public class PostTagsAddResponse : BaseUseCaseResponse
	{
		public List<PostTag> PostTags { get; set; }
	}
}
