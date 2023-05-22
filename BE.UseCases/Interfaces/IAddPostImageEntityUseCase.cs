
using BE.Core;

namespace BE.UseCases.Interfaces
{
	public interface IAddPostImageEntityUseCase
	{
        /// <summary>
        /// Add and instance of a image entity(db recored)
        /// </summary>
        /// <param name="postImage"></param>
        /// <returns></returns>
        Task<(PostImage PostImageEntity, bool Success, string ErrorMessage)> ExecuteAsync(PostImage postImage);
	}
}
