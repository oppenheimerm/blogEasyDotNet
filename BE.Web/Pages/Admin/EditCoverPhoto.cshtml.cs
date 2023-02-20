using BE.UseCases.Interfaces;
using BE.Web.Helpers;
using BE.Web.Models.VM;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BE.Web.Pages.Admin
{
    public class EditCoverPhotoModel : PageModel
    {
		public EditCoverPhotoVM EditCoverPhotoVM { get; set; }
		public IAddCoverPhotoUseCase AddCoverPhotoUseCase { get; }
		private IViewBlogEntryById ViewBlogEntryById { get; }
		private IDeleteCoverPhotoUseCase DeletePhotoResponse { get; set; }
		private IEditPostUseCase EditPostUseCase { get; }

		public EditCoverPhotoModel(IViewBlogEntryById viewBlogEntryById, IDeleteCoverPhotoUseCase deletePhotoResponse,
			IAddCoverPhotoUseCase addCoverPhotoUseCase, IEditPostUseCase editPostUseCase)
		{
			ViewBlogEntryById = viewBlogEntryById;
			DeletePhotoResponse = deletePhotoResponse;
			AddCoverPhotoUseCase = addCoverPhotoUseCase;
			EditPostUseCase = editPostUseCase;
		}

		public async Task<IActionResult> OnGetAsync(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}


			var response = await ViewBlogEntryById.ExecuteAsync(id);


			if (response.Success)
			{
				EditCoverPhotoVM = new EditCoverPhotoVM()
				{
					PostId = response.PostEntry.Id,
					CoverPhoto = response.PostEntry.PostCoverPhoto,
					PostTitle = response.PostEntry.ToPostVM().Title,
					PostFolderName = response.PostEntry.ImageFolder.Name
				};

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

			if (Request.Form.Files == null || Request.Form.Files.Count < 1)
			{
				return NotFound();
			}

			//	Get the post
			var _postToEdit = await ViewBlogEntryById.ExecuteAsync(id.Value);
			if (_postToEdit == null)
			{
				return NotFound();
			}

			var newCover = Request.Form.Files[0];

			if (!string.IsNullOrEmpty(_postToEdit.PostEntry.PostCoverPhoto))
			{
				// delete old cover photo
				var oldPhotoPath = ViewHelpers.GetCoverPhotoPath(_postToEdit.PostEntry.ImageFolder.Name, _postToEdit.PostEntry.PostCoverPhoto);
				var deletePhotoResponse = await DeletePhotoResponse.ExecuteAsync(oldPhotoPath);
				if (deletePhotoResponse.Success)
				{
					// add new photo					
					var coverPhotoPath = Helpers.Blog.PostsImageBaseDirectory + "\\" + _postToEdit.PostEntry.ImageFolder.Name;

					var addNewCoverStatus = await AddCoverPhotoUseCase.ExecuteAsync(newCover, coverPhotoPath);
					if (addNewCoverStatus.Success)
					{
						// update post
						_postToEdit.PostEntry.PostCoverPhoto = addNewCoverStatus.CoverPhotoFileName;
						await EditPostUseCase.ExecuteAsync(_postToEdit.PostEntry);

						return RedirectToPage("/Admin/PostDetails", new { id = id.Value });
					}
					else
					{
						return Page();
					}
				}
				else
				{
					return Page();
				}
			}
			else
			{
				return Page();
			}



		}
	}
}
