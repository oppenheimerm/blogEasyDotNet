
using BE.UseCases.DataStoreInterfaces;
using BE.UseCases.Interfaces;
using BE.UseCases.Response.PostResponse;

namespace BE.UseCases.PostUseCase
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
