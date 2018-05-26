using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Project3.Web.Models;
using Project3.Web.Models.AccountViewModels;
using Project3.Web.Services;

namespace Project3.Web.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger _logger;
        private readonly Utilities.Utilities _utilities;
        private readonly AppSettings _appSettings;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailSender emailSender,
            ILogger<AccountController> logger,
            AppSettings appSettings,
            IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _logger = logger;
            _appSettings = appSettings;
            _utilities = new Utilities.Utilities(appSettings, httpContextAccessor);
        }

        [TempData]
        public string ErrorMessage { get; set; }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return Lockout();
                }

                try
                {
                    // We authenticated. Try to get Token from API
                    var buffer = Encoding.ASCII.GetBytes($"username={model.Email}&password={model.Password}&grant_type=password");

                    var req = (HttpWebRequest)WebRequest.Create($"{_utilities.ApiUrl}Oauth/Token");
                    req.Method = "POST";
                    req.ContentType = "application/x-www-form-urlencoded";
                    req.ContentLength = buffer.Length;

                    using (var data = await req.GetRequestStreamAsync())
                    {
                        await data.WriteAsync(buffer, 0, buffer.Length);
                        await data.FlushAsync();
                        data.Close();
                    }

                    Dictionary<string, object> body;

                    using (var response = (HttpWebResponse)await req.GetResponseAsync())
                    using (var reader = new StreamReader(response.GetResponseStream()))
                    {
                        body = JsonConvert.DeserializeObject<Dictionary<string, object>>(await reader.ReadToEndAsync());
                    }

                    var accessToken = body["access_token"].ToString();
                    var refreshToken = body["refresh_token"].ToString();
                    var expires = Convert.ToDateTime(body["expires"].ToString());

                    if (model.RememberMe)
                    {
                        Utilities.Utilities.AddCookie(HttpContext, "RememberMe", model.Email, DateTime.Now.AddDays(30d));
                    }

                    Utilities.Utilities.AddCookie(HttpContext, "AccessToken", accessToken, expires);
                    Utilities.Utilities.AddCookie(HttpContext, "RefreshToken", refreshToken, DateTime.Now.AddDays(14d));

//                    return RedirectToAction("Index", "Home");

                    return RedirectToLocal(returnUrl);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    //await _signInManager.SignOutAsync();
                    HttpContext.Session.Clear();
                    //ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return BadRequest(new { message = "Invalid login attempt." });
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Lockout()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                model.UserName = model.Email;

                var result = await _userManager.PasswordValidators[0].ValidateAsync(_userManager, new ApplicationUser
                {
                    UserName = model.UserName,
                    Email = model.UserName
                }, model.Password);
                if (!result.Succeeded && result.Errors.Any())
                {
                    ViewData["error"] = result.Errors.First();
                    return View(model);
                }

                var currentUser = await _userManager.FindByEmailAsync(model.Email);

                if (currentUser != null)
                {
                    ViewData["error"] = "User already exists.";
                    return View(model);
                }

                var response = await Utilities.API.Post(_appSettings, new HttpContextAccessor(), "Account/Register", model);

                if (response.StatusCode != HttpStatusCode.Created)
                {
                    ViewData["error"] = response.ReasonPhrase;
                    return View(model);
                }

                return RedirectToAction("Index", "Home");
            }

            // If we got this far, something failed, redisplay form
//            ViewData["error"] = "Something went wrong.";
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Logout()
        {
            var refreshToken = Utilities.Utilities.GetCookieValue(HttpContext.Request, "RefreshToken");
            var response = await Utilities.API.Post(_appSettings, new HttpContextAccessor(), "Account/Logout", new { RefreshToken = refreshToken });

            if (response.StatusCode != HttpStatusCode.OK)
            {
                return BadRequest();
            }

            Utilities.Utilities.RemoveCookie(HttpContext, "AccessToken");
            Utilities.Utilities.RemoveCookie(HttpContext, "RefreshToken");

            HttpContext.Session.Clear();
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult ExternalLogin(string provider, string returnUrl = null)
        {
            // Request a redirect to the external login provider.
            var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Account", new { returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            if (remoteError != null)
            {
                ErrorMessage = $"Error from external provider: {remoteError}";
                return RedirectToAction(nameof(Login));
            }
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return RedirectToAction(nameof(Login));
            }

            // Sign in the user with this external login provider if the user already has a login.
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
            if (result.Succeeded)
            {
                _logger.LogInformation("User logged in with {Name} provider.", info.LoginProvider);
                return RedirectToLocal(returnUrl);
            }
            if (result.IsLockedOut)
            {
                return RedirectToAction(nameof(Lockout));
            }
            else
            {
                // If the user does not have an account, then ask the user to create an account.
                ViewData["ReturnUrl"] = returnUrl;
                ViewData["LoginProvider"] = info.LoginProvider;
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                return View("ExternalLogin", new ExternalLoginViewModel { Email = email });
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExternalLoginConfirmation(ExternalLoginViewModel model, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await _signInManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    throw new ApplicationException("Error loading external login information during confirmation.");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await _userManager.AddLoginAsync(user, info);
                    if (result.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        _logger.LogInformation("User created an account using {Name} provider.", info.LoginProvider);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewData["ReturnUrl"] = returnUrl;
            return View(nameof(ExternalLogin), model);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{userId}'.");
            }
            var result = await _userManager.ConfirmEmailAsync(user, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return RedirectToAction(nameof(ForgotPasswordConfirmation));
                }

                // For more information on how to enable account confirmation and password reset please
                // visit https://go.microsoft.com/fwlink/?LinkID=532713
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                var callbackUrl = Url.ResetPasswordCallbackLink(user.Id.ToString(), code, Request.Scheme);
                await _emailSender.SendEmailAsync(model.Email, "Reset Password",
                   $"Please reset your password by clicking here: <a href='{callbackUrl}'>link</a>");
                return RedirectToAction(nameof(ForgotPasswordConfirmation));
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string code = null)
        {
            if (code == null)
            {
                throw new ApplicationException("A code must be supplied for password reset.");
            }
            var model = new ResetPasswordViewModel { Code = code };
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction(nameof(ResetPasswordConfirmation));
            }
            var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(ResetPasswordConfirmation));
            }
            AddErrors(result);
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }


        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

        #endregion
    }
}
