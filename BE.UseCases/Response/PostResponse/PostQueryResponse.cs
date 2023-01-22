

using BE.Core;

namespace BE.UseCases.Response.PostResponse
{
    public class PostQueryResponse : BaseUseCaseResponse
    {
        // Collection PostEntries to prevent collision with post plural.
        public IQueryable<Post>? PostsEntries { get; set; }
    }
}
