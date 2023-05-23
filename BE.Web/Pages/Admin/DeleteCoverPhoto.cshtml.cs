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

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var response = await ViewBlogEntryById.ExecuteAsync(id);

            if (response.Success && response.Item1.ImageFolder is not null)
            {
                DeleteCoverPhotoVM = new DeleteCoverPhotoVM()
                {
                    PostId = response.Item1.Id,
                    PostTitle = response.Item1.Title,
                    CoverPhotoUrl = ViewHelpers.GetPostCoverImage(response.Item1.ImageFolder.Name, response.Item1.PostCoverPhoto)

                };

                return Page();
            }
            else
            {
                return NotFound();
            }
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            //	get the post
            var getPostByIdStatus = await ViewBlogEntryById.ExecuteAsync(id);
            if (getPostByIdStatus.Success)
            {
                if (getPostByIdStatus.Item1.ImageFolder is not null)
                {
                    // Has cover photo?
                    var images = getPostByIdStatus.Item1.ImageFolder.Images;
                    //if(getPostByIdStatus.Item1.ImageFolder.Images.Any()) { }
                    if (images is not null)
                    {
                        var oldCoverPhotoEntity = images.Where(i => i.IsCoverPhoto == true).FirstOrDefault();
                        var oldPhotoPath = ViewHelpers.GetCoverPhotoPath(getPostByIdStatus.Item1.ImageFolder.Name, oldCoverPhotoEntity.FileName);
                        var deleteOldCoverPhotoResponse = await DeletePhotoResponse.ExecuteAsync(oldPhotoPath);
                        if (deleteOldCoverPhotoResponse.Success)
                        {
                            //	Delete the old CoverPhoto db entity(oldCoverPhotoEntity):
                            var deleteOldCoverPhotoStatus = await DeleteCoverPhotoDbEntityUseCase.ExecuteAsync(oldCoverPhotoEntity);
                            if (deleteOldCoverPhotoStatus.Success)
                            {
                                //	update post
                                getPostByIdStatus.Item1.PostCoverPhoto = string.Empty;
                                getPostByIdStatus.Item1.LastModified = DateTime.Now;
                                await EditPostUseCase.ExecuteAsync(getPostByIdStatus.Item1);
                            }
                            else
                            {
                                //	unable to delete old cover phote db entity, log it, return page
                                return Page();
                            }

                            return RedirectToPage("/Admin/PostPreview", new { id = getPostByIdStatus.Item1.Id });
                        }
                        else
                        {
                            //	unable to delete old cover photo, log it, return page
                            return Page();
                        }
                    }
                    else
                    {
                        return NotFound();
                    }

                }
                else
                {
                    return NotFound();
                }


            }
            else
            {
                //	Could not find post, log it
                return NotFound();
            }
        }
    }
}
