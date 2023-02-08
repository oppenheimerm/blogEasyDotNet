using BE.UseCases.Response.PostResponse;


namespace BE.UseCases.Interfaces
{
	public interface IViewBlogEntiresByFilterUseCase
	{
		PostQueryResponse Execute();
	}
}
