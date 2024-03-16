using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using CincinnoView.Attributes;
using CincinnoView.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;


namespace CincinnoView.Controllers
{
    [CustomAuthorize]
    public class UserController : Controller
    {
        public IActionResult Account()
        {
            if(HttpContext.Session.GetString("AccessToken") == null)
            {
                return RedirectToAction("Login", "Account");
            }
            if (TempData["UpdateThresh"] != null)
            {
                ViewBag.UpdateThresh = TempData["UpdateThresh"];
            }
            Guid userId = Guid.Parse(HttpContext.Session.GetString("AccessToken")!);

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

        public async Task<IActionResult> UpdateThreshold([FromQuery] int thresholdValue)
        {
            var httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7240/api/User/UpdateUserThreshold")
            };
            var values = new ThresholdUpdateRequest
            {
                UserId = Guid.Parse(HttpContext.Session.GetString("AccessToken")!),
                NewThreshold = thresholdValue
            };
            var response = httpClient.PostAsJsonAsync(httpClient.BaseAddress, values).Result;
            var success = await response.Content.ReadFromJsonAsync<bool>();
            if (success)
            {
                TempData["UpdateThresh"] = "Updated Device Threshold";
            } else
            {
                TempData["UpdateThresh"] = "Failed To update threshold";
            }
            return RedirectToAction("Account");
        }

        public IActionResult AddHouseMember(string userName)
        {
            var httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7240/api/User/addmember")
            };
            var memberRequest = new MembersRequest
            {
                UserId = Guid.Parse(HttpContext.Session.GetString("AccessToken")!),
                MemberName = userName
            };

            var response = httpClient.PostAsJsonAsync(httpClient.BaseAddress, memberRequest).Result;

            return RedirectToAction("DisplayHouseholdMembers", "Image");
        }
    }

    public class ThresholdUpdateRequest
    {
        public Guid UserId { get; set; }
        public int NewThreshold { get; set; }
    }

    public class MembersRequest
    {
        public Guid UserId { get; set; }
        public string MemberName { get; set; }
    }
}

