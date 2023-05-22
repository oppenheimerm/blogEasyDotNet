
using BE.Core;
using BE.UseCases.Interfaces;
using BE.UseCases.Interfaces.DataStore;

namespace BE.UseCases.UseCase.Image
{
	public class GetFolderEntityUseCase : IGetFolderEntityUseCase
	{
		private readonly IFolderEntityRepository FolderEntityRepository;
		public GetFolderEntityUseCase(IFolderEntityRepository folderEntityRepository)
		{
			FolderEntityRepository = folderEntityRepository;
		}

        public async Task<(ImageFolder FolderEntity, bool Success, string ErrorMessage)> ExecuteAsync(int id)
        {
            var folder = await FolderEntityRepository.GetFolderById(id);
            return folder;
        }
    }
}
