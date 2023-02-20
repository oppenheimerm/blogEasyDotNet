using System.ComponentModel.DataAnnotations;

namespace BE.Web.Models.VM
{
	public class EditPostVM
	{
		public int? Id { get; set; }
		[Required]
		[MaxLength(160, ErrorMessage = "Maximum 160 character limit")]
		public string Title { get; set; } = string.Empty;
		[Required]
		[MaxLength(512, ErrorMessage = "Maximum 512 character limit")]
		public string PostExcerpt { get; set; } = string.Empty;
		[Required]
		public string PostContent { get; set; } = string.Empty;
		public bool IsPublished { get; set; } = false;
		public DateTime LastModified { get; set; } = DateTime.UtcNow;
		[Required]
		public string PostContentParsed { get; set; } = string.Empty;
		/// <summary>
		/// Tags as string
		/// </summary>
		public string? Tags { get; set; } = string.Empty;
		public string PostCover { get; set; } = string.Empty;
		public string PostFolderName { get; set; } = string.Empty;
	}
}
