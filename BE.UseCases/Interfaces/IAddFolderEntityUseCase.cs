
using BE.Core;
using BE.UseCases.Response.PhotoResponse;

namespace BE.UseCases.Interfaces
{
    public interface IAddFolderEntityUseCase
    {
        /// <summary>
        /// Usecase to add image folder(physical).  Returns an instance of <see cref="AddFolderResponse"/>
        /// </summary>
        /// <param name="folderPathPrefix"></param>
        /// <returns></returns>
        Task<(ImageFolder Folder, bool Success, string ErrorMessage)> ExecuteAsync(ImageFolder imageFolder);
    }
}
