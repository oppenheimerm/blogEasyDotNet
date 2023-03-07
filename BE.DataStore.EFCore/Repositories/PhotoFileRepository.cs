using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using BE.UseCases.Response.PhotoResponse;
using BE.DataStore.EFCore.Utilities;
using Microsoft.Extensions.Logging;

namespace BE.UseCases.Interfaces.DataStore
{
	public class PhotoFileRepository : IPhotoFileRepository
	{
        public IWebHostEnvironment HostEnvironment { get; set; }
		private ILogger<PhotoFileRepository> Logger;
		public PhotoFileRepository(IWebHostEnvironment hostEnvironment, ILogger<PhotoFileRepository> logger)
		{
			HostEnvironment = hostEnvironment;
			Logger = logger;
		}


		/// <summary>
		/// Upload a cover photo for an instance of <see cref="Post"/>.  Must specify full path.
		/// </summary>
		/// <param name="cover"></param>
		/// <param name="path"></param>
		/// <returns></returns>
		public async Task<AddPhotoResponse> UploadCoverPhotoAsync(IFormFile cover, string path)
		{
			AddPhotoResponse addPhotoResponse = new();

			try
			{

				//var hostPath = HostEnvironment.WebRootPath.ToString();
				var uploadsFolder = Path.Combine(HostEnvironment.WebRootPath, path);
				addPhotoResponse.PhotoFileName = "cover_photo_" + Guid.NewGuid().ToString() + Path.GetExtension(cover.FileName);
				string filePath = Path.Combine(uploadsFolder, addPhotoResponse.PhotoFileName);

				using (var fileStream = new FileStream(filePath, FileMode.Create))
				{
					await cover.CopyToAsync(fileStream);
				}

				addPhotoResponse.Success = true;
				Logger.LogInformation($"Photo {addPhotoResponse.PhotoFileName} uploaded at {DateTime.Now}");
				return addPhotoResponse;


			}
			catch (Exception ex)
			{
				addPhotoResponse.Success = false;
				addPhotoResponse.ErrorMessage = ex.Message;
				Logger.LogCritical(ex, $"Faild to upload photo at {DateTime.Now}");
				return addPhotoResponse;
			}

		}

		public async Task<AddPhotoResponse> UploadPhotoAsync(IFormFile image, string path)
		{
			AddPhotoResponse addPhotoResponse = new();

			try
			{
				var filenameClean = Path.GetFileNameWithoutExtension(image.FileName).CleanAndFormatPhotoName();//image.FileName.CleanAndFormatPhotoName();
				var uploadsFolder = Path.Combine(HostEnvironment.WebRootPath, path);
				addPhotoResponse.PhotoFileName = filenameClean + Path.GetExtension(image.FileName.ToLower());
				string filePath = Path.Combine(uploadsFolder, addPhotoResponse.PhotoFileName);

				using (var fileStream = new FileStream(filePath, FileMode.Create))
				{
					await image.CopyToAsync(fileStream);
				}

				addPhotoResponse.Success = true;
				return addPhotoResponse;


			}
			catch (Exception ex)
			{
				addPhotoResponse.Success = false;
				addPhotoResponse.ErrorMessage = ex.Message;
				return addPhotoResponse;
			}

		}

		public async Task<DeleteCoverPhotoResponse> DeleteCoverPostAsync(string path)
		{
			DeleteCoverPhotoResponse deletePhotoResponse = new();

			try
			{
				var hostPath = HostEnvironment.WebRootPath.ToString();
				var coverPhotosFolder = Path.Combine(HostEnvironment.WebRootPath, path);

				var exist = File.Exists(coverPhotosFolder);
				if (exist)
				{
					//System.IO.File.Delete(filePath);
					await Task.Run(() =>
					{
						System.IO.File.Delete(coverPhotosFolder);
					});
					deletePhotoResponse.Success = true;
					return deletePhotoResponse;
				}
				deletePhotoResponse.Success = false;
				deletePhotoResponse.ErrorMessage = $"Path: {coverPhotosFolder} not found";
				return deletePhotoResponse;
			}
			catch (Exception Ex)
			{
				deletePhotoResponse.Success = false;
				deletePhotoResponse.ErrorMessage = Ex.Message;
				return deletePhotoResponse;
			}
		}

		/// <summary>
		/// Create a folder directory for <see cref="Core.Post"/>
		/// </summary>
		/// <param name="pathPrefix"></param>
		/// <returns></returns>
		public async Task<AddFolderResponse> CreatePostImageDirectoryAsync(string pathPrefix)
		{
			AddFolderResponse addFolderResponse = new();
			try
			{
				var folderName = Guid.NewGuid().ToString("N");
				// pathPrefix = img\\posts\\foldername
				var path = pathPrefix + "\\" + folderName;

				var folder = Path.Combine(HostEnvironment.WebRootPath, path);
				if (!Directory.Exists(folder))
				{
					await Task.Run(() => {
						Directory.CreateDirectory(folder);

					});
					addFolderResponse.TimeStamp = Directory.GetCreationTime(folder);
					addFolderResponse.Success = true;
					addFolderResponse.FolderName = folderName;
					return addFolderResponse;
				}
				else
				{
					addFolderResponse.Success = false;
					addFolderResponse.ErrorMessage = "Folder already exist";
					return addFolderResponse;
				}
			}
			catch (Exception Ex)
			{
				addFolderResponse.Success = false;
				addFolderResponse.ErrorMessage = Ex.Message;
				return addFolderResponse;
			}
		}

		public async Task<PurgePostFilesResponse> PurgePostFiles(string path)
		{
			//	Path should be in the format: img\\posts\\foldername
			PurgePostFilesResponse purgePostFilesResponse = new();

			var folder = Path.Combine(HostEnvironment.WebRootPath, path);
			if (Directory.Exists(folder))
			{
				try
				{
					DirectoryInfo directoryInfo = new DirectoryInfo(folder);

					var deleteFiles = await Task.Run(() => Parallel.ForEach(directoryInfo.EnumerateFiles(), f =>
					{
						f.Delete();
					}));

					if (deleteFiles.IsCompleted == true)
					{

						//	Delete this directory
						if (Directory.Exists(folder))
						{
							directoryInfo.Delete();
						}
					}

					purgePostFilesResponse.Success = true;
					Logger.LogInformation($"Sucessfully deleted folder: {folder}. Timestamp {DateTime.Now}");
					return purgePostFilesResponse;


				}
				catch (Exception Ex)
				{
					purgePostFilesResponse.Success = false;
					purgePostFilesResponse.ErrorMessage = Ex.Message;
					Logger.LogError(Ex, $"Failed to delete folder: {folder}. Timestamp: {DateTime.Now}");
					return purgePostFilesResponse;
				}
			}
			else
			{
				purgePostFilesResponse.Success = false;
				purgePostFilesResponse.ErrorMessage = $"Could not find path: {path}";
				Logger.LogError($"Failed to delete folder: {folder}. Could not find path. Timestamp: {DateTime.UtcNow.ToLongTimeString}");
				return purgePostFilesResponse;
			}
		}
	}


}
