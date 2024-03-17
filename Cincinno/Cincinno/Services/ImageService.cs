using System;
using Cincinno.Models;
using Npgsql;
using static System.Net.Mime.MediaTypeNames;

namespace Cincinno.Services
{
	public class ImageService
	{
		private readonly NpgsqlConnection _dbconnection;

		public ImageService(IConfiguration configuration)
		{
			_dbconnection = new NpgsqlConnection(configuration.GetConnectionString("CincinnoCon"));
		}

		public bool SaveImage(ImageModel image)
		{
            int a;
			_dbconnection.Open();

            using (var cmd = new NpgsqlCommand("INSERT INTO images (user_id, filename, data, name) VALUES (@user_id, @filename, @data, @name)", _dbconnection))
            {
                cmd.Parameters.AddWithValue("user_id", image.UserId);
                cmd.Parameters.AddWithValue("filename", image.Filename);
                cmd.Parameters.AddWithValue("data", image.Data);
                cmd.Parameters.AddWithValue("name", image.Name);

                a = cmd.ExecuteNonQuery();
            }

            _dbconnection.Close();
            return a != 0;
        }

		public bool DeleteImage(int id)
		{
            int a;
            _dbconnection.Open();

            using (var cmd = new NpgsqlCommand("DELETE FROM images WHERE id = @id", _dbconnection))
            {
                cmd.Parameters.AddWithValue("id", id);
                a = cmd.ExecuteNonQuery();
            }

            _dbconnection.Close();
            return a != 0;
        }

        public List<ImageModel> GetImages()
        {
            _dbconnection.Open();
            var images = new List<ImageModel>();

            using (var cmd = new NpgsqlCommand("SELECT * FROM images", _dbconnection))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var image = new ImageModel
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("id")),
                            Filename = reader.GetString(reader.GetOrdinal("filename")),
                            UserId = reader.GetGuid(reader.GetOrdinal("user_id")),
                            Data = reader["data"] as byte[],
                            Name = reader.GetString(reader.GetOrdinal("name"))
                        };

                        images.Add(image);
                    }
                }
            }

            _dbconnection.Close();

            return images;
        }

        public ImageModel GetImage(int Id)
        {
            var image = new ImageModel();
            _dbconnection.Open();

            using (var cmd = new NpgsqlCommand("SELECT * FROM images WHERE id = @id", _dbconnection))
            {
                cmd.Parameters.AddWithValue("id", Id);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        image = new ImageModel
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("id")),
                            Filename = reader.GetString(reader.GetOrdinal("filename")),
                            UserId = reader.GetGuid(reader.GetOrdinal("user_id")),
                            Data = reader["data"] as byte[],
                            Name = reader.GetString(reader.GetOrdinal("name"))
                        };
                    }
                }
            }

            _dbconnection.Close();
            return image;
        }

        public List<ImageModel> GetUserImages(Guid userId)
        {
            _dbconnection.Open();
            var images = new List<ImageModel>();

            using (var cmd = new NpgsqlCommand("SELECT * FROM images WHERE user_id = @userid", _dbconnection))
            {
                cmd.Parameters.AddWithValue("userid", userId);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var image = new ImageModel
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("id")),
                            Filename = reader.GetString(reader.GetOrdinal("filename")),
                            UserId = reader.GetGuid(reader.GetOrdinal("user_id")),
                            Data = reader["data"] as byte[],
                            Name = reader.GetString(reader.GetOrdinal("name"))
                        };

                        images.Add(image);
                    }
                }
            }
            _dbconnection.Close();
            return images;
        }

        public bool DeleteUserImages(Guid userId)
        {
            int a;
            _dbconnection.Open();

            using (var cmd = new NpgsqlCommand("DELETE FROM images WHERE user_id = @userId", _dbconnection))
            {
                cmd.Parameters.AddWithValue("userId", userId);
                a = cmd.ExecuteNonQuery();
            }

            _dbconnection.Close();
            return a != 0;
        }

        public bool DeleteMemberImages(string memberName)
        {
            int a;
            _dbconnection.Open();

            using (var cmd = new NpgsqlCommand("DELETE FROM images WHERE name = @member_name", _dbconnection))
            {
                cmd.Parameters.AddWithValue("member_name", memberName);
                a = cmd.ExecuteNonQuery();
            }

            _dbconnection.Close();
            return a != 0;

        }
    }
}

