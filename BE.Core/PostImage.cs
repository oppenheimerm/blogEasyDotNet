
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BE.Core
{
	/// <summary>
	/// An instance of a post image, or cover photo for <see cref="Post"/>
	/// </summary>
	public class PostImage
	{
		[Key]
		public int Id { get; set; }
		[Required]
		public string FileName { get; set; } = string.Empty;
		public bool IsCoverPhoto { get; set; } = false;
		public DateTime TimeStamp { get; set; } = DateTime.Now;
		[Required]
		[ForeignKey("ImageFolder")]
		public int ImageFolderId { get; set; }
		public ImageFolder? ImageFolder { get; set; }
	}
}
