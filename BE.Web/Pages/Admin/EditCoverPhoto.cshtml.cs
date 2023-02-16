using BE.UseCases.Interfaces;
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
		private IDeleteCoverPhotoUseCase DeletePhotoResponse { get; set; }
		private IEditPostUseCase EditPostUseCase { get; }

		public EditCoverPhotoModel(IViewBlogEntryById viewBlogEntryById, IDeleteCoverPhotoUseCase deletePhotoResponse,
			IAddCoverPhotoUseCase addCoverPhotoUseCase, IEditPostUseCase editPostUseCase)
		{
			ViewBlogEntryById = viewBlogEntryById;
			DeletePhotoResponse = deletePhotoResponse;
			AddCoverPhotoUseCase = addCoverPhotoUseCase;
			EditPostUseCase = editPostUseCase;
		}

		public void OnGet()
        {
        }
    }
}
