using BE.UseCases.Interfaces.DataStore;
using BE.UseCases.Interfaces;
using BE.UseCases.Response.PhotoResponse;

namespace BE.UseCases.UseCase.Image
{
	public class PurgPostFilesUseCase : IPurgPostFilesUseCase
	{
		private IPhotoFileRepository PhotoFileRepository { get; }
		public PurgPostFilesUseCase(IPhotoFileRepository photoFileRepository)
		{
			PhotoFileRepository = photoFileRepository;
		}

		public async Task<PurgePostFilesResponse> ExecuteAsync(string folderPath)
		{
			var status = await PhotoFileRepository.PurgePostFiles(folderPath);
			return status;
		}
	}
}
