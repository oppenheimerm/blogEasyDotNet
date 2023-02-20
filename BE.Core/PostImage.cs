
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

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
		public string? FileName { get; set; }
		public bool IsCoverPhoto { get; set; } = false;
		public DateTime TimeStamp { get; set; } = DateTime.Now;
		[Required]
		[ForeignKey("Post")]
		public int PostId { get; set; }
		public Post? Post { get; set; }
	}
}
