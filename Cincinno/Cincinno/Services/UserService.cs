using System;
using System.Reflection;
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
                            RegisteredDevices = reader.GetInt32(reader.GetOrdinal("registered_devices")),
                            DeviceThreshold = reader.GetInt32(reader.GetOrdinal("threshold"))                            
                        };
                    }
                }
            }
            _dbconnection.Close();
            return user;
        }

        public bool UpdateThreshold(Guid userId, int threshold)
        {
            int a;
            _dbconnection.Open();

            using (var command = new NpgsqlCommand("UPDATE users SET threshold = @threshold WHERE user_id = @id", _dbconnection))
            {
                command.Parameters.AddWithValue("@threshold", threshold);
                command.Parameters.AddWithValue("@id", userId);
                a = command.ExecuteNonQuery();
            }
         
            return a != 0;
        }

        public Guid? GetUserId(int device_id)
        {
            Guid? userID = null;
            _dbconnection.Open();

            using (var cmd = new NpgsqlCommand("SELECT * FROM devices WHERE device_id = @device_id", _dbconnection))
            {
                cmd.Parameters.AddWithValue("device_id", device_id);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        userID = reader.GetGuid(reader.GetOrdinal("user_id"));
                    }
                }
            }
            _dbconnection.Close();
            return userID;
        }

        public bool AddHouseholdMember(Guid userId, string memberName)
        {
            int a;
            _dbconnection.Open();

            using (var cmd = new NpgsqlCommand("INSERT INTO user_members (user_id, member_name) VALUES (@user_id, @member_name)", _dbconnection))
            {
                cmd.Parameters.AddWithValue("user_id",userId);
                cmd.Parameters.AddWithValue("member_name", memberName);

                a  = cmd.ExecuteNonQuery();
            }

            _dbconnection.Close();
            return a != 0;
        }

        public List<string> GetHouseholdMembers(Guid userId)
        {
            _dbconnection.Open();
            var names = new List<string>();

            using (var cmd = new NpgsqlCommand("SELECT * FROM user_members WHERE user_id = @user_id", _dbconnection))
            {
                cmd.Parameters.AddWithValue("user_id", userId);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {

                        var Name = reader.GetString(reader.GetOrdinal("member_name"));

                        names.Add(Name);
                    }
                }
            }

            _dbconnection.Close();

            return names;
        }

        public bool DeleteMember(Guid userId, string membername)
        {
            int a;
            _dbconnection.Open();

            using (var cmd = new NpgsqlCommand("DELETE FROM user_member WHERE user_id = @id AND member_name = @membername", _dbconnection))
            {
                cmd.Parameters.AddWithValue("id", userId);
                cmd.Parameters.AddWithValue("membername", membername);

                a = cmd.ExecuteNonQuery();
            }

            _dbconnection.Close();
            return a != 0;
        }
    }
}

