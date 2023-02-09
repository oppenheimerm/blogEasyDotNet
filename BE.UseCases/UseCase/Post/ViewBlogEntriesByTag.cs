using BE.UseCases.Interfaces;
using BE.UseCases.Interfaces.DataStore;
using BE.UseCases.Response.PostResponse;


namespace BE.UseCases.UseCase.Post
{
	public class ViewBlogEntriesByTag : IViewBlogEntriesByTag
	{
		private readonly IPostsRepository postsRepository;

		public ViewBlogEntriesByTag(IPostsRepository postsRepository)
		{
			this.postsRepository = postsRepository;
		}

		public PostQueryResponse ExecuteAsync(string tagNameEncoded)
		{
			var postEntries = postsRepository.GetPostsByTag(tagNameEncoded);
			return postEntries;
		}
	}
}
