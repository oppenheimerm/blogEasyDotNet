using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using BE.UseCases.Response.PhotoResponse;
using BE.UseCases.Interfaces.DataStore;

namespace BE.DataStore.EFCore.Repositories
{
    public class PhotoRepository : IPhotoRepository
	{
		public IWebHostEnvironment HostEnvironment { get; set; }
		public PhotoRepository(IWebHostEnvironment hostEnvironment)
		{
			HostEnvironment = hostEnvironment;
		}


		public async Task<AddPhotoResponse> UploadCoverPhotoAsync(IFormFile cover)
		{
			AddPhotoResponse addPhotoResponse = new();

			try
			{

				var hostPath = HostEnvironment.WebRootPath.ToString();

				var uploadsFolder = Path.Combine(HostEnvironment.WebRootPath, "img\\posts\\cover");
				addPhotoResponse.CoverPhotoFileName = Guid.NewGuid().ToString() + "_" + cover.FileName;
				string filePath = Path.Combine(uploadsFolder, addPhotoResponse.CoverPhotoFileName);

				using (var fileStream = new FileStream(filePath, FileMode.Create))
				{
					await cover.CopyToAsync(fileStream);
				}

				addPhotoResponse.Success = true;
				return addPhotoResponse;


			}
			catch (Exception ex)
			{
				addPhotoResponse.Success = false;
				addPhotoResponse.ErrorMessage = ex.Message;
				return addPhotoResponse;
			}

		}
	}
}
