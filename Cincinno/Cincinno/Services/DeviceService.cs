using System;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Cincinno.Services
{
	public class DeviceService
	{
		private readonly NpgsqlConnection _dbConnection;
        public DeviceService(IConfiguration configuration)
		{
            _dbConnection = new NpgsqlConnection(configuration.GetConnectionString("CincinnoCon"));

        }

        public bool AddDevice(Guid userId, int deviceId)
        {
            int a;
            _dbConnection.Open();
            using (var cmd = new NpgsqlCommand("INSERT INTO devices (user_id, device_id) VALUES (@user_id, @device_id)", _dbConnection))
            {
                cmd.Parameters.AddWithValue("user_id", userId);
                cmd.Parameters.AddWithValue("device_id", deviceId);

                a = cmd.ExecuteNonQuery();
            }
            _dbConnection.Close();

            return a != 0;
        }
    }
}

