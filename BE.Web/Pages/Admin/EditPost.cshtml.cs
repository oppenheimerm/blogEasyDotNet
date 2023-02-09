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
		private IUpdatePostTagsUseCase UpdatePostTagsUse { get; }
		private IDeletePostTagsUseCase DeletePostTagsUseCase { get; }
		private IAddPostTagsUseCase AddPostTagsUseCase { get; }

		public EditPostModel(IEditPostUseCase editPostUseCase, IViewBlogEntryById viewBlogEntryById,
			IUpdatePostTagsUseCase updatePostTagsUseCase, IDeletePostTagsUseCase deletePostTagsUseCase,
			IAddPostTagsUseCase addPostTagsUseCase)
		{
			this.EditPostUseCase = editPostUseCase;
			ViewBlogEntryById = viewBlogEntryById;
			UpdatePostTagsUse = updatePostTagsUseCase;
			DeletePostTagsUseCase = deletePostTagsUseCase;
			AddPostTagsUseCase = addPostTagsUseCase;
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

			var oldPostTags = _postToEdit.PostEntry.Tags;
			//  As this point new tags is an unparsed string
			var ediptPostHasTags = (string.IsNullOrEmpty(Post.Tags)) ? false : true;


			// Update only the alloed properties on the Post entity
			_postToEdit.PostEntry = Post.ToPost(_postToEdit.PostEntry);
			_postToEdit.PostEntry.Title = ViewHelpers.ToTitleCase(_postToEdit.PostEntry.Title);
			_postToEdit.PostEntry.Slug = EntityHelpers.CreateSlug(_postToEdit.PostEntry.Title);

			// update post
			var postStatus = await EditPostUseCase.ExecuteAsync(_postToEdit.PostEntry);
			if (postStatus.Success)
			{
				// if editPostHasTag == true, we need to update
				if (ediptPostHasTags)
				{
					// Got old tags?
					if (oldPostTags != null && oldPostTags.Count > 0)
					{
						// update the post tags
						var postTagsToEdit = await EntityHelpers.GetPostTagsAsync(Post.Tags, id.Value);
						await UpdatePostTagsUse.ExecuteAsync(postTagsToEdit, oldPostTags.ToList());
					}
					else
					{
						// just add these new ones
						var _newTags = await EntityHelpers.GetPostTagsAsync(Post.Tags, id.Value);
						await AddPostTagsUseCase.ExecuteAsync(_newTags);

					}

				}
				else
				{
					// We need to delete the old tags
					await DeletePostTagsUseCase.ExecuteAsync(oldPostTags.ToList());
				}


				return RedirectToPage("./PostPreview");
			}
			else
			{
				return Page();
			}

		}
	}
}
