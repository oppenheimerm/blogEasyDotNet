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


		public CreatePostModel(ICreatePostUseCase createPostUseCase, IWebHostEnvironment hostEnvironment,
			IAddCoverPhotoUseCase addCoverPhotoUseCase)
		{
			CreatePostUseCase = createPostUseCase;
			HostEnvironment = hostEnvironment;
			AddCoverPhotoUseCase = addCoverPhotoUseCase;
		}

		public void OnGet()
		{
		}

		public async Task<IActionResult> OnPostAsync()
		{
			//Request.Form.Files[0]
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

			// Remember this returns sucess status
			if (PostVM.PostCover != null)
			{
				var uploadStatus = await AddCoverPhotoUseCase.ExecuteAsync(PostVM.PostCover);
				if (uploadStatus.Success)
				{
					post.PostCoverPhoto = uploadStatus.CoverPhotoFileName;
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
	}
}
