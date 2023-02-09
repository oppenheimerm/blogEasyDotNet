using BE.UseCases.Interfaces;
using BE.UseCases.Interfaces.DataStore;
using BE.UseCases.Response.PostResponse;

namespace BE.UseCases.UseCase.Post
{
    public class ViewBlogEntryBySlug : IViewBlogEntryBySlug
    {
        private readonly IPostsRepository postsRepository;

        public ViewBlogEntryBySlug(IPostsRepository postsRepository)
        {
            this.postsRepository = postsRepository;
        }

        public async Task<PostEntryResponse> ExecuteAsync(string? slug)
        {
            var postEntry = await postsRepository.GetPostBySlug(slug);
            return postEntry;
        }
    }
}
