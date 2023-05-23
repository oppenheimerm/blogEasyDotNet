namespace BE.Web.Services
{
    public interface IBlogUserServices
    {
        bool ValidateUser(string username, string password);
    }
}
