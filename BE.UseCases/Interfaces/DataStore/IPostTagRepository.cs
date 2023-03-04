using BE.Core;
using BE.UseCases.Response.PostTagResponse;

namespace BE.UseCases.Interfaces.DataStore
{
    public interface IPostTagRepository
    {
		//Task<PostTagAddResponse> PostTagAdd(PostTag postTag);
		//Task<PostTagsAddResponse> AddNewTags(List<PostTag> tags);
		Task<PostTagsDeleteResponse> RemoveOldTagsAsync(List<PostTag> tags);
		//Task<PostTagUpdateResponse> UpdateTags(List<PostTag> newTags, List<PostTag> oldTags);
	}
}
