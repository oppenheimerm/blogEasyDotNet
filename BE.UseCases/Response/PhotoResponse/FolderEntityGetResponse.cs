
using BE.Core;

namespace BE.UseCases.Response.PhotoResponse
{
	public class FolderEntityGetResponse : BaseUseCaseResponse
	{
		public ImageFolder Folder { get; set; }
	}
}
