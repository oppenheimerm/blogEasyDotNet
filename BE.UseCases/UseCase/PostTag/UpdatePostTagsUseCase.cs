
using Post_Tag = BE.Core.PostTag;
using BE.UseCases.Interfaces.DataStore;
using BE.UseCases.Interfaces;
using BE.UseCases.Response.PostTagResponse;

namespace BE.UseCases.UseCase.PostTag
{
	public class UpdatePostTagsUseCase : IUpdatePostTagsUseCase
	{
		private readonly IPostTagRepository postTagRepository;
		public UpdatePostTagsUseCase(IPostTagRepository postTagRepository)
		{
			this.postTagRepository = postTagRepository;
		}

		public async Task<PostTagUpdateResponse> ExecuteAsync(List<Post_Tag> newTags, List<Post_Tag> oldTags)
		{
			var _post = await postTagRepository.UpdateTags(newTags, oldTags);
			return _post;
		}
	}
}
