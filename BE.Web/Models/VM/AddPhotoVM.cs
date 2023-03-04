using System.ComponentModel.DataAnnotations;

namespace BE.Web.Models.VM
{
	public class AddPhotoVM
	{
		[Required]
		public IFormFile? NewPhoto { get; set; }
		[Required]
		public int PostId { get; set; }
		public string? PostTitle { get; set; }
	}
}
