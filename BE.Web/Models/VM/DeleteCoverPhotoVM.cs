using System.ComponentModel.DataAnnotations;

namespace BE.Web.Models.VM
{
	public class DeleteCoverPhotoVM
	{
		[Required]
		public int PostId { get; set; }
		public string? PostTitle { get; set; }
		public string? CoverPhotoUrl { get; set; }
	}
}
