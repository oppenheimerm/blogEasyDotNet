
using BE.Core;
using BE.UseCases.Interfaces.DataStore;
using BE.UseCases.Response.PhotoResponse;
using Microsoft.Extensions.Logging;

namespace BE.DataStore.EFCore.Repositories
{
	public class PostImageEntityRepository : IPostImageEntityRepository
	{
		private readonly BEDbContext Context;
        private readonly ILogger<PostImageEntityRepository> Logger;

        public PostImageEntityRepository(BEDbContext context, ILogger<PostImageEntityRepository> logger)
        {
            Context = context;
            Logger = logger;
        }

        public async Task<(PostImage PostImageEntity, bool Success, string ErrorMessage)> AddPhotoEntityAsync(PostImage ImageEntity)
        {
            try
            {
                Context.PostImages.Add(ImageEntity);
                await Context.SaveChangesAsync();
                Logger.LogInformation($"PostImageEntity with Id: {ImageEntity.Id}, added to database at: {DateTime.UtcNow}");
                return (ImageEntity, true, string.Empty);

            }
            catch (Exception ex)
            {
                Logger.LogInformation($"Faild to add PostImageEntity to the database at: {DateTime.UtcNow}");
                return (new PostImage(), false, ex.ToString());
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
