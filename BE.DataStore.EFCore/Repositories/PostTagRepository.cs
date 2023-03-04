
using BE.Core;
using BE.UseCases.Interfaces.DataStore;
using BE.UseCases.Response.PostTagResponse;

namespace BE.DataStore.EFCore.Repositories
{
    public class PostTagRepository : IPostTagRepository
	{
		private readonly BEDbContext Context;

		public PostTagRepository(BEDbContext context)
		{
			Context = context;
		}

		/*public async Task<PostTagAddResponse> PostTagAdd(PostTag postTag)
		{
			PostTagAddResponse postTagAdd = new();

			try
			{
				Context.PostTags.Add(postTag);
				await Context.SaveChangesAsync();
				postTagAdd.PostTag = postTag;
				postTagAdd.Success = true;
				return postTagAdd;

			}
			catch (Exception ex)
			{
				postTagAdd.Success = false;
				postTagAdd.ErrorMessage = ex.Message;
				return postTagAdd;
			}
		}*/

		public async Task<PostTagsDeleteResponse> RemoveOldTagsAsync(List<PostTag> tags)
		{
			PostTagsDeleteResponse postTagsDeleteResponse = new();

			try
			{
				_ = await Task.Run(() => Parallel.ForEach(tags, t =>
				{
					Context.PostTags.Remove(t);
				}));
				await Context.SaveChangesAsync();

				postTagsDeleteResponse.Success = true;
				return postTagsDeleteResponse;
			}
			catch (Exception ex)
			{
				postTagsDeleteResponse.Success = false;
				postTagsDeleteResponse.ErrorMessage = ex.Message;
				return postTagsDeleteResponse;
			}
		}

		/*public async Task<PostTagsAddResponse> AddNewTags(List<PostTag> tags)
		{
			PostTagsAddResponse postTagsAddResponsepostTagsAddResponse = new();

			try
			{
				_ = await Task.Run(() => Parallel.ForEach(tags, async t =>
				{
					await Context.PostTags.AddRangeAsync(tags);
				}));

				await Context.SaveChangesAsync();
				postTagsAddResponsepostTagsAddResponse.Success = true;
				postTagsAddResponsepostTagsAddResponse.PostTags = tags;
				return postTagsAddResponsepostTagsAddResponse;
			}
			catch (Exception ex)
			{
				postTagsAddResponsepostTagsAddResponse.Success = false;
				postTagsAddResponsepostTagsAddResponse.ErrorMessage = ex.Message;
				return postTagsAddResponsepostTagsAddResponse;
			}

		}*/

		/*public async Task<PostTagUpdateResponse> UpdateTags(List<PostTag> newTags, List<PostTag> oldTags)
		{
			PostTagUpdateResponse postTagUpdateResponse = new();


			try
			{
				//  Delete OldTags
				if (oldTags.Any())
				{
					await RemoveOldTagsAsync(oldTags);
				}
				//	Add new tags if any
				if (newTags.Any())
				{
					await AddNewTags(newTags);
				}


				postTagUpdateResponse.PostTags = newTags;
				postTagUpdateResponse.Success = true;
				return postTagUpdateResponse;

			}
			catch (Exception ex)
			{
				postTagUpdateResponse.Success = false;
				postTagUpdateResponse.ErrorMessage = ex.Message;
				return postTagUpdateResponse;
			}
		}*/
	}
}
