
using BE.UseCases.Interfaces.DataStore;
using BE.UseCases.Interfaces;
using BE.UseCases.Response.PhotoResponse;

namespace BE.UseCases.UseCase.Image
{
    public class AddFolderUseCase : IAddFolderUseCase
    {
        private readonly IPhotoFileRepository PhotoRepository;

        public AddFolderUseCase(IPhotoFileRepository photoRepository)
        {
            PhotoRepository = photoRepository;
        }
        /// <summary>
        /// Usecase to add image folder.  Returns an instance of <see cref="AddFolderResponse"/>
        /// </summary>
        /// <param name="folderPathPrefix"></param>
        /// <returns></returns>
		public async Task<(string Foldername, DateTime TimeStamp, bool Success, string ErrorMessage)> ExecuteAsync(string folderPathPrefix)
        {
            var folder = await PhotoRepository.CreatePostImageDirectoryAsync(folderPathPrefix);
            return folder;
        }
    }
}
