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
                postEntryResponse.Success = true;
                return postEntryResponse;
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
                postEntryResponse.Success = true;

                return postEntryResponse;
            }
            catch (Exception ex)
            {
                postEntryResponse.Success = false;
                postEntryResponse.ErrorMessage = ex.Message;
                return postEntryResponse;
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
    }
}
