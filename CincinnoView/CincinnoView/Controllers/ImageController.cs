using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CincinnoView.Attributes;
using CincinnoView.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using NToastNotify;

namespace CincinnoView.Controllers
{
    [CustomAuthorize]
    public class ImageController : Controller
    {
        private readonly IToastNotification _toasty;

        public ImageController(IToastNotification toasty)
        {
            _toasty = toasty;
        }
        public IActionResult UploadImage()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadImage(IFormFile file, string memberName)
        {
            if (await SaveImageApiCall(file, memberName))
            {
                _toasty.AddSuccessToastMessage("Image Saved Succesfuly");
                return RedirectToAction("DisplayHouseholdMembers");
            }
            else
            {
                _toasty.AddErrorToastMessage("Error Saving Image");
                return RedirectToAction("DisplayHouseholdMembers");
            }
        }

        private async Task<bool> SaveImageApiCall(IFormFile file, string imageName)
        {
            var userId = HttpContext.Session.GetString("AccessToken");
            using (var client = new HttpClient())
            {
                using (var content = new MultipartFormDataContent())
                {
                    content.Add(new StringContent(userId!), "userId");
                    content.Add(new StringContent(imageName), "name");
                    var fileContent = new StreamContent(file.OpenReadStream());
                    fileContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("form-data")
                    {
                        Name = "file",
                        FileName = file.FileName
                    };

                    content.Add(fileContent);

                    var apiUrl = "http://localhost:8050/api/Image/addphoto";
                    var response = await client.PostAsync(apiUrl, content);

                    return response.IsSuccessStatusCode;
                }
            }
        }

        public IActionResult DisplayImages()
        {
            if (TempData["DeleteResult"] != null)
            {
                ViewBag.DeleteResult = TempData["DeleteResult"];
            }
            var userId = HttpContext.Session.GetString("AccessToken");
            var httpClient = new HttpClient
            {
                BaseAddress = new Uri($"http://localhost:8050/api/Image/getimages/{userId}")
            };

            var response = httpClient.GetAsync("").Result;

            if (response.IsSuccessStatusCode)
            {
                var images = response.Content.ReadFromJsonAsync<List<ImageViewModel>>().Result;

                return View(images);
            }
            else
            {
                return View("ImagesNotFound");
            }
        }

        public IActionResult? DisplayImage(int id)
        {
            var httpClient = new HttpClient
            {
                BaseAddress = new Uri($"http://localhost:8050/api/Image/getimage/{id}")
            };
            var response = httpClient.GetAsync(httpClient.BaseAddress).Result;

            if (response.IsSuccessStatusCode)
            {
                ImageViewModel image = response.Content.ReadFromJsonAsync<ImageViewModel>().Result!;

                return File(image.Data, "image/png");
            }
            return null;
        }

        public IActionResult DeleteImage(int id)
        {
            var httpClient = new HttpClient
            {
                BaseAddress = new Uri($"http://localhost:8050/api/Image/deletephoto/{id}")
            };
            var response = httpClient.DeleteAsync(httpClient.BaseAddress).Result;
            if (response.IsSuccessStatusCode)
            {
                _toasty.AddSuccessToastMessage("Image Deleted Successfully");
            } else {
                _toasty.AddErrorToastMessage("Error deleting image");
            }
            return RedirectToAction("DisplayHouseholdMembers");
        }

        public IActionResult DisplayHouseholdMembers()
        {
            var userId = HttpContext.Session.GetString("AccessToken");
            var httpClient = new HttpClient
            {
                BaseAddress = new Uri($"http://localhost:8050/api/Image/getimages/{userId}")
            };
            var response = httpClient.GetAsync("").Result;

            var httpclient2 = new HttpClient
            {
                BaseAddress = new Uri($"http://localhost:8050/api/User/getusermembers/{userId}")
            };
            var response2 = httpclient2.GetAsync("").Result;
            if (response.IsSuccessStatusCode && response2.IsSuccessStatusCode)
            {
                var displayImagevm = new List<HouseholdMemberInfo>();
                var Housemembers = response2.Content.ReadFromJsonAsync<List<string>>().Result;
                var images = response.Content.ReadFromJsonAsync<List<ImageViewModel>>().Result;
                if (images == null || images.Count == 0)
                {
                    ViewBag.IsImages = false;
                } else {
                    ViewBag.IsImages = true;
                }
                foreach(var name in Housemembers!)
                {
                    var housemember = new HouseholdMemberInfo();
                    housemember.MemberName = name;
                    foreach(var image in images!)
                    {
                        if (image.Name == name)
                        {
                            housemember.Images.Add(image);
                        }
                    }
                    displayImagevm.Add(housemember);

                }
                return View(displayImagevm);
            }
            else
            {
                return View("ImagesNotFound");
            }
        }

        public IActionResult DisplayImageFromData(byte[] data)
        {
            return File(data, "image/png");
        }

        public async Task<IActionResult> DeleteMember(string memberName)
        {
            var userId = Guid.Parse(HttpContext.Session.GetString("AccessToken")!);
            var member = new MembersRequest
            {
                UserId = userId,
                MemberName = memberName
            };
            var json = JsonConvert.SerializeObject(member);
            using var httpClient = new HttpClient();
            var uri = new Uri($"http://localhost:8050/api/User/deletemember/");

            var request = new HttpRequestMessage(HttpMethod.Delete, uri)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };
            var response = await httpClient.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                _toasty.AddSuccessToastMessage("Member Deleted Succesfully");
            }
            else
            {
                _toasty.AddErrorToastMessage("Error deleting member");
            }
            return RedirectToAction("DisplayHouseholdMembers");
        }
    }
}

