using BE.Core;
using BE.UseCases.Interfaces.DataStore;
using BE.UseCases.Response.PostResponse;
using Microsoft.EntityFrameworkCore;

namespace BE.DataStore.EFCore.Repositories
{
    public class PostsRepository : IPostsRepository
	{
        private readonly BEDbContext context;

        public PostsRepository(BEDbContext context)
        {
            this.context = context;
        }

        public PostQueryResponse GetAllPosts()
        {
            PostQueryResponse postQueryResponse = new();

            try
            {
                postQueryResponse.PostsEntries = context.Posts.Include(f => f.ImageFolder)
                    .OrderByDescending(x => x.PubDate).AsNoTracking().AsQueryable();
                postQueryResponse.Success = true;
                return postQueryResponse;
            }
            catch (Exception ex)
            {
                postQueryResponse.Success = false;
                postQueryResponse.ErrorMessage = ex.Message;
                return postQueryResponse;
            }
        }

        public PostQueryResponse GetPostsByTag(string tagNameEncoded)
        {
            PostQueryResponse? postQueryResponse = new();

            try
            {
#pragma warning disable CS8619 // Nullability of reference types in value doesn't match target type.
                postQueryResponse.PostsEntries = context.PostTags.Include(x => x.Post)
                    .Include(f => f.Post.ImageFolder)
                    .Where(t => t.TagNameEncoded == tagNameEncoded)
                    .AsNoTracking()
                    .OrderBy(d => d.Post.PubDate)
                    .Select(y => y.Post)
                    .AsQueryable();
#pragma warning restore CS8619 // Nullability of reference types in value doesn't match target type.
                postQueryResponse.Success = true;
                return postQueryResponse;
            }
            catch (Exception ex)
            {
                postQueryResponse.Success = false;
                postQueryResponse.ErrorMessage = ex.Message;
                return postQueryResponse;
            }
        }

        public async Task<(Post Post, bool Success, string ErrorMessage)> GetPostBySlug(string slug)
        {

            try
            {
                var postEntryResponse = await context.Posts
                    .Include(t => t.Tags)
                    .Include(t => t.ImageFolder)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Slug == slug);


                if (postEntryResponse is not null)
                {
                    return (postEntryResponse, true, string.Empty);
                }
                else
                {
                    return (new Post(), false, $"No post foud for the slug of: {slug}");
                }

            }
            catch (Exception ex)
            {
                return (new Post(), false, ex.ToString());
            }
        }

        public async Task<(Post Post, bool Success, string ErrorMessage)> GetPostById(int id)
        {
            try
            {
                var postEntryResponse = await context.Posts
                    .Include(t => t.Tags)
                    .Include(t => t.ImageFolder).ThenInclude(i => i.Images)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(p => p.Id == id);

                if (postEntryResponse is not null)
                {
                    return (postEntryResponse, true, string.Empty);
                }
                else
                {
                    return (new Post(), false, $"No post found with the id of: {id}");
                }

            }
            catch (Exception ex)
            {
                return (new Post(), false, ex.ToString());
            }
        }

        public async Task<PostAddResponse> PostAdd(Post post)
        {
            PostAddResponse postAdd = new();

            try
            {
                context.Posts.Add(post);
                await context.SaveChangesAsync();
                postAdd.PostEntry = post;
                postAdd.Success = true;
                return postAdd;

            }
            catch (Exception ex)
            {
                postAdd.Success = false;
                postAdd.ErrorMessage = ex.Message;
                return postAdd;
            }
        }

        public async Task<PostEditResponse> PostEdit(Post post)
        {
            PostEditResponse postEdit = new();

            try
            {
                context.Update(post);
                await context.SaveChangesAsync();
                postEdit.PostEntry = post;
                postEdit.Success = true;
                return postEdit;
            }
            catch (Exception ex)
            {
                postEdit.Success = false;
                postEdit.ErrorMessage = ex.Message;
                return postEdit;
            }
        }

		public async Task<PostDeleteResponse> PostDelete(int? Id)
		{
			PostDeleteResponse postDeleteResponse = new();

			if (Id.HasValue)
			{
				var post = await context.Posts.FindAsync(Id);
				if (post != null)
				{
					context.Posts.Remove(post);
					await context.SaveChangesAsync();
					postDeleteResponse.Success = true;
					return postDeleteResponse;
				}
				else
				{
					postDeleteResponse.Success = false;
					postDeleteResponse.ErrorMessage = $"Could not find post with id: {Id.Value}";
					return postDeleteResponse;
				}
			}
			else
			{
				postDeleteResponse.Success = false;
				postDeleteResponse.ErrorMessage = "Id missing";
				return postDeleteResponse;
			}
		}
	}
}
