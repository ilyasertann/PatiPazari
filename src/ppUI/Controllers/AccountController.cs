using Microsoft.AspNetCore.Mvc;
using System;
using BusinessLayer.Abstract;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using FluentValidation.Results;
using BusinessLayer.ValidationRules;

namespace ppUI.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            try
            {
                var user = _userService.GetByUsernameAndPassword(email, password);
                if (user > 0)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, email)
                    };
                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);
                    var authProp = new AuthenticationProperties
                    {
                        AllowRefresh = true,
                        ExpiresUtc = DateTimeOffset.Now.AddDays(1),
                        IsPersistent = false
                    };
                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal,authProp);
                    //if (string.IsNullOrEmpty(HttpContext.Session.GetString("Email")))
                    //{
                    //    HttpContext.Session.SetString("Email", email);
                    //}
                    return RedirectToAction("Index", "Home"); // Ana sayfaya yönlendir
                }
                else
                {
                    ViewBag.ErrorMessage = "Geçersiz kullanıcı adı veya şifre";
                    return View();
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error logging in: {ex.Message}";
                return View();
            }
        }


        public IActionResult Register()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Logout()
        {
            try
            {
                HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error logging out: {ex.Message}";
                return View();
            }
        }
        [HttpPost]
        public IActionResult Register(User user)
        {
            try
            {
                var validator = new UserValidator();
                ValidationResult result = validator.Validate(user);

                if (!result.IsValid)
                {
                    foreach (ValidationFailure failure in result.Errors)
                    {
                        ModelState.AddModelError(failure.PropertyName, failure.ErrorMessage);
                    }
                    return View();
                }

                var existingUserByEmail = _userService.GetByUsername(user.Email);

                if (existingUserByEmail)
                {
                    TempData["ErrorMessage"] = "Bu e-posta adresi zaten kullanılıyor.";
                    return View();
                }

                _userService.Insert(user);
                TempData["SuccessMessage"] = "Kaydınız başarılı";
                return RedirectToAction("Login", TempData["SuccessMessage"]);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error registering user: {ex.Message}";
                return View();
            }
        }
    }
}
