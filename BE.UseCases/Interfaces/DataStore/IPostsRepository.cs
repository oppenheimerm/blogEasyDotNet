using BE.Core;
using BE.UseCases.Response.PostResponse;

namespace BE.UseCases.Interfaces.DataStore
{
    public interface IPostsRepository
    {
        PostQueryResponse GetAllPosts();
        Task<(Post Post, bool Success, string ErrorMessage)> GetPostById(int id);
        Task<(Post Post, bool Success, string ErrorMessage)> GetPostBySlug(string slug);
        PostQueryResponse GetPostsByTag(string tagNameEncoded);
        Task<PostAddResponse> PostAdd(Post post);
        Task<PostEditResponse> PostEdit(Post post);
        Task<PostDeleteResponse> PostDelete(int? Id);

	}
}
