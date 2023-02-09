
using Post_Tag = BE.Core.PostTag;
using BE.UseCases.Interfaces.DataStore;
using BE.UseCases.Interfaces;
using BE.UseCases.Response.PostTagResponse;

namespace BE.UseCases.UseCase.PostTag
{
	public class DeletePostTagsUseCase : IDeletePostTagsUseCase
	{
		private readonly IPostTagRepository postTagRepository;

		public DeletePostTagsUseCase(IPostTagRepository postTagRepository)
		{
			this.postTagRepository = postTagRepository;
		}

		public async Task<PostTagsDeleteResponse> ExecuteAsync(List<Post_Tag> oldTags)
		{
			var response = await postTagRepository.RemoveOldTagsAsync(oldTags);
			return response;
		}
	}
}
