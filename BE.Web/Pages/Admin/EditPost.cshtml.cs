using BE.Core;
using BE.Core.Utilities;
using BE.UseCases.Interfaces;
using BE.Web.Helpers;
using BE.Web.Models.VM;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace BE.Web.Pages.Admin
{
	public class EditPostModel : PageModel
	{
		[BindProperty]
		public EditPostVM? Post { get; set; }
		private IEditPostUseCase EditPostUseCase { get; }
		private IViewBlogEntryById ViewBlogEntryById { get; }
		private IDeletePostTagsUseCase DeletePostTagsUseCase { get; }
		public bool HasImages { get; set; } = false;
		public List<PostImage>? PostImages { get; set; }
		public string? FolderBasePath { get; set; }
        private readonly ILogger<EditPostModel> Logger;
        public EditPostModel(IEditPostUseCase editPostUseCase, IViewBlogEntryById viewBlogEntryById,
			IDeletePostTagsUseCase deletePostTagsUseCase, ILogger<EditPostModel> logger)
        {
			this.EditPostUseCase = editPostUseCase;
			ViewBlogEntryById = viewBlogEntryById;
			DeletePostTagsUseCase = deletePostTagsUseCase;
            Logger = logger;
		}

        public async Task<IActionResult> OnGetAsync(int id)
        {
            //  When you don't have to include related data, FindAsync is more efficient.
            var response = await ViewBlogEntryById.ExecuteAsync(id);
            Post = response.Item1.ToPostVM();
            if (response.Success == true)
            {
                HasImages = true;
                PostImages = response.Item1?.ImageFolder?.Images?.ToList();
                FolderBasePath = ViewHelpers.GetPostImageBaseUrl(response.Item1?.ImageFolder?.Name ?? string.Empty);
            }


            if (response.Success)
            {
                return Page();
            }
            else
            {
                return NotFound();
            }
;
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {

            // prevent overposting
            var _postToEdit = await ViewBlogEntryById.ExecuteAsync(id);
            if (_postToEdit.Item1 == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (Post is not null)
            {
                // Update only the alloewd properties on the Post entity
                var postUpdate = Post.ToPost(_postToEdit.Item1);
                postUpdate.Title = ViewHelpers.ToTitleCase(postUpdate.Title);
                postUpdate.Slug = EntityHelpers.CreateSlug(postUpdate.Title);
                postUpdate.LastModified = DateTime.Now;

                // update post
                var postStatus = await EditPostUseCase.ExecuteAsync(postUpdate);
                if (postStatus.Success)
                {
                    ICollection<PostTag>? tagsToDelete = postUpdate.Tags;
                    List<PostTag>? tagsToAdd = new List<PostTag>();

                    // New tags if any
                    if (!string.IsNullOrEmpty(Post.Tags))
                    {
                        tagsToAdd = await EntityHelpers.GetPostTagsAsync(Post.Tags, postUpdate.Id);
                        postUpdate.Tags = tagsToAdd;

                        var addNewTagsStatus = await EditPostUseCase.ExecuteAsync(postUpdate);
                        if (addNewTagsStatus.Success)
                        {
                            // delete old tags and return post preview
                            //var deleteOldTagsStatus = await DeletePostTagsUseCase.ExecuteAsync(tagsToDelete.ToList());
                            return RedirectToPage("/Admin/PostPreview", new { id = postUpdate.Id });
                        }
                        else
                        {
                            // could not add new tags, log it, return page
                            return Page();
                        }
                    }
                    else
                    {
                        //  no tags associated with this post, so delete old ones
                        // delete old tags and return post preview
                        await DeletePostTagsUseCase.ExecuteAsync(tagsToDelete.ToList());
                        return RedirectToPage("/Admin/PostPreview", new { id = postUpdate.Id });
                    }

                }
                else
                {
                    return Page();
                }

            }
            else
            {
                Logger.LogError($"Failed to edit post at: {DateTime.UtcNow}");
                return StatusCode(500);
            }
        }
    }
}
