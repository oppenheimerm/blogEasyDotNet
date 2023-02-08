using BE.Core;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text;

namespace BE.DataStore.EFCore
{
    public static class DbInitializer
    {
        private static List<Post>? InitialPostCollection;

        public static async void Initialize(BEDbContext context)
        {
            InitialPostCollection = new List<Post>();
            //InitialPostTagCollection = new List<PostTag>();
            //  Look for any posts
            if (context.Posts.Any())
            {
                return; // Db has already been seeded
            }

            await InitDb(context);

        }

        private static void GeneratePosts(BEDbContext context)
        {
            var _posts = new Post[] {
                new Post {
                    Title = $"Welcome to \'BlogEasy\' Blog",
                    Content = $"<p>\r\nLorem ipsum dolor sit amet, consectetur adipiscing elit. Aenean lacinia nibh et tempor cursus. Aenean lobortis leo a velit imperdiet, nec sagittis augue dictum. In vel viverra purus. Curabitur id purus ac nisl viverra tempor. Ut tincidunt mauris vitae quam fringilla mollis. Duis accumsan, tellus ut rhoncus venenatis, eros orci gravida neque, quis luctus ante urna sed ligula. Donec sit amet orci vel sapien consectetur maximus non ut mauris. Curabitur et scelerisque est. Proin in massa non erat mollis viverra. Aliquam mattis nisi vehicula, semper dui et, cursus nulla. Aliquam malesuada erat et sem porta malesuada. Curabitur finibus lacinia enim vitae elementum.\r\n</p><p>\r\nUt ut ante ac turpis sollicitudin feugiat. Vestibulum sed lobortis dolor. Ut mollis porttitor enim, id aliquet ipsum ultrices et. Etiam eget velit elementum, mattis enim nec, posuere arcu. Nullam et arcu ultrices, blandit augue ac, tincidunt sem. Suspendisse quis orci non sapien tincidunt egestas. Vivamus ac nisl quis massa consequat lacinia eu in sem. Ut finibus ligula sit amet nisi porta, id luctus velit hendrerit. Pellentesque sit amet egestas est. Suspendisse ornare quis nulla sed luctus. Nullam viverra sapien sit amet ipsum iaculis vestibulum. Proin rutrum ex quis quam rhoncus tempor. Duis sed placerat ligula.\r\n</p> <p>\r\nPraesent finibus maximus diam, accumsan lobortis diam maximus porttitor. Nulla vitae ligula malesuada, lobortis mi non, lacinia lectus. Phasellus nec aliquam metus. Praesent nunc dui, facilisis ornare leo nec, sagittis facilisis nisi. Praesent posuere lectus ut ipsum euismod, et imperdiet felis imperdiet. Pellentesque in elementum diam. In nisl ex, tincidunt eget ultricies ut, congue accumsan lacus. Quisque quis neque vitae arcu laoreet pretium. Duis in nunc id nisl fermentum facilisis.\r\n</p>",
                    Excerpt = "Consectetur adipiscing elit. Aenean lacinia nibh et tempor cursus. Aenean lobortis leo a velit imperdiet, nec sagittis augue dictum. In vel viverra purus. Curabitur id purus ac nisl viverra tempor. Ut tincidunt mauris vitae quam fringilla",
                    PubDate = DateTime.Now  ,
                    LastModified = DateTime.Now,
                    PostCoverPhoto = "vs-code.png" ,
                    Tags = new[]
                    {
                        new PostTag{ TagName = "C#"},
                        new PostTag{ TagName = "Asp.Net"},
                        new PostTag{ TagName = "web"}
                    }
                },
				new Post
				{
					Title= $"Blog Easy Is Math's Ready!" ,
					Content = $"# How about Maths?\r\n\r\nWith help from [MathJax](https://www.mathjax.org/) We've got you covered:\r\n\r\nWhen $a \\ne 0$, there are two solutions to \r\n$\\(x^2 + y^2 = z^2\\)$, and they are:\r\n\r\n$$ x = {{-b \\pm \\sqrt{{b^2-4ac}} \\over 2a}}$$.\r\n\r\n### The Lorenz Equations\r\n$$\r\n\\begin{{align}}\r\n    \\dot{{x}} & = \\sigma(y-x) \\\\\r\n    \\dot{{y}} & = \\rho x - y - xz \\\\\r\n    \\dot{{z}} & = -\\beta z + xy\r\n    \\end{{align}}\r\n$$\r\n\r\n### The Cauchy-Schwarz Inequality\r\n$$\r\n\\left( \\sum_{{k=1}}^n a_k b_k \\right)^{{\\!\\!2}} \\leq\r\n     \\left( \\sum_{{k=1}}^n a_k^2 \\right) \\left( \\sum_{{k=1}}^n b_k^2 \\right)\r\n$$\r\n\r\n### A Cross Product Formula\r\n\r\n$$\r\n\\mathbf{{V}}_1 \\times \\mathbf{{V}}_2 =\r\n       \\begin{{vmatrix}}\r\n        \\mathbf{{i}} & \\mathbf{{j}} & \\mathbf{{k}} \\\\\r\n        \\frac{{\\partial X}}{{\\partial u}} & \\frac{{\\partial Y}}{{\\partial u}} & 0 \\\\\r\n        \\frac{{\\partial X}}{{\\partial v}} & \\frac{{\\partial Y}}{{\\partial v}} & 0 \\\\\r\n       \\end{{vmatrix}}\r\n$$\r\n\r\n\r\n### An Identity of Ramanujan\r\n\r\n$$\r\n\\frac{{1}}{{(\\sqrt{{\\phi \\sqrt{{5}}-\\phi) e^{{\\frac25 \\pi}} =\r\n         1+\\frac{{e^{{-2\\pi}} {{1+\\frac{{e^{{-4\\pi}} {{1+\\frac{{e^{{-6\\pi}}\r\n          {{1+\\frac{{e^{{-8\\pi}} {{1+\\ldots}} }} }} }}\r\n$$\r\n\r\n### In-line Mathematics\r\nFinally, while display equations look good for a page of samples, the ability to mix math and text in a paragraph is also important. This expression $\\sqrt{{3x-1}}+(1+x)^2 $ \r\n is an example of an inline equation. As you see, MathJax equations can be used this way as well, without unduly disturbing the spacing between lines.\r\n\r\nYou can open a file from **Google Drive**, **Dropbox** or **GitHub** by opening the **Synchronize** sub-menu and clicking **Open from**. Once opened in the workspace, any modification in the file will be automatically synced.\r\n\r\n> You can find more information about **LaTeX** mathematical expressions [here](http://meta.math.stackexchange.com/questions/5020/mathjax-basic-tutorial-and-quick-reference).",
					PubDate = DateTime.Now.AddDays(-30) ,
					Excerpt = "BlogEasy.Net make's use of MathJax",
					LastModified = DateTime.Now.AddDays(-30),
					PostCoverPhoto = "MathJax.png",
					Tags = new[]
					{
						new PostTag{ TagName = "Mathax"},
						new PostTag{ TagName = "Math"},
						new PostTag{ TagName = "C#"}
					}
				}
			};

            foreach (var item in _posts)
            {
                item.Title = DbInitHelpers.ToTitleCase(item.Title);
                //  Add slug
                item.Slug = DbInitHelpers.CreateSlug(item.Title);
                //  Publish
                item.IsPublished = true;

                if (item.Tags != null)
                {
                    foreach (var t in item.Tags)
                    {
                        //var tag = await context.PostTags.FirstOrDefaultAsync(x => x.Id == t.Id);
                        //var tag = await context.PostTags.FindAsync(t.Id);
                        t.TagNameEncoded = DbInitHelpers.CreateSlug(t.TagName);
                    }

                }

                //	Submit post so we get the id
                //await context.Posts.AddAsync(item);			

                InitialPostCollection.Add(item);
            }
        }


        /*public static async Task SavePost(BEDbContext context)
        {
            foreach (var item in InitialPostCollection)
            {
                await context.Posts.AddAsync(item);
            }
            await context.SaveChangesAsync();
        }*/

        public static async Task InitDb(BEDbContext context)
        {

            //  Generate Post
            Task taskPost = Task.Factory.StartNew(delegate
            { GeneratePosts(context); });
            taskPost.Wait();

            if (taskPost.IsCompleted)
            {

                InitialPostCollection.ForEach(p => context.Posts.Add(p));
                context.SaveChangesAsync().Wait();
            }
        }
    }

    public static class DbInitHelpers
    {
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
            var reservedCharacters = new List<string> { "!", "#", "$", "&", "'", "(", ")", "*", ",", "/", ":", ";", "=", "?", "@", "[", "]", "\"", "%", ".", "<", ">", "\\", "^", "_", "'", "{", "}", "|", "~", "`", "+" };

            foreach (var chr in reservedCharacters)
            {
                text = text.Replace(chr, string.Empty, StringComparison.OrdinalIgnoreCase);
            }

            return text;
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
