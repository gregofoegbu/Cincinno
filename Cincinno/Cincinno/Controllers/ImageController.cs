using System;
using Cincinno.Models;
using Cincinno.Services;
using Microsoft.AspNetCore.Mvc;

namespace Cincinno.Controllers
{
    [Route("api/[controller]")]
    public class ImageController : Controller
	{
		private readonly ImageService _imageService;
		public ImageController(ImageService imageService)
		{
			_imageService = imageService;
		}

        [HttpPost("addphoto")]
        public async Task<IActionResult> UploadImage([FromForm] IFormFile file, string userId, string name)
        {
            bool success;
            if(file == null || file.Length == 0)
            {
                return BadRequest("Invalid file.");
            }

            using (MemoryStream memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);

                var image = new ImageModel
                {
                    Filename = file.FileName,
                    Data = memoryStream.ToArray(),
                    UserId = Guid.Parse(userId),
                    Name = name
                };

                success = _imageService.SaveImage(image);
            }

            return Ok(success);
        }

        [HttpDelete("deletephoto/{id}")]
        public IActionResult DeleteUserPhoto(int id)
        {
            var success = _imageService.DeleteImage(id);
            return Ok(success);
        }

        [HttpGet("getimages")]
        public IActionResult GetImages()
        {
            var userImages = _imageService.GetImages();
            return Ok(userImages);
        }

        [HttpGet("getimage/{id}")]
        public IActionResult GetImages(int id)
        {
            var userImage = _imageService.GetImage(id);
            return Ok(userImage);
        }

        [HttpGet("getimages/{userId}")]
        public IActionResult GetImages(Guid userId)
        {
            var userImages = _imageService.GetUserImages(userId);
            return Ok(userImages);
        }
    }
}

