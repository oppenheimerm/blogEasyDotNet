
namespace BE.DataStore.EFCore.Utilities
{
	public static class StringExtensions
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public static string RemoveWhitespace(this string input)
		{
			return new string(input.ToCharArray()
				.Where(c => !Char.IsWhiteSpace(c))
				.ToArray());
		}

		/// <summary>
		/// Clean image filename. Excludes extension
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public static string CleanAndFormatPhotoName(this string name)
		{
			var cleanedString = RemoveWhitespace(name);
			cleanedString = String.Concat(cleanedString.Where(char.IsLetterOrDigit));
			return cleanedString.ToLowerInvariant();
		}
	}
}
