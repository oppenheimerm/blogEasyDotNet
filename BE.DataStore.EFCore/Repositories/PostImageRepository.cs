
using BE.Core;
using BE.UseCases.Interfaces.DataStore;
using BE.UseCases.Response.PhotoResponse;

namespace BE.DataStore.EFCore.Repositories
{
	public class PostImageRepository : IPostImageRepository
	{
		private readonly BEDbContext Context;

		public PostImageRepository(BEDbContext context)
		{
			Context = context;
		}

		public async Task<PostImageEntityAddResponse> FolderEntityAdd(PostImage postImage)
		{
			PostImageEntityAddResponse postImageEntityAddResponse = new();

			try
			{
				Context.PostImages.Add(postImage);
				await Context.SaveChangesAsync();
				postImageEntityAddResponse.PostImage = postImage;
				postImageEntityAddResponse.Success = true;
				return postImageEntityAddResponse;

			}
			catch (Exception ex)
			{
				postImageEntityAddResponse.Success = false;
				postImageEntityAddResponse.ErrorMessage = ex.Message;
				return postImageEntityAddResponse;
			}
		}
	}
}
