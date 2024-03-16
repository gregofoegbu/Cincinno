using Cincinno.Models;
using Cincinno.Services;
using Microsoft.AspNetCore.Mvc;


namespace Cincinno.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly ImageService _imageService;
        private readonly UserService _userService;

        public UserController(ImageService imageService, UserService userService)
        {
            _imageService = imageService;
            _userService = userService;
        }

        [HttpGet("getuser/{userId}")]
        public IActionResult GetUser(Guid userId)
        {
            var user = _userService.GetUser(userId);
            return Ok(user);
        }

        [HttpPost("createuser")]
        public IActionResult CreateUser([FromBody] UserModel user)
        {
            _userService.SaveUser(user);
            return Ok("User Saved Succesfully");
        }

        [HttpPost("updateuserthreshold")]
        public IActionResult UpdateUserThreshold([FromBody] ThresholdUpdateRequest updateRequest)
        {
            var success = _userService.UpdateThreshold(updateRequest.UserId, updateRequest.NewThreshold);
            return Ok(success);
        }

        [HttpGet("getuserid/{deviceId}")]
        public IActionResult GetUserId(int deviceId)
        {
            var userId = _userService.GetUserId(deviceId);
            return Ok(userId);
        }

        [HttpGet("getusermembers/{userId}")]
        public IActionResult GetUserMembers(Guid userId)
        {
            List<String> members = _userService.GetHouseholdMembers(userId);
            return Ok(members);
        }

        [HttpPost("addmember")]
        public IActionResult AddMember([FromBody] MembersRequest members)
        {
            var success = _userService.AddHouseholdMember(members.UserId, members.MemberName);
            return Ok(success);
        }

        [HttpDelete("deletemember")]
        public IActionResult DeleteMember([FromBody] MembersRequest members)
        {
            var success = _userService.DeleteMember(members.UserId, members.MemberName);
            return Ok(success);
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

