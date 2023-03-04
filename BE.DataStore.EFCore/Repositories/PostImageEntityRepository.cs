
using BE.Core;
using BE.UseCases.Interfaces.DataStore;
using BE.UseCases.Response.PhotoResponse;

namespace BE.DataStore.EFCore.Repositories
{
	public class PostImageEntityRepository : IPostImageEntityRepository
	{
		private readonly BEDbContext Context;

		public PostImageEntityRepository(BEDbContext context)
		{
			Context = context;
		}

		public async Task<AddPhotoEntityResponse> AddPhotoEntityAsync(PostImage ImageEntity)
		{
			AddPhotoEntityResponse addPhotoEntityResponse = new();


			try
			{
				Context.PostImages.Add(ImageEntity);
				await Context.SaveChangesAsync();
				addPhotoEntityResponse.PostImage = ImageEntity;
				addPhotoEntityResponse.Success = true;
				return addPhotoEntityResponse;

			}
			catch (Exception ex)
			{
				addPhotoEntityResponse.Success = false;
				addPhotoEntityResponse.ErrorMessage = ex.Message;
				return addPhotoEntityResponse;
			}
		}

		public async Task<DeletePhotoEntityResponse> DeletePhotoEntityAsync(PostImage ImageEntity)
		{
			DeletePhotoEntityResponse deletePhotoEntityResponse = new();

			try
			{
				Context.PostImages.Remove(ImageEntity);
				await Context.SaveChangesAsync();
				deletePhotoEntityResponse.Success = true;
				return deletePhotoEntityResponse;
			}
			catch (Exception ex)
			{
				deletePhotoEntityResponse.Success = false;
				return deletePhotoEntityResponse;
			}
		}
	}
}
