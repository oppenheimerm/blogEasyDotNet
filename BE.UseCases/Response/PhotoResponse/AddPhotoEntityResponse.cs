
using BE.Core;

namespace BE.UseCases.Response.PhotoResponse
{
	/// <summary>
	/// Corresponding db entity for a photo file response.
	/// </summary>
	public class AddPhotoEntityResponse : BaseUseCaseResponse
	{
		public PostImage? PostImage { get; set; }
	}
}
