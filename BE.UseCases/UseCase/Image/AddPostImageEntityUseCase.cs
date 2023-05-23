

using BE.Core;
using BE.UseCases.Interfaces;
using BE.UseCases.Interfaces.DataStore;

namespace BE.UseCases.UseCase.Image
{
    public class AddPostImageEntityUseCase : IAddPostImageEntityUseCase
	{
		private readonly IPostImageEntityRepository PostImageRepository;
		public AddPostImageEntityUseCase(IPostImageEntityRepository postImageRepository)
		{
			PostImageRepository = postImageRepository;
		}

        public async Task<(PostImage PostImageEntity, bool Success, string ErrorMessage)> ExecuteAsync(PostImage postImage)
        {
            var photo = await PostImageRepository.AddPhotoEntityAsync(postImage);
            return photo;
        }
    }
}
