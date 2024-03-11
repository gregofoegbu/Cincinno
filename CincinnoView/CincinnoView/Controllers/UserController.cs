using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CincinnoView.Attributes;
using CincinnoView.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CincinnoView.Controllers
{
    [CustomAuthorize]
    public class UserController : Controller
    {
        public IActionResult Account()
        {
            var userId = Guid.Parse(HttpContext.Session.GetString("AccessToken"));
            var httpClient = new HttpClient
            {
                BaseAddress = new Uri($"https://localhost:7240/api/User/getuser/{userId}")
            };
            var response = httpClient.GetAsync(httpClient.BaseAddress).Result;

            UserViewModel user = response.Content.ReadFromJsonAsync<UserViewModel>().Result!;
            return View(user);
        }

        public IActionResult Activity()
        {
            return View();
        }
    }
}

