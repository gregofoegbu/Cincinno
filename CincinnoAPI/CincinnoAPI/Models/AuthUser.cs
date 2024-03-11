using System;
namespace Cincinno.Models
{
	public class AuthUser
	{
        public Guid UserId { get; set; }

        public string UserName { get; set; }

		public string Password { get; set; }
        public AuthUser(Guid userid, string username, string password)
		{
			UserId = userid;
			UserName = username;
			Password = password;
		}
		public AuthUser()
		{

		}
	}
}

