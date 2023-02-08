using Microsoft.AspNetCore.Mvc;

namespace BE.Web.Helpers
{
	public static class ViewHelpers
	{
		public static string GetPostBackgroundImage(string? imageSrc)
		{
			return $"/img/posts/{imageSrc}";
		}

		public static string GetPostLink(string? postSlug) => $"/blog/{postSlug}/";
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
