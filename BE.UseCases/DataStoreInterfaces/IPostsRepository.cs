
using BE.UseCases.Response.PostResponse;

namespace BE.UseCases.DataStoreInterfaces
{
	public interface IPostsRepository
	{
		PostQueryResponse GetAllPosts();
        Task<PostEntryResponse> GetPostBySlug(string slug);
        PostQueryResponse GetPostsByTag(string tagNameEncoded);
    }
}
