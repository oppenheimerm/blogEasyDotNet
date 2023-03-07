using BE.Core;
using BE.Web.Models.VM;

namespace BE.Web.Helpers
{
	/// <summary>
	/// Extension methods for ViewModels to Model.  Yes, I know I can use AutoMapper here,
	/// but I have triend to keeps it simple, and do it myself.
	/// </summary>
	public static class ModelExtensions
	{

		/// <summary>
		/// From <see cref="CreatePostVM"/> to <see cref="Post"/>
		/// </summary>
		/// <param name="vm"></param>
		/// <returns></returns>
		public static Post ToPost(this CreatePostVM vm)
		{
			return new Post
			{
				Title = vm.Title,
				Excerpt = vm.PostExcerpt,
				Content = vm.PostContent,
				IsPublished = vm.IsPublished
				//Id - null until saved
			};
		}

		/// <summary>
		/// Return and updated <see cref="Post"/> from a <see cref="EditPostVM"/>
		/// </summary>
		/// <param name="vm"></param>
		/// <param name="post"></param>
		/// <returns></returns>
		public static Post ToPost(this EditPostVM vm, Post post)
		{
			post.Title = vm.Title;
			post.Excerpt = vm.PostExcerpt;
			post.Content = vm.PostContent;
			post.IsPublished = vm.IsPublished;
			post.LastModified = DateTime.Now.ToUniversalTime();
			return post;
		}

		/// <summary>
		/// Convert a instance of a <see cref="Post"/> to a <see cref="EditPostVM"/>
		/// </summary>
		/// <param name="post"></param>
		/// <returns></returns>
		public static EditPostVM ToPostVM(this Post post)
		{
			return new EditPostVM
			{
				Title = post.Title,
				PostExcerpt = post.Excerpt,
				PostContent = post.Content,
				IsPublished = post.IsPublished,
				Id = post.Id,
				PostContentParsed = post.Content,
				Tags = ViewHelpers.PostTagsToString(post.Tags.ToArray()),
				PostCover = post.PostCoverPhoto,
				// null check necessary, not all post have image folder!
				PostFolderName = (post.ImageFolder != null) ? post.ImageFolder.Name : string.Empty
			};
		}

		/// <summary>
		/// Returns a lightweight DTO PhotoEnity <see cref="PostImage"/>
		/// </summary>
		/// <param name="post"></param>
		/// <returns></returns>
		public static AddPhotoVM ToAddPhotoVM(this Post post) => new AddPhotoVM
		{
			PostId = post.Id,
			PostTitle = post.Title,
		};

		/// <summary>
		/// Return an instance of a <see cref="EditCoverPhotoVM"/> from <see cref="Post"/>
		/// </summary>
		/// <param name="post"></param>
		/// <returns></returns>
		public static EditCoverPhotoVM ToToEditCoverPhotoVM(this Post post) => new EditCoverPhotoVM
		{
			PostId = post.Id,
			CoverPhoto = (!string.IsNullOrEmpty(post.PostCoverPhoto)) ? post.PostCoverPhoto : string.Empty,
			PostTitle = post.Title,
			PostFolderName = (post.ImageFolder != null) ? post.ImageFolder.Name : string.Empty,
		};
	}
}
