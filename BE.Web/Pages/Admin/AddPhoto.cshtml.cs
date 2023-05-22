using BE.Core;
using BE.UseCases.Interfaces;
using BE.Web.Helpers;
using BE.Web.Models.VM;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace BE.Web.Pages.Admin
{
	public class AddPhotoModel : PageModel
	{
        private readonly ILogger<AddPhotoModel> Logger;
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
			IAddFolderEntityUseCase addFolderEntityUseCase, IEditPostUseCase editPostUseCase, ILogger<AddPhotoModel> logger)
		{
			AddPostImageEntityUseCase = addPostImageEntityUseCase;
			ViewBlogEntryById = viewBlogEntryById;
			GetFolderEntityUseCase = getFolderEntityUseCase;
			AddPhotoUseCase = addPhotoUseCase;
			AddFolderUseCase = addFolderUseCase;
			AddFolderEntityUseCase = addFolderEntityUseCase;
			EditPostUseCase = editPostUseCase;
            Logger = logger;
		}

        public async Task<IActionResult> OnGetAsync(int id)
        {
            //  When you don't have to include related data, FindAsync is more efficient.
            var responsePost = await ViewBlogEntryById.ExecuteAsync(id);
            if (responsePost.Success)
            {
                AddPhotoVM = responsePost.Item1.ToAddPhotoVM();
                if (responsePost.Item1.ImageFolder != null)
                {
                    PostImages = responsePost.Item1.ImageFolder?.Images?.ToList();
                    FolderBasePath = ViewHelpers.GetPostImageBaseUrl(responsePost.Item1.ImageFolder?.Name ?? string.Empty);
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

            //	Get / verify new photo
            if (Request.Form.Files.Count >= 1 && AddPhotoVM is not null)
            {
                AddPhotoVM.NewPhoto = Request.Form.Files[0];

                if (!FileHelpers.ValidImageFile(AddPhotoVM.NewPhoto))
                {
                    ModelState.AddModelError("", ".jpg, jpeg, and .png files only");
                    return Page();
                }

            }
            else
            {
                ModelState.AddModelError("", "No photo found.");
                return Page();
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            // get associated post
            var post = await ViewBlogEntryById.ExecuteAsync(AddPhotoVM.PostId);
            if (post.Success)
            {
                //	Get existing folderDbEntity
                var folderEntity = await GetFolderEntityUseCase.ExecuteAsync(post.Item1.Id);
                if (folderEntity.Success)
                {
                    //	persist the image
                    var imagePath = Helpers.Blog.PostsImageBaseDirectory + "\\" + folderEntity.FolderEntity.Name;
                    var uploadStatus = await AddPhotoUseCase.ExecuteAsync(AddPhotoVM.NewPhoto, imagePath);
                    if (uploadStatus.Success)
                    {
                        //	Sucess?
                        //	Add the photo entity to the database
                        PostImage newPhoto = new()
                        {
                            FileName = uploadStatus.FileName,
                            IsCoverPhoto = false,
                            ImageFolderId = folderEntity.FolderEntity.Id
                        };
                        //	Add it
                        var addImageEntityStatus = await AddPostImageEntityUseCase.ExecuteAsync(newPhoto);
                        if (addImageEntityStatus.Success)
                        {
                            return RedirectToPage("/Admin/EditPost", new { id = post.Item1.Id });
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
                        // was unable to uplad image, log it, return internal server error
                        Logger.LogError($"Could not uplaod image. time: {DateTime.UtcNow}");
                        return StatusCode(500);
                    }
                }
                else
                {
                    // need to add folder entity, then add new photo
                    var newFolderStatus = await AddFolderUseCase.ExecuteAsync(Helpers.Blog.PostsImageBaseDirectory);
                    if (newFolderStatus.Success == true)
                    {
                        //	Add folder db entity
                        ImageFolder imgFolder = new ImageFolder()
                        {
                            PostId = post.Item1.Id,
                            LastUpdated = DateTime.UtcNow,
                            TimeStamp = newFolderStatus.TimeStamp,
                            Name = newFolderStatus.Foldername
                        };
                        var AddFolderEntityStatus = await AddFolderEntityUseCase.ExecuteAsync(imgFolder);

                        if (newFolderStatus.Success == true)
                        {
                            //	Add the physical image file
                            var photoPath = Helpers.Blog.PostsImageBaseDirectory + "\\" + newFolderStatus.Foldername;
                            var uploadStatus = await AddPhotoUseCase.ExecuteAsync(AddPhotoVM.NewPhoto, photoPath);
                            if (uploadStatus.Success)
                            {
                                //	Add the photo db entity
                                PostImage newPhoto = new()
                                {
                                    FileName = uploadStatus.FileName,
                                    IsCoverPhoto = false,
                                    ImageFolderId = AddFolderEntityStatus.Folder.Id
                                };
                                //	Add it
                                var addImageEntityStatus = await AddPostImageEntityUseCase.ExecuteAsync(newPhoto);
                                if (addImageEntityStatus.Success)
                                {
                                    //	Update postEntity
                                    await EditPostUseCase.ExecuteAsync(post.Item1);
                                    return RedirectToPage("/Admin/EditPost", new { id = post.Item1.Id });
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

                    return Page();
                }
            }
            else
            {
                //	could not get post for this image for some reason,
                //	log it, return internal server error
                Logger.LogError($"Could not add photo for post: {AddPhotoVM.PostId}, post not found");
                return StatusCode(500);
            }
        }

    }
}
