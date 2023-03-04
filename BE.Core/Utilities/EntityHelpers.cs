using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text;

namespace BE.Core.Utilities
{
	public static class EntityHelpers
	{
		private static readonly List<string> ReservedCharacters = new List<string>() { "\"!\", \"#\", \"$\", \"&\", \"'\", \"(\", \")\", \"*\", \",\", \"/\", \":\", \";\", \"=\", \"?\", \"@\", \"[\", \"]\", \"\\\"\", \"%\", \".\", \"<\", \">\", \"\\\\\", \"^\", \"_\", \"'\", \"{\", \"}\", \"|\", \"~\", \"`\", \"+\"" };

		public static Task<List<PostTag>> GetPostTagsAsync(string tagsAsString)
		{
			return Task.Run<List<PostTag>>(() =>
			{
				return tagsAsString.Split(',')
				.Select(x => x.Trim())
				.Where(x => !string.IsNullOrWhiteSpace(x))
				.Select(p => new PostTag
				{
					TagName = p.Trim().ToLowerInvariant(),
					TagNameEncoded = CreateSlug(p.Trim().ToLowerInvariant())
				})
				.ToList<PostTag>();
			});

		}

		/// <summary>
		/// Helper method for converting tags(as string) into a List<PostTag>
		/// </summary>
		/// <param name="tagsAsString"></param>
		/// <param name="postId"></param>
		/// <returns></returns>
		public static Task<List<PostTag>> GetPostTagsAsync(string tagsAsString, int postId)
		{
			return Task.Run<List<PostTag>>(() =>
			{
				return tagsAsString.Split(',')
				.Select(x => x.Trim())
				.Where(x => !string.IsNullOrWhiteSpace(x))
				.Select(p => new PostTag
				{
					TagName = p.Trim().ToLowerInvariant(),
					TagNameEncoded = CreateSlug(p.Trim().ToLowerInvariant()),
					PostId = postId
				})
				.ToList<PostTag>();
			});

		}


		private static string RemoveReservedUrlCharacters(string text)
		{

			//	I don't like turning C# or C++ into c....
			var checkForCLang = text.ToLowerInvariant();
			switch (checkForCLang)
			{
				case "c++":
					text = "cplusplus";
					break;
				case "c#":
					text = "csharp";
					break;
				default:
					foreach (var chr in ReservedCharacters)
					{
						text = text.Replace(chr, string.Empty, StringComparison.OrdinalIgnoreCase);
					}
					break;
			}

			return text;
		}

		private static string RemoveDiacritics(string text)
		{
			var normalizedString = text.Normalize(NormalizationForm.FormD);
			var stringBuilder = new StringBuilder();

			foreach (var c in normalizedString)
			{
				var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
				if (unicodeCategory != UnicodeCategory.NonSpacingMark)
				{
					stringBuilder.Append(c);
				}
			}

			return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
		}

		[SuppressMessage("Globalization", "CA1308:Normalize strings to uppercase", Justification = "The slug should be lower case.")]
		public static string CreateSlug(string title)
		{
			title = title?.ToLowerInvariant().Replace(
				Constants.Space, Constants.Dash, StringComparison.OrdinalIgnoreCase) ?? string.Empty;
			title = RemoveDiacritics(title);
			title = RemoveReservedUrlCharacters(title);

			return title.ToLowerInvariant();
		}
	}

	public static class Constants
	{
		public static readonly string Space = " ";
		public static readonly string Dash = "-";
	}
}
