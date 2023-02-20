namespace BE.Web.Helpers
{
    public static class Blog
    {
        public static readonly string? Name = "blog:name";
		/// <summary>
		/// Get base directory for blog image persistence to drive. 
		/// need to be in the '\\' format i.e: img\\post\\...
		/// using the '\\' syntax.
		/// </summary>
		public static readonly string PostsImageBaseDirectory = "img\\posts";
		/// <summary>
		/// Get base directory  for blog post to show in view pages.
		/// returns: /img/post/
		/// </summary>
		public static readonly string ViewPostImageBaseDirectory = "/img/posts";
	}

    public static class User
    {
        public static readonly string? Password = "User:password";
        public static readonly string? Salt = "User:salt";
        public static readonly string? UserName = "User:username";
        public static readonly string? HashKey = "User:hashkey";

    }

    public static class Constants
    {
        public static readonly string Space = " ";
        public static readonly string Dash = "-";
    }
}
