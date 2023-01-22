

namespace BE.UseCases.Response
{
    public class BaseUseCaseResponse
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
    }
}
