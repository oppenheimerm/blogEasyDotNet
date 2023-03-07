using BE.Core;
using BE.UseCases.Interfaces;
using BE.Web.Helpers;
using BE.Web.Models.VM;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BE.Web.Pages.Admin
{
	public class AddPhotoModel : PageModel
	{
		private IAddPostImageEntityUseCase AddPostImageEntityUseCase { get; }
		private IAddPhotoUseCase AddPhotoUseCase { get; }
		private IViewBlogEntryById ViewBlogEntryById { get; }
		private IGetFolderEntityUseCase GetFolderEntityUseCase { get; }
		private IAddFolderUseCase AddFolderUseCase { get; }
		private IAddFolderEntityUseCase AddFolderEntityUseCase { get; }
		private IEditPostUseCase EditPostUseCase { get; }
		[BindProperty]
		public AddPhotoVM AddPhotoVM { get; set; }
		public bool HasImages { get; set; } = false;
		public List<PostImage>? PostImages { get; set; }
		public string? FolderBasePath { get; set; }


		public AddPhotoModel(IAddPostImageEntityUseCase addPostImageEntityUseCase, IViewBlogEntryById viewBlogEntryById,
			IGetFolderEntityUseCase getFolderEntityUseCase, IAddPhotoUseCase addPhotoUseCase, IAddFolderUseCase addFolderUseCase,
			IAddFolderEntityUseCase addFolderEntityUseCase, IEditPostUseCase editPostUseCase)
		{
			AddPostImageEntityUseCase = addPostImageEntityUseCase;
			ViewBlogEntryById = viewBlogEntryById;
			GetFolderEntityUseCase = getFolderEntityUseCase;
			AddPhotoUseCase = addPhotoUseCase;
			AddFolderUseCase = addFolderUseCase;
			AddFolderEntityUseCase = addFolderEntityUseCase;
			EditPostUseCase = editPostUseCase;
		}

		public async Task<IActionResult> OnGetAsync(int id)
		{
			//  When you don't have to include related data, FindAsync is more efficient.
			var responsePost = await ViewBlogEntryById.ExecuteAsync(id);
			if (responsePost.Success)
			{
				AddPhotoVM = responsePost.PostEntry.ToAddPhotoVM();
				if (responsePost.PostEntry.ImageFolder != null)
				{
					PostImages = responsePost.PostEntry.ImageFolder.Images.ToList();
					FolderBasePath = ViewHelpers.GetPostImageBaseUrl(responsePost.PostEntry.ImageFolder.Name);
					HasImages = true;
				}
				return Page();
			}
			else
			{
				return NotFound();
			}

		}

		public async Task<IActionResult> OnPostAsync()
		{

			//	Get photo
			if (Request.Form.Files.Count >= 1)
			{
				AddPhotoVM.NewPhoto = Request.Form.Files[0];

				if (ViewHelpers.ValidImageFileExtension(AddPhotoVM.NewPhoto) == false)
				{
					ModelState.AddModelError("", ".jpg, jpeg, and .png files only");
					return Page();
				}

			}


			if (!ModelState.IsValid)
			{
				return Page();
			}


			// Get post
			var post = await ViewBlogEntryById.ExecuteAsync(AddPhotoVM.PostId);
			if (post.Success)
			{
				// Do we already have photo folder for this post?
				//	 If it has a PostCoverPhoto or 
				if (!string.IsNullOrEmpty(post.PostEntry.PostCoverPhoto)  || post.PostEntry.ImageFolder != null)
				{
					// Get this existing foler
					var getFolderEntityResponseStatus = await GetFolderEntityUseCase.ExecuteAsync(post.PostEntry.ImageFolder.Id);

					if (getFolderEntityResponseStatus.Success)
					{
						//	persist image
						var imagePath = Helpers.Blog.PostsImageBaseDirectory + "\\" + getFolderEntityResponseStatus.Folder.Name;
						var uploadStatus = await AddPhotoUseCase.ExecuteAsync(AddPhotoVM.NewPhoto, imagePath);

						if (uploadStatus.Success)
						{
							//	Sucess?
							//	Add the photo entity to the database
							PostImage newPhoto = new PostImage()
							{
								FileName = uploadStatus.PhotoFileName,
								IsCoverPhoto = false,
								ImageFolderId = getFolderEntityResponseStatus.Folder.Id
							};
							//	Add it
							var addImageEntityStatus = await AddPostImageEntityUseCase.ExecuteAsync(newPhoto);
							if (addImageEntityStatus.Success)
							{
								return RedirectToPage("/Admin/EditPost", new { id = post.PostEntry.Id });
							}
							else
							{
								//	Could not add image entity
								// should log it and return page
								return Page();

							}
						}
						else
						{
							//	Could not upload image
							return Page();
						}
					}
					else
					{
						// Could not get folder
						return Page();
					}

				}
				else
				{
					//	We need to create a image folder for this post
					var addFolderStatus = await AddFolderUseCase.ExecuteAsync(Helpers.Blog.PostsImageBaseDirectory);
					if (addFolderStatus.Success == true)
					{
						//	Add folder db entity
						ImageFolder imgFolder = new ImageFolder()
						{
							PostId = post.PostEntry.Id,
							LastUpdated = DateTime.Now,
							TimeStamp = addFolderStatus.TimeStamp.Value,
							Name = addFolderStatus.FolderName
						};
						var AddFolderEntityStatus = await AddFolderEntityUseCase.ExecuteAsync(imgFolder);

						if (addFolderStatus.Success == true)
						{
							//	Add the physical image file
							var photoPath = Helpers.Blog.PostsImageBaseDirectory + "\\" + addFolderStatus.FolderName;
							var uploadStatus = await AddPhotoUseCase.ExecuteAsync(AddPhotoVM.NewPhoto, photoPath);
							if (uploadStatus.Success)
							{
								//	Add the photo db entity
								PostImage newPhoto = new PostImage()
								{
									FileName = uploadStatus.PhotoFileName,
									IsCoverPhoto = false,
									ImageFolderId = AddFolderEntityStatus.Id
								};
								//	Add it
								var addImageEntityStatus = await AddPostImageEntityUseCase.ExecuteAsync(newPhoto);
								if (addImageEntityStatus.Success)
								{
									//	Update postEntity
									await EditPostUseCase.ExecuteAsync(post.PostEntry);

									return RedirectToPage("/Admin/EditPost", new { id = post.PostEntry.Id });
								}
								else
								{
									//	Could not add image entity
									// should log it and return page
									return Page();

								}
							}
							else
							{
								//	Could not add physical image file
								return Page();
							}
						}
						else
						{
							//	Could not add new folder entity for db
							return Page();
						}
					}
					else
					{
						// Unable to add folder
						return Page();
					}
				}

			}
			else
			{
				// Could not find page error
				return Page();
			}
		}

	}
}
