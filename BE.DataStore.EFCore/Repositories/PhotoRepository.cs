﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using BE.UseCases.Response.PhotoResponse;
using BE.UseCases.Interfaces.DataStore;

namespace BE.DataStore.EFCore.Repositories
{
    public class PhotoRepository : IPhotoRepository
	{
		public IWebHostEnvironment HostEnvironment { get; set; }

		public PhotoRepository(IWebHostEnvironment hostEnvironment)
		{
			HostEnvironment = hostEnvironment;
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
                addPhotoResponse.CoverPhotoFileName = "cover_photo_" + Guid.NewGuid().ToString() + Path.GetExtension(cover.FileName);
                string filePath = Path.Combine(uploadsFolder, addPhotoResponse.CoverPhotoFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await cover.CopyToAsync(fileStream);
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
                        File.Delete(coverPhotosFolder);
                    });
                    deletePhotoResponse.Success = true;
                    return deletePhotoResponse;
                }
                deletePhotoResponse.Success = false;
                deletePhotoResponse.ErrorMessage = $"PAth: {path} not found";
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
    }
}
