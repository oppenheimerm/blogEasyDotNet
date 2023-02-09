
using BE.UseCases.Interfaces.DataStore;
using BE.UseCases.Interfaces;
using BE.UseCases.Response.PostTagResponse;
using Post_Tag = BE.Core.PostTag;

namespace BE.UseCases.UseCase.PostTag
{
	public class AddPostTagsUseCase : IAddPostTagsUseCase
	{
		private readonly IPostTagRepository PostTagRepository;
		public AddPostTagsUseCase(IPostTagRepository postTagRepository)
		{
			PostTagRepository = postTagRepository;
		}

		public async Task<PostTagsAddResponse> ExecuteAsync(List<Post_Tag> tags)
		{
			var response = await PostTagRepository.AddNewTags(tags);
			return response;
		}
	}
}
