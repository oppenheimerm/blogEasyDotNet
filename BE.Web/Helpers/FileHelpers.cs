namespace BE.Web.Helpers
{
    public static class FileHelpers
    {
        private static string[] permittedExtensions = { ".jpg", ".jpeg", ".png", ".webp" };

        public static bool ValidImageFile(IFormFile file)
        {
            var ext = Path.GetExtension(file.FileName.ToLowerInvariant());
            if (string.IsNullOrEmpty(ext) || !permittedExtensions.Contains(ext))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
