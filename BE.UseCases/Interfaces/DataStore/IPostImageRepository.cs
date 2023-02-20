
using BE.Core;
using BE.UseCases.Response.PhotoResponse;

namespace BE.UseCases.Interfaces.DataStore
{
	public interface IPostImageRepository
	{
		/// <summary>
		/// Add a <see cref="PostImage"/> database entity to datastore
		/// </summary>
		/// <param name="postImage"></param>
		/// <returns></returns>
		Task<PostImageEntityAddResponse> FolderEntityAdd(PostImage postImage);
	}
}
