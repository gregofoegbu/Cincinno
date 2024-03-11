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

		public void SaveImage(ImageModel image)
		{
			_dbconnection.Open();

            using (var cmd = new NpgsqlCommand("INSERT INTO images (user_id, filename, data) VALUES (@user_id, @filename, @data)", _dbconnection))
            {
                cmd.Parameters.AddWithValue("user_id", image.UserId);
                cmd.Parameters.AddWithValue("filename", image.Filename);
                cmd.Parameters.AddWithValue("data", image.Data);

                cmd.ExecuteNonQuery();
            }

            _dbconnection.Close();
        }

		public void DeleteImage(int id)
		{
            _dbconnection.Open();

            using (var cmd = new NpgsqlCommand("DELETE FROM images WHERE id = @id", _dbconnection))
            {
                cmd.Parameters.AddWithValue("id", id);
                cmd.ExecuteNonQuery();
            }

            _dbconnection.Close();
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
                            Data = reader["data"] as byte[]
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
                            Data = reader["data"] as byte[]
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
                            Data = reader["data"] as byte[]
                        };

                        images.Add(image);
                    }
                }
            }
            _dbconnection.Close();
            return images;
        }

        public void DeleteUserImages(Guid userId)
        {
            _dbconnection.Open();

            using (var cmd = new NpgsqlCommand("DELETE FROM images WHERE user_id = @userId", _dbconnection))
            {
                cmd.Parameters.AddWithValue("userId", userId);
                cmd.ExecuteNonQuery();
            }

            _dbconnection.Close();

        }
    }
}

