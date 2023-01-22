using BE.Core;
using Microsoft.EntityFrameworkCore;

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
            var posts = new Post[] {
                new Post {
                    Title = $"Welcome to \'BlogEasy\' Blog",
                    Content = $"<p>\r\nLorem ipsum dolor sit amet, consectetur adipiscing elit. Aenean lacinia nibh et tempor cursus. Aenean lobortis leo a velit imperdiet, nec sagittis augue dictum. In vel viverra purus. Curabitur id purus ac nisl viverra tempor. Ut tincidunt mauris vitae quam fringilla mollis. Duis accumsan, tellus ut rhoncus venenatis, eros orci gravida neque, quis luctus ante urna sed ligula. Donec sit amet orci vel sapien consectetur maximus non ut mauris. Curabitur et scelerisque est. Proin in massa non erat mollis viverra. Aliquam mattis nisi vehicula, semper dui et, cursus nulla. Aliquam malesuada erat et sem porta malesuada. Curabitur finibus lacinia enim vitae elementum.\r\n</p><p>\r\nUt ut ante ac turpis sollicitudin feugiat. Vestibulum sed lobortis dolor. Ut mollis porttitor enim, id aliquet ipsum ultrices et. Etiam eget velit elementum, mattis enim nec, posuere arcu. Nullam et arcu ultrices, blandit augue ac, tincidunt sem. Suspendisse quis orci non sapien tincidunt egestas. Vivamus ac nisl quis massa consequat lacinia eu in sem. Ut finibus ligula sit amet nisi porta, id luctus velit hendrerit. Pellentesque sit amet egestas est. Suspendisse ornare quis nulla sed luctus. Nullam viverra sapien sit amet ipsum iaculis vestibulum. Proin rutrum ex quis quam rhoncus tempor. Duis sed placerat ligula.\r\n</p> <p>\r\nPraesent finibus maximus diam, accumsan lobortis diam maximus porttitor. Nulla vitae ligula malesuada, lobortis mi non, lacinia lectus. Phasellus nec aliquam metus. Praesent nunc dui, facilisis ornare leo nec, sagittis facilisis nisi. Praesent posuere lectus ut ipsum euismod, et imperdiet felis imperdiet. Pellentesque in elementum diam. In nisl ex, tincidunt eget ultricies ut, congue accumsan lacus. Quisque quis neque vitae arcu laoreet pretium. Duis in nunc id nisl fermentum facilisis.\r\n</p>",
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
                    Title= $"I'm ready for maths!" ,
                    Content = $"<p>\r\nMorbi euismod mollis turpis a interdum. Pellentesque auctor massa a dolor sollicitudin, non scelerisque libero laoreet. Nunc ut massa sodales, tincidunt purus vel, sollicitudin nisi. Nulla facilisi. Nunc faucibus mauris a tristique facilisis. Sed sollicitudin lorem at mollis condimentum. Vivamus pellentesque ante eget lectus sagittis, nec posuere diam ullamcorper.\r\n</p> <span class=\"MathJax_Preview\" style=\"color: inherit; display: none;\"></span> <p>\r\nDuis ut nunc condimentum, rutrum velit at, gravida nisl. Proin quis lectus a urna dignissim auctor. Maecenas sodales tincidunt mi, a interdum nulla. Sed ante tortor, placerat sed neque eu, faucibus commodo lacus. Nunc ut dignissim odio, at euismod ipsum. Morbi convallis egestas egestas. Morbi molestie luctus hendrerit. Proin posuere sem et ex finibus sollicitudin.\r\n</p> <span class=\"MathJax_Preview\" style=\"color: inherit; display: none;\"></span>",
                    PubDate = DateTime.Now.AddDays(-30) ,
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
        }

        public static async Task InitDb(BEDbContext context)
        {

            //  Generate Post
            Task taskPost = Task.Factory.StartNew(delegate
            { GeneratePosts(context); });
            taskPost.Wait();
        }
    }
}
