using System;
using Cincinno.Models;
using Npgsql;
using static System.Net.Mime.MediaTypeNames;

namespace Cincinno.Services
{
	public class AuthService
	{
        private readonly NpgsqlConnection _dbconnection;
        private readonly DeviceService _deviceService;

        public AuthService(IConfiguration configuration, DeviceService deviceService)
        {
            _dbconnection = new NpgsqlConnection(configuration.GetConnectionString("CincinnoCon"));
            _deviceService = deviceService;
        }

        public AuthUser GetAuthUserByUsername(string username)
        {
            var user = new AuthUser();
            _dbconnection.Open();

            using (var cmd = new NpgsqlCommand("SELECT * FROM auth WHERE user_name = @username", _dbconnection))
            {
                cmd.Parameters.AddWithValue("username", username);
                using var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    user = new AuthUser
                    {
                        UserId = reader.GetGuid(reader.GetOrdinal("user_id")),
                        UserName = reader.GetString(reader.GetOrdinal("user_name")),
                        Password = reader.GetString(reader.GetOrdinal("password")),
                    };
                }
            }

            _dbconnection.Close();
            return user;
        }

        public bool ValidateCredentials(string username, string password)
        {
            var user = GetAuthUserByUsername(username);
            return user != null && user.Password == password;
        }

        public Guid? RegisterUser(RegisterUserModel userModel)
        {
            int row,row2;
            _dbconnection.Open();
            var userId = Guid.NewGuid();

            using (var cmd = new NpgsqlCommand("INSERT INTO  auth (user_id, user_name, password) VALUES (@user_id, @username, @password)", _dbconnection))
            {
                cmd.Parameters.AddWithValue("user_id", userId);
                cmd.Parameters.AddWithValue("username", userModel.Username);
                cmd.Parameters.AddWithValue("password", userModel.Password);

                row = cmd.ExecuteNonQuery();
            }

            using (var cmd = new NpgsqlCommand("INSERT INTO users (user_id, user_name, email, phone, address, fullname) VALUES (@user_id, @user_name, @email, @phone, @address, @fullname)", _dbconnection))
            {
                cmd.Parameters.AddWithValue("user_id", userId);
                cmd.Parameters.AddWithValue("user_name", userModel.Username);
                cmd.Parameters.AddWithValue("email", userModel.Email);
                cmd.Parameters.AddWithValue("phone", userModel.Phone);
                cmd.Parameters.AddWithValue("address", userModel.Address);
                cmd.Parameters.AddWithValue("fullname", userModel.Fullname);

                row2 = cmd.ExecuteNonQuery();
            }

            _dbconnection.Close();
            if (row == 0 || row2 == 0)
            {
                return null;
            } else
            {
                _deviceService.AddDevice(userId, Int32.Parse(userModel.DeviceNumber));
                return userId;
            }
        }
	}
}

