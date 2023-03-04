using BE.UseCases.Interfaces;
using BE.Web.Helpers;
using BE.Web.Models.VM;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BE.Web.Pages.Admin
{
    public class DeleteCoverPhotoModel : PageModel
    {
		private IViewBlogEntryById ViewBlogEntryById { get; }
		public DeleteCoverPhotoVM? DeleteCoverPhotoVM { get; set; }
		private IDeleteCoverPhotoFileUseCase DeletePhotoResponse { get; }
		private IDeleteCoverPhotoDbEntityUseCase DeleteCoverPhotoDbEntityUseCase { get; }
		private IEditPostUseCase EditPostUseCase { get; }

		public DeleteCoverPhotoModel(IViewBlogEntryById viewBlogEntryById, IDeleteCoverPhotoFileUseCase deletePhotoResponse,
			IDeleteCoverPhotoDbEntityUseCase deleteCoverPhotoDbEntityUseCase, IEditPostUseCase editPostUseCase)
		{
			ViewBlogEntryById = viewBlogEntryById;
			DeletePhotoResponse = deletePhotoResponse;
			DeleteCoverPhotoDbEntityUseCase = deleteCoverPhotoDbEntityUseCase;
			EditPostUseCase = editPostUseCase;
		}

		public async Task<IActionResult> OnGetAsync(int? id)
		{
			if (!id.HasValue)
			{
				return NotFound();
			}

			var response = await ViewBlogEntryById.ExecuteAsync(id);

			if (response.Success)
			{
				DeleteCoverPhotoVM = new DeleteCoverPhotoVM()
				{
					PostId = response.PostEntry.Id,
					PostTitle = response.PostEntry.Title,
					CoverPhotoUrl = ViewHelpers.GetPostCoverImage(response.PostEntry.ImageFolder.Name, response.PostEntry.PostCoverPhoto)

				};

				return Page();
			}
			else
			{
				return NotFound();
			}
		}

		public async Task<IActionResult> OnPostAsync(int? id)
		{
			if (id.HasValue)
			{
				//	get the post
				var getPostByIdStatus = await ViewBlogEntryById.ExecuteAsync(id);
				if (getPostByIdStatus.Success)
				{
					//	Get old cover photo db entitty
					var oldCoverPhotoEntity = getPostByIdStatus.PostEntry.ImageFolder.Images.Where(i => i.IsCoverPhoto == true).FirstOrDefault();
					if (!string.IsNullOrEmpty(getPostByIdStatus.PostEntry.PostCoverPhoto) || (oldCoverPhotoEntity != null))
					{
						var oldPhotoPath = ViewHelpers.GetCoverPhotoPath(getPostByIdStatus.PostEntry.ImageFolder.Name, getPostByIdStatus.PostEntry.PostCoverPhoto);
						var deleteOldCoverPhotoResponse = await DeletePhotoResponse.ExecuteAsync(oldPhotoPath);
						if (deleteOldCoverPhotoResponse.Success)
						{
							//	Delete the old CoverPhoto db entity(oldCoverPhotoEntity):
							var deleteOldCoverPhotoStatus = await DeleteCoverPhotoDbEntityUseCase.ExecuteAsync(oldCoverPhotoEntity);
							if (deleteOldCoverPhotoStatus.Success)
							{
								//	update post
								getPostByIdStatus.PostEntry.PostCoverPhoto = string.Empty;
								getPostByIdStatus.PostEntry.LastModified = DateTime.UtcNow;
								await EditPostUseCase.ExecuteAsync(getPostByIdStatus.PostEntry);
							}
							else
							{
								//	unable to delete old cover phote db entity, log it, return page
								return Page();
							}

							return RedirectToPage("/Admin/PostPreview", new { id = getPostByIdStatus.PostEntry.Id });
						}
						else
						{
							//	unable to delete old cover photo, log it, return page
							return Page();
						}

					}
					else
					{
						//	Could not get PostCoverPhot or OldCoverPhot was null, log it
						return Page();
					}

				}
				else
				{
					//	Could not find post, log it
					return NotFound();
				}
			}
			else
			{
				return NotFound();
			}
		}
	}
}
