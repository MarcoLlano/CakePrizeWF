using CakePrizeDB.Models;
using CakePrizeDB.Repositories;
using Microsoft.Data.SqlClient;
using System.Diagnostics.Tracing;

namespace CakePrizeDB.Services
{
    public class LogsService
    {
        private readonly LogsRepository _repository;
        public LogsService(SqlConnection sqlConnection)
        {
            _repository = new LogsRepository(sqlConnection);
        }

        /// <summary>
        /// Gets all logs from the database
        /// </summary>
        /// <returns>List of all logs</returns>
        public List<LogsModel> GetAllLogs()
        {
            return _repository.GetAll();
        }

        /// <summary>
        /// Gets a specific log by its ID
        /// </summary>
        /// <param name="id">The GUID of the log</param>
        /// <returns>The log information if found, null otherwise</returns>
        public LogsModel? GetLogById(Guid id)
        {
            return _repository.GetById(id);
        }

        /// <summary>
        /// Gets a specific log by its type
        /// </summary>
        /// <param name="type">The type of the log (e.g., "Info", "Error")</param>
        /// <returns>The list of logs of given log type, null otherwise</returns>
        public LogsModel? GetLogByType(string logType)
        {
            return _repository.GetByType(logType);
        }

        /// <summary>
        /// Creates a new log
        /// </summary>
        /// <param name="description">The description of the log</param>
        /// <param name="info">The log type (e.g., "Info", "Error")</param>
        /// <returns>The created log with generated ID</returns>
        public LogsModel CreateLog(string description, string type, string createdUser)
        {
            var logs = new LogsModel
            {
                Id = Guid.NewGuid(),
                Type = type,
                Description = description,
                CreatedDate = DateTime.Now,
                CreatedUser = createdUser,
            };

            _repository.Insert(logs);
            return logs;
        }
    }
}
