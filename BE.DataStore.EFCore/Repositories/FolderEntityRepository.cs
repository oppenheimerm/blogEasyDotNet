
using BE.Core;
using BE.UseCases.Interfaces;
using BE.UseCases.Interfaces.DataStore;
using BE.UseCases.Response.PhotoResponse;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BE.DataStore.EFCore.Repositories
{
	public class FolderEntityRepository : IFolderEntityRepository
	{
		private readonly BEDbContext Context;
        private ILogger<FolderEntityRepository> Logger;
        public FolderEntityRepository(BEDbContext context,
            ILogger<FolderEntityRepository> logger)
		{
			Context = context;
			Logger = logger;
		}

        public async Task<(ImageFolder Folder, bool Success, string ErrorMessage)> FolderEntityAdd(ImageFolder imageFolder)
        {
            try
            {
                Context.ImageFolders.Add(imageFolder);
                await Context.SaveChangesAsync();
                Logger.LogInformation($"Folder entity with Id: {imageFolder.Id}, added to database at: {DateTime.UtcNow}");
                return (imageFolder, true, string.Empty);

            }
            catch (Exception ex)
            {
                Logger.LogError($"Failed to add folder entitiy to database at: {DateTime.UtcNow}");
                return (new ImageFolder(), false, ex.ToString());
            }
        }

		public async Task<FolderEntityRemoveResponse> FolderEntityDelete(ImageFolder imageFolder)
		{
			FolderEntityRemoveResponse folderEntityRemove = new();

			try
			{
				Context.ImageFolders.Remove(imageFolder);
				await Context.SaveChangesAsync();

				folderEntityRemove.Success = true;
				return folderEntityRemove;
			}
			catch (Exception ex)
			{
				folderEntityRemove.Success = false;
				folderEntityRemove.ErrorMessage = ex.Message;
				return folderEntityRemove;
			}
		}

		public async Task<FolderEntityGetResponse> GetFolderById(int id)
		{
			FolderEntityGetResponse folderEntityGetResponse = new();

			try
			{
				folderEntityGetResponse.Folder = await Context.ImageFolders
					.Include(i => i.Images)
					.AsNoTracking()
					.FirstOrDefaultAsync(f => f.Id == id);
				folderEntityGetResponse.Success = true;
				return folderEntityGetResponse;

			}
			catch
			{
				folderEntityGetResponse.Success = false;
				return folderEntityGetResponse;
			}
		}
	}
}
