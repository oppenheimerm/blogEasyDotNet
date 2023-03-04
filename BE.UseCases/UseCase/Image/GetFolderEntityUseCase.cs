
using BE.UseCases.Interfaces;
using BE.UseCases.Interfaces.DataStore;
using BE.UseCases.Response.PhotoResponse;

namespace BE.UseCases.UseCase.Image
{
	public class GetFolderEntityUseCase : IGetFolderEntityUseCase
	{
		private readonly IFolderEntityRepository FolderEntityRepository;
		public GetFolderEntityUseCase(IFolderEntityRepository folderEntityRepository)
		{
			FolderEntityRepository = folderEntityRepository;
		}

		public async Task<FolderEntityGetResponse> ExecuteAsync(int id)
		{
			var folder = await FolderEntityRepository.GetFolderById(id);
			return folder;
		}
	}
}
