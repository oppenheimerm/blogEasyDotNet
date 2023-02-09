using BE.Core;
using BE.UseCases.Response.PostResponse;

namespace BE.UseCases.Interfaces.DataStore
{
    public interface IPostsRepository
    {
        PostQueryResponse GetAllPosts();
        Task<PostEntryResponse> GetPostBySlug(string slug);
        PostQueryResponse GetPostsByTag(string tagNameEncoded);
		Task<PostAddResponse> PostAdd(Post post);
		Task<PostEditResponse> PostEdit(Post post);
		Task<PostEntryResponse> GetPostById(int id);
	}
}
