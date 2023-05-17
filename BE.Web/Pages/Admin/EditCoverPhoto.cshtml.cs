using BE.Core;
using BE.UseCases.Interfaces;
using BE.UseCases.UseCase.Image;
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
		private IAddFolderEntityUseCase AddFolderEntityUseCase { get; }
		private IAddFolderUseCase AddFolderUseCase { get; }

		public EditCoverPhotoModel(IViewBlogEntryById viewBlogEntryById, IDeleteCoverPhotoFileUseCase deletePhotoResponse,
			IAddCoverPhotoUseCase addCoverPhotoUseCase, IEditPostUseCase editPostUseCase,
			IDeleteCoverPhotoDbEntityUseCase deleteCoverPhotoDbEntityUseCase, IAddPostImageEntityUseCase addPostImageEntityUseCase,
			IAddFolderEntityUseCase addFolderEntityUseCase, IAddFolderUseCase addFolderUseCase)
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


			if (response.Success && response.PostEntry != null)
			{
				EditCoverPhotoVM = response.PostEntry.ToToEditCoverPhotoVM();
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

			var _postToEdit = await ViewBlogEntryById.ExecuteAsync(id.Value);

			if (Request.Form.Files == null || Request.Form.Files.Count < 1)
			{
				EditCoverPhotoVM = _postToEdit.PostEntry.ToToEditCoverPhotoVM();
				ModelState.AddModelError("", "Please add image to upload");
				return Page();
			}

			//	verify correct file type
			var newCover = Request.Form.Files[0];
			if (!FileHelpers.ValidImageFile(newCover))
			{
				EditCoverPhotoVM = _postToEdit.PostEntry.ToToEditCoverPhotoVM();
				ModelState.AddModelError("", ".jpg, jpeg, .png files only");
				return Page();
			}

			if (_postToEdit == null)
			{
				return NotFound();
			}


			//	Make sure that we have a valid coverphotoe and image folder
			if (!string.IsNullOrEmpty(_postToEdit.PostEntry.PostCoverPhoto) && _postToEdit.PostEntry.ImageFolder != null)
			{
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
									_postToEdit.PostEntry.LastModified = DateTime.Now;
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
			else
			{
				//	We need to create a new image folder(physical) for post and add this new cover photo
				//	Add new folder
				var addFolderStatus = await AddFolderUseCase.ExecuteAsync(Helpers.Blog.PostsImageBaseDirectory);
				if (addFolderStatus.Success)
				{
					//	Add new folder db entity
					ImageFolder imgFolder = new ImageFolder()
					{
						PostId = _postToEdit.PostEntry.Id,
						LastUpdated = DateTime.Now,
						TimeStamp = addFolderStatus.TimeStamp.Value,
						Name = addFolderStatus.FolderName
					};
					var addFolderEntityStatus = await AddFolderEntityUseCase.ExecuteAsync(imgFolder);
					//	Add the physical image
					if (addFolderEntityStatus.Success == true)
					{
						//	img\\posts\\foldername
						var coverPhotoPath = Helpers.Blog.PostsImageBaseDirectory + "\\" + addFolderStatus.FolderName;
						var uploadStatus = await AddCoverPhotoUseCase.ExecuteAsync(newCover, coverPhotoPath);
						if (uploadStatus.Success)
						{
							//	add PostImage entity
							PostImage postImage = new PostImage()
							{
								FileName = uploadStatus.PhotoFileName,
								IsCoverPhoto = true,
								TimeStamp = addFolderStatus.TimeStamp.Value,
								ImageFolderId = addFolderEntityStatus.Id,
							};

							var addPostImageEntityStatus = await AddPostImageEntityUseCase.ExecuteAsync(postImage);
							if (addPostImageEntityStatus.Success == true)
							{
								// Post.ImageFolderCreated has been created
								_postToEdit.PostEntry.PostCoverPhoto = uploadStatus.PhotoFileName;
								_postToEdit.PostEntry.LastModified = DateTime.Now;
								//	update post and redirect to preview
								await EditPostUseCase.ExecuteAsync(_postToEdit.PostEntry);
								return RedirectToPage("/Admin/PostPreview", new { id = _postToEdit.PostEntry.Id });
							}
							else
							{
								// Unable to add postImage entity, log it
								return Page();
							}
						}
						else
						{
							//	Unable to  add the physical image, log it
							return Page();
						}
					}
					else
					{
						//	Could not add Folder db entity, log it
						return Page();
					}

				}
				else
				{
					//	Could not add folder, log error
					return Page();
				}
			}


		}
	}
}
