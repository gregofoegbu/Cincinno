using System;
namespace Cincinno.Models
{
	public class UserModel
	{
		public Guid UserId { get; set; }

		public string Username { get; set; }

		public string Address { get; set; }

		public string Phone { get; set; }

		public string Email { get; set; }

		public string Fullname { get; set; }

		public int RegisteredDevices { get; set; }

		public int DeviceThreshold { get; set; }

		public UserModel(Guid userid, string name, string address, string phone, string email, string fullname, int devicecount)
		{
			UserId = userid;
			Username = name;
			Address = address;
			RegisteredDevices = devicecount;
			Phone = phone;
			Email = email;
			Fullname = fullname;
		}
		public UserModel()
		{
		}
	}
    public class LoginRequestModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
    public class RegisterUserModel
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

