
using BE.Core;
using BE.UseCases.Response.PhotoResponse;

namespace BE.UseCases.Interfaces.DataStore
{
	public interface IPostImageEntityRepository
	{
		/// <summary>
		/// Persistence of a <see cref="PostImage"/> Db entity of a image file for <see cref="Post"/>
		/// </summary>
		/// <param name="ImageEntity"></param>
		/// <returns></returns>
		Task<AddPhotoEntityResponse> AddPhotoEntityAsync(PostImage ImageEntity);
		/// <summary>
		/// Remove a <see cref="PostImage"/> db entity
		/// </summary>
		/// <param name="ImageEntity"></param>
		/// <returns></returns>
		Task<DeletePhotoEntityResponse> DeletePhotoEntityAsync(PostImage ImageEntity);
	}
}
