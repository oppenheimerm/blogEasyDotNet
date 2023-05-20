using BE.Core;
using BE.UseCases.Response.PostResponse;

namespace BE.UseCases.Interfaces.DataStore
{
    public interface IPostsRepository
    {
        PostQueryResponse GetAllPosts();
        Task<PostEntryResponse> GetPostById(int id);
        Task<PostEntryResponse> GetPostBySlug(string slug);
        PostQueryResponse GetPostsByTag(string tagNameEncoded);
        Task<(Post? PostEntry, bool Success, string ErrorMessage)> PostAdd(Post post);
        Task<(Post, bool Success, string ErrorMessage)> PostEdit(Post post);
        Task<PostDeleteResponse> PostDelete(int? Id);

	}
}
