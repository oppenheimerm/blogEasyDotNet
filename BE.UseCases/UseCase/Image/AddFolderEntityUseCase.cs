﻿
using BE.Core;
using BE.UseCases.Interfaces;
using BE.UseCases.Interfaces.DataStore;

namespace BE.UseCases.UseCase.Image
{
    public class AddFolderEntityUseCase : IAddFolderEntityUseCase
    {
        private readonly IFolderEntityRepository FolderEntityRepository;
        public AddFolderEntityUseCase(IFolderEntityRepository folderEntityRepository)
        {
            FolderEntityRepository = folderEntityRepository;
        }

        public async Task<(ImageFolder Folder, bool Success, string ErrorMessage)> ExecuteAsync(ImageFolder imageFolder)
        {
            var _post = await FolderEntityRepository.FolderEntityAdd(imageFolder);
            return _post;
        }
    }
}
