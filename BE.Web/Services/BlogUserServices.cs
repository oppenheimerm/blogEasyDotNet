using BE.Web.Helpers;
using System.Security.Cryptography;

namespace BE.Web.Services
{
    public class BlogUserServices : IBlogUserServices
    {
        const int keySize = 64;
        const int iterations = 350000;
        readonly HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA512;

        private IConfiguration Config { get; }

        public BlogUserServices(IConfiguration config)
        {
            Config = config;
        }

        public bool ValidateUser(string username, string password)
        {
            if ((User.UserName is not null) && (username == Config[User.UserName]) && (VerifyPassword(password) == true))
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        bool VerifyPassword(string password)
        {
            var storedSalt = Convert.FromHexString(Config[User.Salt]);
            var hashToCompare = Rfc2898DeriveBytes.Pbkdf2(password, storedSalt, iterations, hashAlgorithm, keySize);
            return hashToCompare.SequenceEqual(Convert.FromHexString(Config[User.HashKey]));
        }
    }
}
