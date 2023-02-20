
namespace BE.UseCases.Response.PhotoResponse
{
	public class AddFolderResponse : BaseUseCaseResponse
	{
		public DateTime? TimeStamp { get; set; }
		public string? FolderName { get; set; }
	}
}
