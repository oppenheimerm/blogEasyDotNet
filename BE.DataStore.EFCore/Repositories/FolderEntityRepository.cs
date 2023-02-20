
using BE.Core;
using BE.UseCases.Interfaces;
using BE.UseCases.Response.PhotoResponse;

namespace BE.DataStore.EFCore.Repositories
{
	public class FolderEntityRepository : IFolderEntityRepository
	{
		private readonly BEDbContext Context;
		public FolderEntityRepository(BEDbContext context)
		{
			Context = context;
		}

		public async Task<FolderEntityAddResponse> FolderEntityAdd(ImageFolder imageFolder)
		{
			FolderEntityAddResponse folderEntityAdd = new();

			try
			{
				Context.ImageFolders.Add(imageFolder);
				await Context.SaveChangesAsync();
				folderEntityAdd.Id = imageFolder.Id;
				folderEntityAdd.Name = imageFolder.Name;
				folderEntityAdd.Success = true;
				return folderEntityAdd;

			}
			catch (Exception ex)
			{
				folderEntityAdd.Success = false;
				folderEntityAdd.ErrorMessage = ex.Message;
				return folderEntityAdd;
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
			}catch(Exception ex)
			{
				folderEntityRemove.Success = false;
				folderEntityRemove.ErrorMessage = ex.Message;
				return folderEntityRemove;
			}
		}
	}
}
