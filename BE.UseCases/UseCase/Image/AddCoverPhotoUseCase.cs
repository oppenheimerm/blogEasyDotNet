﻿using BE.UseCases.Interfaces;
using BE.UseCases.Interfaces.DataStore;
using Microsoft.AspNetCore.Http;

namespace BE.UseCases.UseCase.Image
{
    public class AddCoverPhotoUseCase : IAddCoverPhotoUseCase
    {
		private readonly IPhotoFileRepository PhotoRepository;
		public AddCoverPhotoUseCase(IPhotoFileRepository photoRepository)
        {
            PhotoRepository = photoRepository;
        }

        /// <summary>
        /// Saves an instance of <see cref="IFormFile"/> (imgage) to image folder
        /// for <see cref="Post"/>
        /// </summary>
        /// <param name="CoverImage"></param>
        /// <param name="basePath"></param>
        /// <returns></returns>
        /// <summary>
        /// Saves an instance of <see cref="IFormFile"/> (imgage) to image folder
        /// for <see cref="Post"/>
        /// </summary>
        /// <param name="CoverImage"></param>
        /// <param name="basePath"></param>
        /// <returns></returns>
        public async Task<(string FileName, bool Success, string ErrorMessage)> ExecuteAsync(IFormFile CoverImage, string basePath)
        {
            var fileName = await PhotoRepository.UploadCoverPhotoAsync(CoverImage, basePath);
            return fileName;
        }
    }
}
