using BE.Core;
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
		private IDeleteCoverPhotoFileUseCase DeletePhotoResponse { get; }
		private IEditPostUseCase EditPostUseCase { get; }
		private IDeleteCoverPhotoDbEntityUseCase DeleteCoverPhotoDbEntityUseCase { get; }
		public IAddPostImageEntityUseCase AddPostImageEntityUseCase { get; }

		public EditCoverPhotoModel(IViewBlogEntryById viewBlogEntryById, IDeleteCoverPhotoFileUseCase deletePhotoResponse,
			IAddCoverPhotoUseCase addCoverPhotoUseCase, IEditPostUseCase editPostUseCase,
			IDeleteCoverPhotoDbEntityUseCase deleteCoverPhotoDbEntityUseCase, IAddPostImageEntityUseCase addPostImageEntityUseCase)
		{
			ViewBlogEntryById = viewBlogEntryById;
			DeletePhotoResponse = deletePhotoResponse;
			AddCoverPhotoUseCase = addCoverPhotoUseCase;
			EditPostUseCase = editPostUseCase;
			DeleteCoverPhotoDbEntityUseCase = deleteCoverPhotoDbEntityUseCase;
			AddPostImageEntityUseCase = addPostImageEntityUseCase;
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
			if (!id.HasValue)
			{
				return NotFound();
			}

			if (Request.Form.Files == null || Request.Form.Files.Count < 1)
			{
				return NotFound();
			}

			//	verify correct file type
			var newCover = Request.Form.Files[0];
			if (ViewHelpers.ValidImageFileExtension(newCover) == false)
			{
				ModelState.AddModelError("", ".jpg, jpeg, .png files only");
				return Page();
			}

			//	Get the post
			var _postToEdit = await ViewBlogEntryById.ExecuteAsync(id.Value);
			if (_postToEdit == null)
			{
				return NotFound();
			}

			//	Get old cover photo db entitty
			var oldCoverPhotoEntity = _postToEdit.PostEntry.ImageFolder.Images.Where(i => i.IsCoverPhoto == true).FirstOrDefault();


			if (!string.IsNullOrEmpty(_postToEdit.PostEntry.PostCoverPhoto) || (oldCoverPhotoEntity != null))
			{
				//	Get cover photo path to delete the physical file
				var oldPhotoPath = ViewHelpers.GetCoverPhotoPath(_postToEdit.PostEntry.ImageFolder.Name, _postToEdit.PostEntry.PostCoverPhoto);
				var deletePhotoResponse = await DeletePhotoResponse.ExecuteAsync(oldPhotoPath);
				if (deletePhotoResponse.Success)
				{
					//	Delete the old CoverPhoto db entity(oldCoverPhotoEntity):
					var deleteOldCoverPhotoStatus = await DeleteCoverPhotoDbEntityUseCase.ExecuteAsync(oldCoverPhotoEntity);
					if (deleteOldCoverPhotoStatus.Success)
					{
						// add new photo (physical file)					
						var coverPhotoPath = Helpers.Blog.PostsImageBaseDirectory + "\\" + _postToEdit.PostEntry.ImageFolder.Name;
						var addNewCoverStatus = await AddCoverPhotoUseCase.ExecuteAsync(newCover, coverPhotoPath);
						if (addNewCoverStatus.Success)
						{
							//	add the corresponding db file
							PostImage newCoverPhoto = new PostImage()
							{
								IsCoverPhoto = true,
								FileName = addNewCoverStatus.PhotoFileName,
								TimeStamp = DateTime.Now,
								ImageFolderId = _postToEdit.PostEntry.ImageFolder.Id
							};

							//await AddPostImageEntityUseCase.ExecuteAsync(postImage);
							var addCoverPhotoEntityStatus = await AddPostImageEntityUseCase.ExecuteAsync(newCoverPhoto);

							if (addCoverPhotoEntityStatus.Success)
							{

								// update post
								_postToEdit.PostEntry.PostCoverPhoto = addNewCoverStatus.PhotoFileName;
								await EditPostUseCase.ExecuteAsync(_postToEdit.PostEntry);

								return RedirectToPage("/Admin/PostDetails", new { id = id.Value });
							}
							else
							{
								// We were unable to add the new cover photo db entity
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
						// Unable to dele the old coverPhoto
						return Page();
					}

				}
				else
				{
					// Either string was null or empty(_postToEdit.PostEntry.PostCoverPhoto)
					return Page();
				}
			}
			else
			{
				//	Could not get PostCoverPhot or OldCoverPhot was null, log it
				return Page();
			}



		}
	}
}
