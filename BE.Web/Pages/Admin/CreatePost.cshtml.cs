using BE.Core;
using BE.Core.Utilities;
using BE.UseCases.Interfaces;
using BE.Web.Helpers;
using BE.Web.Models.VM;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;

namespace BE.Web.Pages.Admin
{
	public class CreatePostModel : PageModel
	{
		[BindProperty]
		public CreatePostVM? PostVM { get; set; }

		public ICreatePostUseCase CreatePostUseCase { get; }
		public IAddCoverPhotoUseCase AddCoverPhotoUseCase { get; }
		public IAddPostImageEntityUseCase AddPostImageEntityUseCase { get; }
		private IEditPostUseCase EditPostUseCase { get; }

		public IWebHostEnvironment HostEnvironment { get; }
		private IAddFolderUseCase AddFolderUseCase { get; }
		private IAddFolderEntityUseCase AddFolderEntityUseCase { get; }


		public CreatePostModel(ICreatePostUseCase createPostUseCase, IWebHostEnvironment hostEnvironment,
			IAddCoverPhotoUseCase addCoverPhotoUseCase, IAddFolderUseCase addFolderUseCase,
			IAddFolderEntityUseCase addFolderEntityUseCase, IAddPostImageEntityUseCase addPostImageEntityUseCase,
			IEditPostUseCase editPostUseCase)
		{
			CreatePostUseCase = createPostUseCase;
			HostEnvironment = hostEnvironment;
			AddCoverPhotoUseCase = addCoverPhotoUseCase;
			AddFolderUseCase = addFolderUseCase;
			AddFolderEntityUseCase = addFolderEntityUseCase;
			AddPostImageEntityUseCase = addPostImageEntityUseCase;
			EditPostUseCase = editPostUseCase;
		}

		public void OnGet()
		{
		}

		public async Task<IActionResult> OnPostAsync()
		{
			if(PostVM is not null)
			{
                //Request.Form.Files[0]
                if (Request.Form.Files.Count >= 1)
                {
                    PostVM.PostCover = Request.Form.Files[0];

                    if (!FileHelpers.ValidImageFile(PostVM.PostCover))
                    {
                        ModelState.AddModelError("", ".jpg, jpeg, .png files only");
                        return Page();
                    }
                }

                if (!ModelState.IsValid)
                {
                    return Page();
                }


                var post = PostVM.ToPost();
                post.Title = ViewHelpers.ToTitleCase(post.Title);
                post.Slug = EntityHelpers.CreateSlug(post.Title);

                if (PostVM.Tags is not null)
                {
                    if (!string.IsNullOrEmpty(PostVM.Tags))
                    {
                        post.Tags = await EntityHelpers.GetPostTagsAsync(PostVM.Tags);
                    }
                }


                //	Add new post
                var (PostEntry, Success, _) = await CreatePostUseCase.ExecuteAsync(post);
                if ( Success  && PostEntry is not null)
                {
                    bool needsFolder = (Request.Form.Files.Count >= 1) ? true : false;
                    if (needsFolder)
                    {
                        //	Add new folder
                        var addFolderStatus = await AddFolderUseCase.ExecuteAsync(Helpers.Blog.PostsImageBaseDirectory);
                        if (addFolderStatus.Success)
                        {
                            //	Add new folder entity
                            ImageFolder imgFolder = new ImageFolder()
                            {
                                PostId = PostEntry.Id,
                                LastUpdated = DateTime.Now,
                                TimeStamp = addFolderStatus.TimeStamp,
                                Name = addFolderStatus.Foldername
                            };
                            var addFolderEntityStatus = await AddFolderEntityUseCase.ExecuteAsync(imgFolder);

                            //	Add the physical image
                            if (addFolderEntityStatus.Success)
                            {

                                var coverPhotoPath = Helpers.Blog.PostsImageBaseDirectory + "\\" + addFolderStatus.Foldername;
                                var addCoverPhotoStatus = await AddCoverPhotoUseCase.ExecuteAsync(PostVM.PostCover, coverPhotoPath);
                                if (addCoverPhotoStatus.Success)
                                {
                                    //	add PostImage entity
                                    PostImage postImage = new PostImage()
                                    {
                                        FileName = addCoverPhotoStatus.FileName,
                                        IsCoverPhoto = true,
                                        TimeStamp = addFolderStatus.TimeStamp,
                                        ImageFolderId = addFolderEntityStatus.Folder.Id,
                                    };

                                    var addPostImageEntityStatus = await AddPostImageEntityUseCase.ExecuteAsync(postImage);

                                    if (addPostImageEntityStatus.Success)
                                    {
                                        // Post.ImageFolderCreated has been created
                                        PostEntry.PostCoverPhoto = addCoverPhotoStatus.FileName;
                                        //	update post and redirect to preview
                                        await EditPostUseCase.ExecuteAsync(PostEntry);
                                        return RedirectToPage("/Admin/PostPreview", new { id = PostEntry.Id });
                                    }
                                    else
                                    {
                                        //	Could not add ImageEntity
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
                                //	Could not add the physical image, log and return
                                return Page();
                            }

                        }
                        else
                        {
                            // Could not add folder, log and return form
                            return Page();
                        }
                    }
                    else
                    {
                        // we're done
                        return RedirectToPage("/Admin/PostPreview", new { id = PostEntry.Id });
                    }
                }
                else
                {
                    //	Could not create post
                    return Page();
                }
            }
			else
			{
				return NotFound();
			}

		}
	}
}
