
using BE.Core;
using BE.UseCases.Response.PhotoResponse;

namespace BE.UseCases.Interfaces
{
    public interface IAddFolderEntityUseCase
    {
        /// <summary>
        /// Persistance of an instance of a <see cref="ImageFolder"/> 
        /// </summary>
        /// <param name="imageFolder"></param>
        /// <returns></returns>
        Task<FolderEntityAddResponse> ExecuteAsync(ImageFolder imageFolder);
    }
}
