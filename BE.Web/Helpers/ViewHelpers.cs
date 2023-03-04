using BE.Core;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace BE.Web.Helpers
{
	public static class ViewHelpers
	{
		private static string[] permittedExtensions = { ".jpg", ".jpeg", ".png" };

		public static string GetPostCoverImage(string folderName, string coverPhoto)
		{
			if (!string.IsNullOrEmpty(folderName) && !string.IsNullOrEmpty(coverPhoto))
			{
				return Helpers.Blog.ViewPostImageBaseDirectory + "/" + folderName + "/" + coverPhoto;
			}
			else
				return string.Empty;
		}

		/// <summary>
		/// Helper for Post Image file urls. Returns basepath/folder/
		/// </summary>
		/// <param name="folderName"></param>
		/// <returns></returns>
		public static string GetPostImageBaseUrl(string folderName)
		{
			return Helpers.Blog.ViewPostImageBaseDirectory + "/" + folderName + "/";
		}

		/// <summary>
		/// Get full path for Post cover photo
		/// </summary>
		/// <param name="foldername"></param>
		/// <param name="coverPhoto"></param>
		/// <returns></returns>
		public static string GetCoverPhotoPath(string foldername, string coverPhoto)
		{
			return Helpers.Blog.PostsImageBaseDirectory + "\\" + foldername + "\\" + coverPhoto;
		}

		public static string GetPostLink(string? postSlug) => $"/blog/{postSlug}/";


		/// <summary>
		/// Truncate a string to a set size.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="maxLength"></param>
		/// <returns></returns>
		/// public static string Truncate(this string value, int maxLength)
		public static string Truncate(string value, int maxLength)
		{
			if (string.IsNullOrEmpty(value)) return value;
			return value.Length <= maxLength ? value : value.Substring(0, maxLength);
		}

		/// <summary>
		/// Returns an instance of a string in Title case format (en-GB)
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public static string ToTitleCase(string input)
		{
			var textInfo = new CultureInfo("en-GB", false).TextInfo;
			return textInfo.ToTitleCase(input);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="val"></param>
		/// <param name="maxlen"></param>
		/// <returns></returns>
		/// public static string ShortenAndFormatText(this string val, int maxlen)
		public static string ShortenAndFormatText(string val, int maxlen)
		{
			const string postFix = "...";

			if (string.IsNullOrEmpty(val)) return val;

			if (val.Length > maxlen)
			{
				var truncateFirst = Truncate(val, (maxlen - postFix.Length));
				var truncateLast = truncateFirst + postFix;
				return truncateLast;
			}
			else
			{
				return val;
			}
		}

		public static string PostTagsToString(PostTag[] tags)
		{
			var _tagsAsString = string.Empty;

			foreach (var tag in tags)
			{
				if (!string.IsNullOrEmpty(tag.TagName))
				{
					_tagsAsString += ToTitleCase(tag.TagName) + ", ";
				}
			}

			return _tagsAsString;
		}

		/// <summary>
		/// Validates a uploaded image file extension
		/// </summary>
		/// <param name="file"></param>
		/// <returns></returns>
		public static bool ValidImageFileExtension(IFormFile file)
		{
			var ext = Path.GetExtension(file.FileName.ToLowerInvariant());
			if (string.IsNullOrEmpty(ext) || !permittedExtensions.Contains(ext))
			{
				return false;
			}
			else
			{
				return true;
			}
		}
	}

	public static class UrlHelperExtensions
	{
		public static string GetLocalUrl(this IUrlHelper urlHelper, string localUrl)
		{
			if (!urlHelper.IsLocalUrl(localUrl))
			{
				return urlHelper!.Page("/Index");
			}

			return localUrl;
		}
	}
}
