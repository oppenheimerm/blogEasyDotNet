namespace BE.Web.Helpers
{
    public static class Blog
    {
        public static readonly string? Name = "blog:name";
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
