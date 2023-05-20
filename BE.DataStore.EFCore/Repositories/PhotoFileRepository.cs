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
        public async Task<(string FileName, bool Success, string ErrorMessage)> UploadCoverPhotoAsync(IFormFile cover, string path)
        {
            try
            {

                //var hostPath = HostEnvironment.WebRootPath.ToString();
                var uploadsFolder = Path.Combine(HostEnvironment.WebRootPath, path);
                var photoFileName = "cover_photo_" + Guid.NewGuid().ToString() + Path.GetExtension(cover.FileName);
                string filePath = Path.Combine(uploadsFolder, photoFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await cover.CopyToAsync(fileStream);
                }
                Logger.LogInformation($"Photo {photoFileName} uploaded at {DateTime.Now}");
                return (photoFileName, true, string.Empty);


            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Faild to upload photo at {DateTime.Now}");
                return (string.Empty, false, ex.ToString());
            }

        }

        public async Task<(string FileName, bool Success, string ErrorMessage)> UploadPhotoAsync(IFormFile image, string path)
        {
            try
            {
                var filenameClean = Path.GetFileNameWithoutExtension(image.FileName).CleanAndFormatPhotoName();//image.FileName.CleanAndFormatPhotoName();
                var uploadsFolder = Path.Combine(HostEnvironment.WebRootPath, path);
                var photoFileName = filenameClean + Path.GetExtension(image.FileName.ToLower());
                string filePath = Path.Combine(uploadsFolder, photoFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await image.CopyToAsync(fileStream);
                }

                Logger.LogInformation($"Photo {photoFileName} uploaded at {DateTime.Now} to {uploadsFolder} at: {DateTime.UtcNow}");
                return (photoFileName, true, string.Empty);

            }
            catch (Exception ex)
            {
                Logger.LogError($"Failed to upload photo at: {DateTime.UtcNow}");
                return (string.Empty, false, ex.ToString());
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
        public async Task<(string FolderName, DateTime Timestamp, bool Success, string ErrorMessage)> CreatePostImageDirectoryAsync(string pathPrefix)
        {
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
                    Logger.LogInformation($"Folder: {folderName} created sucessfully at: {DateTime.UtcNow}");
                    return (folderName, Directory.GetCreationTime(folder), true, string.Empty);
                }
                else
                {
                    return (string.Empty, DateTime.Now, false, "Folder already exist.");
                }
            }
            catch (Exception Ex)
            {
                Logger.LogError($" Failed to create folder at: {DateTime.UtcNow}");
                return (string.Empty, DateTime.Now, false, Ex.Message);
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
