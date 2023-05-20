
using BE.UseCases.Response.PhotoResponse;

namespace BE.UseCases.Interfaces
{
    public interface IAddFolderUseCase
    {
        /// <summary>
        /// Usecase to add image folder(physical).  Returns an instance of <see cref="AddFolderResponse"/>
        /// </summary>
        /// <param name="folderPathPrefix"></param>
        /// <returns></returns>
        Task<(string Foldername, DateTime TimeStamp, bool Success, string ErrorMessage)> ExecuteAsync(string folderPathPrefix);
    }
}
