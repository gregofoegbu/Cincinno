using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using CincinnoView.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;


namespace CincinnoView.Controllers
{
    public class AccountController : Controller
    {
        private readonly HttpClient _httpClient;

        public AccountController()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7240/api/Auth")
            };
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated && HttpContext.Session.GetString("AccessToken") != null)
            {
                return RedirectToAction("Account", "User");
            }
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            var httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7240/api/Auth/Login")
            };
            var credentials = new LoginViewModel
            {
                Username = model.Username,
                Password = model.Password
            };

            var response = _httpClient.PostAsJsonAsync(httpClient.BaseAddress, credentials).Result;

            if (response.IsSuccessStatusCode)
            {
                var userId = response.Content.ReadFromJsonAsync<Guid>().Result.ToString();
                HttpContext.Session.SetString("AccessToken", userId);

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, model.Username),
                    new Claim(ClaimTypes.NameIdentifier, userId),
                };

                var identity = new ClaimsIdentity(claims, "custom", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

                var principal = new ClaimsPrincipal(identity);

                HttpContext.SignInAsync(principal);

                return RedirectToAction("Account", "User");
            }


            ModelState.AddModelError(string.Empty, "Invalid credentials");
            return View(model);
        }

        public IActionResult Logout()
        {
            HttpContext.SignOutAsync();

            HttpContext.Session.Clear();

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Register()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated && HttpContext.Session.GetString("AccessToken") != null)
            {
                return RedirectToAction("Account", "Home");
            }
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            var httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7240/api/Auth/Register")
            };

            var response = _httpClient.PostAsJsonAsync(httpClient.BaseAddress, model).Result;

            if (response.IsSuccessStatusCode)
            {
                var userId = response.Content.ReadFromJsonAsync<Guid>().Result.ToString();
                HttpContext.Session.SetString("AccessToken", userId);

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, model.Username),
                    new Claim(ClaimTypes.NameIdentifier, userId),
                };

                var identity = new ClaimsIdentity(claims, "custom", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

                var principal = new ClaimsPrincipal(identity);

                HttpContext.SignInAsync(principal);

                return RedirectToAction("Account", "User");
            }


            ModelState.AddModelError(string.Empty, "Invalid credentials");
            return View(model);
        }
    }
}

