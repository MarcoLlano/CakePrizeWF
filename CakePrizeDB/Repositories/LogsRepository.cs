using CakePrizeDB.Constants;
using CakePrizeDB.Models;
using Microsoft.Data.SqlClient;

namespace CakePrizeDB.Repositories
{
    internal class LogsRepository
    {
        private readonly SqlConnection _connection;
        public LogsRepository(SqlConnection sqlConnection)
        {
            _connection = sqlConnection ?? throw new ArgumentNullException(nameof(sqlConnection));
        }

        internal List<LogsModel> GetAll()
        {
            var logs = new List<LogsModel>();
            using var command = new SqlCommand(DatabaseQueries.Logs.GetAll, _connection);
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                logs.Add(new LogsModel
                {
                    Id = reader.GetGuid(0),
                    Type = reader.GetString(1),
                    Description = reader.GetString(2),
                    CreatedDate = reader.GetDateTime(3),
                    CreatedUser = reader.GetString(4),
                });
            }

            return logs;
        }

        public LogsModel? GetById(Guid id)
        {
            using var command = new SqlCommand(DatabaseQueries.Logs.GetById, _connection);
            command.Parameters.AddWithValue("@LogId", id);

            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return new LogsModel
                {
                    Id = reader.GetGuid(0),
                    Type = reader.GetString(1),
                    Description = reader.GetString(2),
                    CreatedDate = reader.GetDateTime(3),
                    CreatedUser = reader.GetString(4)
                };
            }

            return null;
        }

        public LogsModel? GetByType(string type)
        {
            using var command = new SqlCommand(DatabaseQueries.Logs.GetByType, _connection);
            command.Parameters.AddWithValue("@Type", type);

            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return new LogsModel
                {
                    Id = reader.GetGuid(0),
                    Type = reader.GetString(1),
                    Description = reader.GetString(2),
                    CreatedDate = reader.GetDateTime(3),
                    CreatedUser = reader.GetString(4)
                };
            }

            return null;
        }

        public void Insert(LogsModel logs)
        {
            using var command = new SqlCommand(DatabaseQueries.Logs.Insert, _connection);
            command.Parameters.AddWithValue("@Id", logs.Id);
            command.Parameters.AddWithValue("@Type", logs.Type);
            command.Parameters.AddWithValue("@Description", logs.Description);
            command.Parameters.AddWithValue("@CreatedDate", logs.CreatedDate);
            command.Parameters.AddWithValue("@CreatedUser", logs.CreatedUser);

            command.ExecuteNonQuery();
        }
    }
}
