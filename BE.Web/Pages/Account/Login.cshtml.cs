using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using BE.Web.Models;
using BE.Web.Services;
using LgInModel = BE.Web.Models;
using BE.Web.Helpers;

namespace BE.Web.Pages.Account
{
    public class Login : PageModel
    {
        private readonly IBlogUserServices blogUserServices;
        private readonly IConfiguration config;
        private readonly ILogger<Login> Logger;

        public Login(
            IBlogUserServices blogUserServices,
            IConfiguration config,
            ILogger<Login> logger)
        {
            this.blogUserServices = blogUserServices;
            this.config = config;
            LoginModel = new();
            Logger = logger;
        }

        public void OnGet()
        {

        }

        [BindProperty]
        public LoginModel LoginModel { get; set; }

        public string ReturnUrl { get; set; } = string.Empty;

        [TempData]
        public string ErrorMessage { get; set; } = string.Empty;

        public async Task OnGet(string? returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            // Clear the existing external cookie
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);

            // return the value of returnUrl if not empty, else, jsut return string.empty
            ReturnUrl = returnUrl ?? string.Empty;
        }

        public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
        {
            ReturnUrl = returnUrl ?? string.Empty;

            if (ModelState.IsValid)
            {
                // Use LoginModel.Email and LoginModel.Password to authenticate the user
                // with our custom authentication logic, aganist the user fields in the 
                // secerets file
                //
                var user = blogUserServices.ValidateUser(LoginModel.Email, LoginModel.Password);
                if (user)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, LoginModel.Email),
                        new Claim("FullName", config["User:fullname"].ToString()),
                        new Claim(ClaimTypes.Role, "Administrator"),
                    };

                    var claimsIdentity = new ClaimsIdentity(
                        claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    var authProperties = new AuthenticationProperties
                    {
                        //AllowRefresh = <bool>,
                        // Refreshing the authentication session should be allowed.

                        //ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                        // The time at which the authentication ticket expires. A 
                        // value set here overrides the ExpireTimeSpan option of 
                        // CookieAuthenticationOptions set with AddCookie.

                        //IsPersistent = true,
                        // Whether the authentication session is persisted across 
                        // multiple requests. When used with cookies, controls
                        // whether the cookie's lifetime is absolute (matching the
                        // lifetime of the authentication ticket) or session-based.

                        //IssuedUtc = <DateTimeOffset>,
                        // The time at which the authentication ticket was issued.

                        //RedirectUri = <string>
                        // The full path or absolute URI to be used as an http 
                        // redirect response value.
                    };

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties);


                    Logger.LogInformation("User {Email} logged in at {Time}.",
                        LoginModel.Email, DateTime.UtcNow);

                    return LocalRedirect(Url.GetLocalUrl(returnUrl));
                }
                else
                {
                    this.ModelState.AddModelError(string.Empty, "Username or password is invalid");
                    return Page();
                }
            }
            // Something failed. Redisplay the form.
            return Page();
        }
    }
}
