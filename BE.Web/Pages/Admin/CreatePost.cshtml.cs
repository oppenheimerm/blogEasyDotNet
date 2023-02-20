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

		public IWebHostEnvironment HostEnvironment { get; set; }
        private IAddFolderUseCase AddFolderUseCase { get; set; }
        private IAddFolderEntityUseCase AddFolderEntityUseCase { get; }

        public CreatePostModel(ICreatePostUseCase createPostUseCase, IWebHostEnvironment hostEnvironment,
			IAddCoverPhotoUseCase addCoverPhotoUseCase, IAddFolderUseCase addFolderUseCase,
            IAddFolderEntityUseCase addFolderEntityUseCase)
		{
			CreatePostUseCase = createPostUseCase;
			HostEnvironment = hostEnvironment;
			AddCoverPhotoUseCase = addCoverPhotoUseCase;
            AddCoverPhotoUseCase = addCoverPhotoUseCase;
            AddFolderUseCase = addFolderUseCase;
            AddFolderEntityUseCase = addFolderEntityUseCase;
		}

		public void OnGet()
		{
		}

        public async Task<IActionResult> OnPostAsync()
        {
            if (Request.Form.Files.Count >= 1)
            {
                PostVM.PostCover = Request.Form.Files[0];
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            var post = PostVM.ToPost();
            var hasTags = !PostVM.Tags.IsNullOrEmpty();
            post.Title = ViewHelpers.ToTitleCase(post.Title);
            post.Slug = EntityHelpers.CreateSlug(post.Title);

            //	Has tags?(createPostVM)
            if (hasTags)
            {
                post.Tags = await EntityHelpers.GetPostTagsAsync(PostVM.Tags);

            }

            //	Create folder for post images
            var addFolderStatus = await AddFolderUseCase.ExecuteAsync(Helpers.Blog.PostsImageBaseDirectory);
            if (addFolderStatus.Success == true)
            {
                if (PostVM.PostCover != null)
                {
                    // Post.ImageFolderCreated has been created
                    post.ImageFolderCreated = true;

                    //	img\\posts\\foldername
                    var coverPhotoPath = Helpers.Blog.PostsImageBaseDirectory + "\\" + addFolderStatus.FolderName;
                    var uploadStatus = await AddCoverPhotoUseCase.ExecuteAsync(PostVM.PostCover, coverPhotoPath);
                    if (uploadStatus.Success)
                    {
                        post.PostCoverPhoto = uploadStatus.CoverPhotoFileName;

                        var postStatus = await CreatePostUseCase.ExecuteAsync(post);
                        if (postStatus.Success)
                        {
                            //	add photoObject
                            ImageFolder imgFolder = new ImageFolder()
                            {
                                PostId = postStatus.PostEntry.Id,
                                LastUpdated = DateTime.Now,
                                TimeStamp = addFolderStatus.TimeStamp.Value,
                                Name = addFolderStatus.FolderName
                            };

                            await AddFolderEntityUseCase.ExecuteAsync(imgFolder);
                            return RedirectToPage("/Admin/PostPreview", new { id = postStatus.PostEntry.Id });
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
                    //	Just add post
                    var postStatus = await CreatePostUseCase.ExecuteAsync(post);
                    if (postStatus.Success)
                    {
                        return RedirectToPage("/Admin/PostPreview", new { id = postStatus.PostEntry.Id });
                    }
                    else
                    {
                        return Page();
                    }
                }
            }
            else
            {
                return Page();
            }


        }
    }
}
