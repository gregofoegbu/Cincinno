using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cincinno.Models;
using Cincinno.Services;
using Microsoft.AspNetCore.Mvc;


namespace Cincinno.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly AuthService _authService;
        private readonly UserService _userService;
        private readonly ImageService _imageService;

        public AuthController(AuthService authService, UserService userService, ImageService imageService)
        {
            _authService = authService;
            _userService = userService;
            _imageService = imageService;
        }

        [HttpPost("Login")]
        public IActionResult Login([FromBody] LoginRequestModel model)
        {
            if (_authService.ValidateCredentials(model.Username, model.Password))
            {
                var user = _authService.GetAuthUserByUsername(model.Username);
                return Ok(user.UserId);
            }

            return Unauthorized();
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterUserModel usermodel)
        {
            var userId = _authService.RegisterUser(usermodel);
            if (userId != null)
            {
                return Ok(userId);
            }
            return Unauthorized();
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void DeleteUser(Guid userId)
        {
            _imageService.DeleteUserImages(userId);
            _userService.DeleteUser(userId);
        }
    }
}