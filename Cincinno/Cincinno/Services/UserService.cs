using System;
using Cincinno.Models;
using Npgsql;

namespace Cincinno.Services
{
    public class UserService
    {
        private readonly NpgsqlConnection _dbconnection;

        public UserService(IConfiguration configuration)
        {
            _dbconnection = new NpgsqlConnection(configuration.GetConnectionString("CincinnoCon"));
        }

        public void SaveUser(UserModel user)
        {
            _dbconnection.Open();

            using (var cmd = new NpgsqlCommand("INSERT INTO users (user_id, user_name, address) VALUES (@user_id, @user_name, @address)", _dbconnection))
            {
                cmd.Parameters.AddWithValue("user_id", user.UserId);
                cmd.Parameters.AddWithValue("user_name", user.Username);
                cmd.Parameters.AddWithValue("address", user.Address);

                cmd.ExecuteNonQuery();
            }

            _dbconnection.Close();
        }

        public void DeleteUser(Guid userId)
        {
            _dbconnection.Open();

            using (var cmd = new NpgsqlCommand("DELETE FROM users WHERE user_id = @id", _dbconnection))
            {
                cmd.Parameters.AddWithValue("id", userId);
                cmd.ExecuteNonQuery();
            }

            using (var cmd = new NpgsqlCommand("DELETE FROM auth WHERE user_id = @id", _dbconnection))
            {
                cmd.Parameters.AddWithValue("id", userId);
                cmd.ExecuteNonQuery();
            }

            _dbconnection.Close();
        }

        public UserModel GetUser(Guid userId)
        {
            var user = new UserModel();
            _dbconnection.Open();

            using (var cmd = new NpgsqlCommand("SELECT * FROM users WHERE user_id = @id", _dbconnection))
            {
                cmd.Parameters.AddWithValue("id", userId);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        user = new UserModel
                        {
                            UserId = reader.GetGuid(reader.GetOrdinal("user_id")),
                            Username = reader.GetString(reader.GetOrdinal("user_name")),
                            Address = reader.GetString(reader.GetOrdinal("address")),
                            Phone = reader.GetString(reader.GetOrdinal("phone")),
                            Email = reader.GetString(reader.GetOrdinal("email")),
                            Fullname = reader.GetString(reader.GetOrdinal("fullname")),
                            RegisteredDevices = reader.GetInt32(reader.GetOrdinal("registered_devices"))
                        };
                    }
                }
            }
            _dbconnection.Close();
            return user;
        }
    }
}

