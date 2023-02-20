using System.ComponentModel.DataAnnotations;

namespace BE.Web.Models.VM
{
	public class EditCoverPhotoVM
	{
		public int? PostId { get; set; }
		[Required]
		[MaxLength(256, ErrorMessage = "Filename is maximum 256 character.")]
		public IFormFile? PostCover { get; set; }
		public string? CoverPhoto { get; set; }
		public string? PostTitle { get; internal set; }
		public string? PostFolderName { get; set; }
	}
}
