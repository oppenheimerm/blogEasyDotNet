
using BE.Core;
using BE.UseCases.Response.PhotoResponse;

namespace BE.UseCases.Interfaces
{
	public interface IAddPostImageEntityUseCase
	{
		/// <summary>
		/// Add and instance of a image entity(db recored)
		/// </summary>
		/// <param name="postImage"></param>
		/// <returns></returns>
		Task<AddPhotoEntityResponse> ExecuteAsync(PostImage postImage);
	}
}
