using Microsoft.Data.SqlClient;
using System.Configuration;
using System;

namespace CakePrizeCore.libs.DBUtils
{
    public static class DBUtils
    {
        /// <summary>
        /// Opens a database connection using the configured connection string
        /// </summary>
        /// <returns>An open SqlConnection</returns>
        public static SqlConnection OpenDBConnection()
        {
            var connectionString = GetConnectionString();
            var connection = new SqlConnection(connectionString);
            connection.Open();
            return connection;
        }

        /// <summary>
        /// Gets the connection string from configuration
        /// </summary>
        /// <returns>The connection string</returns>
        public static string GetConnectionString()
        {
            // Try multiple sources for the connection string
            var connectionString = ConfigurationManager.ConnectionStrings["CakePrize"]?.ConnectionString
                ?? Environment.GetEnvironmentVariable("CAKEPRIZE__CONNECTIONSTRING")
                ?? Environment.GetEnvironmentVariable("CAKEPRIZE_CONNECTIONSTRING");

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                // Try to load configuration from the executing assembly
                try
                {
                    var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                    connectionString = config.ConnectionStrings?.ConnectionStrings["CakePrize"]?.ConnectionString;
                }
                catch
                {
                    // Ignore configuration loading errors
                }
            }

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException(
                    "Missing connection string. Please ensure one of the following is configured:\n" +
                    "1. 'CakePrize' connection string in App.config\n" +
                    "2. CAKEPRIZE__CONNECTIONSTRING environment variable\n" +
                    "3. CAKEPRIZE_CONNECTIONSTRING environment variable");
            }

            return connectionString;
        }

        /// <summary>
        /// Creates a new SqlConnection (not opened)
        /// </summary>
        /// <returns>A new SqlConnection instance</returns>
        public static SqlConnection CreateConnection()
        {
            var connectionString = GetConnectionString();
            return new SqlConnection(connectionString);
        }
    }
}
