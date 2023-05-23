using BE.Core;
using BE.UseCases.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BE.Web.Pages.Admin
{
    public class DeletePostModel : PageModel
    {
		[BindProperty]
		public Post? Post { get; set; }
		private IViewBlogEntryById ViewBlogEntryById { get; }
		public string ImageFolderBase { get; set; }

        private IPurgPostFilesUseCase PurgPostFilesUseCase { get; }
        private IDeletePostUseCase DeletePostUseCase { get; }

        public DeletePostModel(IViewBlogEntryById viewBlogEntryById, IDeletePostUseCase deletePostUseCase,
            IPurgPostFilesUseCase purgPostFilesUseCase)
		{
            ViewBlogEntryById = viewBlogEntryById;
            DeletePostUseCase = deletePostUseCase;
            PurgPostFilesUseCase = purgPostFilesUseCase;
        }


        public async Task<IActionResult> OnGetAsync(int Id)
        {
            var getBlogByIdStatus = await ViewBlogEntryById.ExecuteAsync(Id);
            if (getBlogByIdStatus.Item1 is not null && getBlogByIdStatus.Success)
            {
                Post = getBlogByIdStatus.Item1;
                ImageFolderBase = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/";
                return Page();
            }
            {
                // unable to retreive blod by id passed in. return
                // not found
                return NotFound();
            }
        }

        public async Task<IActionResult> OnPostAsync(int Id)
        {
            // Get the post
            var getPostStatus = await ViewBlogEntryById.ExecuteAsync(Id);
            if (getPostStatus.Success)
            {
                //	Does this post have an associated image folder? Delete it first
                //  if (!string.IsNullOrEmpty(getPostByIdStatus.PostEntry.PostCoverPhoto) || (oldCoverPhotoEntity != null))
                if (!string.IsNullOrEmpty(getPostStatus.Item1.PostCoverPhoto) || getPostStatus.Item1.ImageFolder != null)
                {
                    //	purge files
                    var folderPath = Helpers.Blog.PostsImageBaseDirectory + "\\" + getPostStatus.Item1.ImageFolder?.Name;
                    var purgePostFilesResponse = await PurgPostFilesUseCase.ExecuteAsync(folderPath);
                    if (purgePostFilesResponse.Success)
                    {
                        //	Proceed to delete the post
                        var deletePostStatus = await DeletePostUseCase.ExecuteAsync(getPostStatus.Item1.Id);
                        if (deletePostStatus.Success)
                        {
                            return RedirectToPage("/Admin/AllPosts");
                        }
                        else
                        {
                            //	Could not delete post, log it
                            return Page();
                        }
                    }
                    else
                    {
                        //	Could not delete files for post, log it and return
                        return Page();
                    }
                }
                else
                {
                    //	No associated imge files for this post, proceed with post deletion
                    var deletePostStatus = await DeletePostUseCase.ExecuteAsync(getPostStatus.Item1.Id);
                    if (deletePostStatus.Success)
                    {
                        return RedirectToPage("/Admin/AllPosts");
                    }
                    else
                    {
                        //	Could not delete post, log it
                        return Page();
                    }
                }

            }
            else
            {
                //	Could not get post entry
                return NotFound();
            }
        }
    }
}
