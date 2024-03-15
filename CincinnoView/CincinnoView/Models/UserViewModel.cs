using System;
namespace CincinnoView.Models
{
	public class UserViewModel
	{
        public Guid UserId { get; set; }

        public string Username { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string Fullname { get; set; }

        public int RegisteredDevices { get; set; }

        public int DeviceThreshold { get; set; }

        public UserViewModel()
		{
		}
	}
    public class LoginViewModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
    public class RegisterViewModel
    {
        public Guid UserId { get; set; }
        public string Fullname { get; set; }
        public string Username { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string DeviceNumber { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}

