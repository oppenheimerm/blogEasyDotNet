
using BE.Core;

namespace BE.UseCases.Response.PostResponse
{
	public class PostAddResponse : BaseUseCaseResponse
	{
		public Post? PostEntry { get; set; }
	}
}
