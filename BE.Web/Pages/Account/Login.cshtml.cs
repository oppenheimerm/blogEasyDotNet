using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LgInModel = BE.Web.Models;

namespace BE.Web.Pages.Account
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public LgInModel.LoginModel? SignInModel { get; set; }
        public void OnGet()
        {
        }
    }
}
