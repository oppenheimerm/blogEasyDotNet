
using System.ComponentModel.DataAnnotations;

namespace BE.Core
{
	/// <summary>
	/// Collection of images associated with a <see cref="Post"/>
	/// </summary>
	public class ImageFolder
	{
		[Required]
		public int Id { get; set; }
		[Required]
		public string Name { get; set; } = string.Empty;
		[Required]
		public int PostId { get; set; }
		public Post? PostPost { get; set; }
		public DateTime TimeStamp { get; set; }
		public DateTime LastUpdated { get; set; }

		public ICollection<PostImage>? Images { get; set; }
	}
}
