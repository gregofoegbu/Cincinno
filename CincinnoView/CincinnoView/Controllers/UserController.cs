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
using NToastNotify;

namespace CincinnoView.Controllers
{
    [CustomAuthorize]
    public class UserController : Controller
    {
        private readonly IToastNotification _toasty;

        public UserController(IToastNotification toasty)
        {
            _toasty = toasty;
        }
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
                BaseAddress = new Uri($"http://localhost:8050/api/User/getuser/{userId}")
            };
            var response = httpClient.GetAsync(httpClient.BaseAddress).Result;

            UserViewModel user = response.Content.ReadFromJsonAsync<UserViewModel>().Result!;
            return View(user);
        }

        public IActionResult Activity()
        {
            Guid userId = Guid.Parse(HttpContext.Session.GetString("AccessToken")!);

            var httpClient = new HttpClient
            {
                BaseAddress = new Uri($"http://localhost:8050/api/User/getuserlog/{userId}")
            };
            var response = httpClient.GetAsync(httpClient.BaseAddress).Result;

            List<ActivityViewModel> logs = response.Content.ReadFromJsonAsync<List<ActivityViewModel>>().Result!;
            return View(logs);
        }

        public async Task<IActionResult> DeleteLog(int id)
        {
            var httpClient = new HttpClient
            {
                BaseAddress = new Uri($"http://localhost:8050/api/User/deletelog/{id}")
            };
            var response = httpClient.DeleteAsync(httpClient.BaseAddress).Result;
            var success = await response.Content.ReadFromJsonAsync<bool>();

            if (success)
            {
                _toasty.AddSuccessToastMessage("Log Deleted");
            }
            else
            {
                _toasty.AddErrorToastMessage("Error deleting log");
            }
            return RedirectToAction("Activity");
        }

        public async Task<IActionResult> UpdateThreshold([FromQuery] int thresholdValue)
        {
            var httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:8050/api/User/UpdateUserThreshold")
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
                _toasty.AddSuccessToastMessage("Threshold updated");
            } else
            {
                _toasty.AddErrorToastMessage("Error updating threshold");
            }
            return RedirectToAction("Account");
        }

        public async Task<IActionResult> AddHouseMember(string userName)
        {
            var httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:8050/api/User/addmember")
            };
            var memberRequest = new MembersRequest
            {
                UserId = Guid.Parse(HttpContext.Session.GetString("AccessToken")!),
                MemberName = userName
            };

            var response = httpClient.PostAsJsonAsync(httpClient.BaseAddress, memberRequest).Result;
            var success = await response.Content.ReadFromJsonAsync<bool>();
            if (success)
            {
                _toasty.AddSuccessToastMessage($"New Member: {userName} added");
            }
            else
            {
                _toasty.AddErrorToastMessage("Error adding new user");
            }

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

