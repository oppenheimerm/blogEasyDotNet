using BE.Core;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace BE.Web.Helpers
{
	public static class ViewHelpers
	{
		public static string GetPostCoverImage(string folderName, string coverPhoto)
		{
            if(!string.IsNullOrEmpty(folderName) && !string.IsNullOrEmpty(coverPhoto))
            {
                return Helpers.Blog.ViewPostImageBaseDirectory + "/" + folderName + "/" + coverPhoto;
            }
            else
                return string.Empty;
			
		}

		/// <summary>
		/// Get full path for Post cover photo
		/// </summary>
		/// <param name="foldername"></param>
		/// <param name="coverPhoto"></param>
		/// <returns></returns>
		public static string GetCoverPhotoPath(string foldername, string coverPhoto)
		{
            if (!string.IsNullOrEmpty(foldername) && !string.IsNullOrEmpty(coverPhoto)){
                return Helpers.Blog.PostsImageBaseDirectory + "\\" + foldername + "\\" + coverPhoto;
            } else
                return string.Empty;
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
