using BE.Core;
using BE.UseCases.Interfaces.DataStore;
using BE.UseCases.Response.PostResponse;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BE.DataStore.EFCore.Repositories
{
    public class PostsRepository : IPostsRepository
	{
        private readonly BEDbContext context;
        private readonly ILogger<PostsRepository> Logger;

        public PostsRepository(BEDbContext context, ILogger<PostsRepository> logger)
        {
            this.context = context;
            Logger = logger;
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

        public async Task<PostEntryResponse> GetPostBySlug(string slug)
        {
            PostEntryResponse postEntryResponse = new();

            try
            {
                postEntryResponse.PostEntry = await context.Posts
                    .Include(t => t.Tags)
                    .Include(t => t.ImageFolder)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Slug == slug);

                if (postEntryResponse.PostEntry != null)
                {
                    postEntryResponse.Success = true;
                    return postEntryResponse;
                }
                else
                {
                    postEntryResponse.Success = false;
                    return postEntryResponse;
                }
            }
            catch (Exception ex)
            {
                postEntryResponse.Success = false;
                postEntryResponse.ErrorMessage = ex.Message;
                return postEntryResponse;
            }
        }

        public async Task<PostEntryResponse> GetPostById(int id)
        {
            PostEntryResponse postEntryResponse = new();

            try
            {
                postEntryResponse.PostEntry = await context.Posts
                    .Include(t => t.Tags)
                    .Include(t => t.ImageFolder).ThenInclude(i => i.Images)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(p => p.Id == id);


                if (postEntryResponse.PostEntry != null)
                {
                    postEntryResponse.Success = true;
                    return postEntryResponse;
                }
                else
                {
                    postEntryResponse.Success = false;
                    return postEntryResponse;
                }

            }
            catch (Exception ex)
            {
                postEntryResponse.Success = false;
                postEntryResponse.ErrorMessage = ex.Message;
                return postEntryResponse;
            }
        }

        public async Task<(Post? PostEntry, bool Success, string ErrorMessage)> PostAdd(Post post)
        {
            try
            {
                context.Posts.Add(post);
                await context.SaveChangesAsync();
                Logger.LogInformation($"Post with Id: {post.Id}, added to database at: {DateTime.UtcNow}");
                return (post, true, string.Empty);

            }
            catch (Exception ex)
            {
                Logger.LogError($"Failed to add post to database. Timestamp : {DateTime.UtcNow}");
                return (post, false, ex.ToString());
            }
        }

        public async Task<(Post, bool Success, string ErrorMessage)> PostEdit(Post post)
        {

            try
            {
                context.Update(post);
                await context.SaveChangesAsync();
                Logger.LogInformation($"Post with Id: {post.Id}, was edited successfully at: {DateTime.UtcNow}");
                return (post, true, string.Empty);
            }
            catch (Exception ex)
            {
                Logger.LogError($"Unable to edit post with Id: {post.Id}. Timestamp was at: {DateTime.UtcNow}");
                return (new Post(), false, ex.ToString());
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
