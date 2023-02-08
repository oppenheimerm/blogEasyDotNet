using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text;

namespace BE.Web.Helpers
{
    public static class StringHelper
    {
        private static readonly List<string> ReservedCharacters = new List<string>() { "\"!\", \"#\", \"$\", \"&\", \"'\", \"(\", \")\", \"*\", \",\", \"/\", \":\", \";\", \"=\", \"?\", \"@\", \"[\", \"]\", \"\\\"\", \"%\", \".\", \"<\", \">\", \"\\\\\", \"^\", \"_\", \"'\", \"{\", \"}\", \"|\", \"~\", \"`\", \"+\"" };

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

        [SuppressMessage("Globalization", "CA1308:Normalize strings to uppercase", Justification = "The slug should be lower case.")]
        public static string CreateSlug(string title)
        {
            title = title?.ToLowerInvariant().Replace(
                Constants.Space, Constants.Dash, StringComparison.OrdinalIgnoreCase) ?? string.Empty;
            title = RemoveDiacritics(title);
            title = RemoveReservedUrlCharacters(title);

            return title.ToLowerInvariant();
        }

        [SuppressMessage("Globalization", "CA1308:Normalize strings to uppercase", Justification = "The slug should be lower case.")]
        public static string CreateTagSlug(string slug)
        {
            slug = slug?.ToLowerInvariant().Replace(
                Constants.Space, Constants.Dash, StringComparison.OrdinalIgnoreCase) ?? string.Empty;
            slug = RemoveDiacritics(slug);
            slug = RemoveReservedTagCharacters(slug);

            return slug.ToLowerInvariant();
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

        private static string RemoveReservedUrlCharacters(string text)
        {
            //var reservedCharacters = new List<string> { "!", "#", "$", "&", "'", "(", ")", "*", ",", "/", ":", ";", "=", "?", "@", "[", "]", "\"", "%", ".", "<", ">", "\\", "^", "_", "'", "{", "}", "|", "~", "`", "+" };

            foreach (var chr in ReservedCharacters)
            {
                text = text.Replace(chr, string.Empty, StringComparison.OrdinalIgnoreCase);
            }

            return text;
        }

        private static string RemoveReservedTagCharacters(string text)
        {
            //var reservedCharacters = new List<string> { "!", "#", "$", "&", "'", "(", ")", "*", ",", "/", ":", ";", "=", "?", "@", "[", "]", "\"", "%", ".", "<", ">", "\\", "^", "_", "'", "{", "}", "|", "~", "`", "+" };

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

    }
}
