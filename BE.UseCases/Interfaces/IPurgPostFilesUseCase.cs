
using BE.UseCases.Response.PhotoResponse;

namespace BE.UseCases.Interfaces
{
	public interface IPurgPostFilesUseCase
	{
		/// <summary>
		/// Delete all image files associated with a <see cref="BE.Core.Post"/> instance.
		/// </summary>
		/// <param name="folderPath"></param>
		/// <returns></returns>
		Task<PurgePostFilesResponse> ExecuteAsync(string folderPath);
	}
}
