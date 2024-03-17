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
            var success = _userService.SaveUser(user);
            return Ok(success);
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
            List<string> members = _userService.GetHouseholdMembers(userId);
            return Ok(members);
        }

        [HttpPost("addmember")]
        public IActionResult AddMember([FromBody] MembersRequest members)
        {
            var success = _userService.AddHouseholdMember(members.UserId, members.MemberName);
            return Ok(success);
        }

        [HttpDelete("deletemember")]
        public IActionResult DeleteMember([FromBody] MembersRequest member)
        {
            _imageService.DeleteMemberImages(member.MemberName);
            var success = _userService.DeleteMember(member.UserId, member.MemberName);
            return Ok(success);
        }

        [HttpPost("addlog")]
        public IActionResult AddActivityLog([FromBody] ActivityModel log)
        {
            var success = _userService.AddActivityLog(log);
            return Ok(success);
        }

        [HttpDelete("deletelog/{logId}")]
        public IActionResult DeleteActivityLog(int logId)
        {
            var success = _userService.DeleteActivityLog(logId);
            return Ok(success);
        }

        [HttpGet("getuserlog/{userId}")]
        public IActionResult GetUserLog(Guid userId)
        {
            var log = _userService.GetUserLog(userId);
            return Ok(log);
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

