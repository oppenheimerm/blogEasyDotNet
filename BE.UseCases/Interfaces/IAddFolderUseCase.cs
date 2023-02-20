
using BE.UseCases.Response.PhotoResponse;

namespace BE.UseCases.Interfaces
{
    public interface IAddFolderUseCase
    {
        /// <summary>
        /// Usecase to add image folder.  Returns an instance of <see cref="AddFolderResponse"/>
        /// </summary>
        /// <param name="folderPathPrefix"></param>
        /// <returns></returns>
        Task<AddFolderResponse> ExecuteAsync(string folderPathPrefix);
    }
}
