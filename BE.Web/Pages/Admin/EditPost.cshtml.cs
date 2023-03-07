using BE.Core;
using BE.Core.Utilities;
using BE.UseCases.Interfaces;
using BE.Web.Helpers;
using BE.Web.Models.VM;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

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
		public EditPostModel(IEditPostUseCase editPostUseCase, IViewBlogEntryById viewBlogEntryById,
			IDeletePostTagsUseCase deletePostTagsUseCase)
		{
			this.EditPostUseCase = editPostUseCase;
			ViewBlogEntryById = viewBlogEntryById;
			DeletePostTagsUseCase = deletePostTagsUseCase;
		}

		public async Task<IActionResult> OnGetAsync(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			//  When you don't have to include related data, FindAsync is more efficient.
			var response = await ViewBlogEntryById.ExecuteAsync(id);
			Post = response.PostEntry.ToPostVM();
			if (response.Success == true && response.PostEntry.ImageFolder != null)
			{
				HasImages = true;
				PostImages = response.PostEntry.ImageFolder.Images.ToList();
				FolderBasePath = ViewHelpers.GetPostImageBaseUrl(response.PostEntry.ImageFolder.Name);
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

		public async Task<IActionResult> OnPostAsync(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			if (!ModelState.IsValid)
			{
				return Page();
			}

			// prevent overposting
			var _postToEdit = await ViewBlogEntryById.ExecuteAsync(id.Value);
			if (_postToEdit == null)
			{
				return NotFound();
			}

			//var oldPostTags = _postToEdit.PostEntry.Tags;
			//  As this point new tags is an unparsed string
			//var ediptPostHasTags = (string.IsNullOrEmpty(Post.Tags)) ? false : true;


			// Update only the alloed properties on the Post entity
			_postToEdit.PostEntry = Post.ToPost(_postToEdit.PostEntry);
			_postToEdit.PostEntry.Title = ViewHelpers.ToTitleCase(_postToEdit.PostEntry.Title);
			_postToEdit.PostEntry.Slug = EntityHelpers.CreateSlug(_postToEdit.PostEntry.Title);
			_postToEdit.PostEntry.LastModified = DateTime.Now;

			// update post
			var postStatus = await EditPostUseCase.ExecuteAsync(_postToEdit.PostEntry);
			if (postStatus.Success)
			{
				ICollection<PostTag>? tagsToDelete = _postToEdit.PostEntry.Tags;
				List<PostTag>? tagsToAdd = new List<PostTag>();

				// New tags if any
				if (!string.IsNullOrEmpty(Post.Tags) && postStatus.PostEntry != null)
				{
					tagsToAdd = await EntityHelpers.GetPostTagsAsync(Post.Tags, id.Value);
					postStatus.PostEntry.Tags = tagsToAdd;

					var addNewTagsStatus = await EditPostUseCase.ExecuteAsync(postStatus.PostEntry);
					if (addNewTagsStatus.Success)
					{
						// delete old tags and return post preview
						//var deleteOldTagsStatus = await DeletePostTagsUseCase.ExecuteAsync(tagsToDelete.ToList());
						return RedirectToPage("/Admin/PostPreview", new { id = postStatus.PostEntry.Id });
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
					return RedirectToPage("/Admin/PostPreview", new { id = postStatus.PostEntry.Id });
				}

			}
			else
			{
				return Page();
			}

		}
	}
}
