
using BE.Core;

namespace BE.UseCases.Response.PostResponse
{
	public class PostEditResponse : BaseUseCaseResponse
	{
		public Post? PostEntry { get; set; }
	}
}
