
namespace BE.UseCases.Response.PhotoResponse
{
	public class FolderEntityAddResponse : BaseUseCaseResponse
	{
		public int Id { get; set; }
		public string Name { get; set; } = string.Empty;
	}
}
