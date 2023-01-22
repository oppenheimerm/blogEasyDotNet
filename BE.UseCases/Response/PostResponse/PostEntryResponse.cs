
using BE.Core;

namespace BE.UseCases.Response.PostResponse
{
    public class PostEntryResponse : BaseUseCaseResponse
    {
        public Post? PostEntry { get; set; }
    }
}
