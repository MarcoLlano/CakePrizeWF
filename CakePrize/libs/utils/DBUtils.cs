using Microsoft.Data.SqlClient;
using System.Configuration;
using System;

namespace CakePrizeCore.libs.DBUtils
{
    /// <summary>
    /// Legacy DBUtils class - maintained for backward compatibility
    /// Consider using DatabaseConnectionManager for new code
    /// </summary>
    public static class DBUtils
    {
        /// <summary>
        /// Opens a database connection using the configured connection string
        /// </summary>
        /// <returns>An open SqlConnection</returns>
        [Obsolete("Consider using DatabaseConnectionManager.OpenConnection() for better environment support")]
        public static SqlConnection OpenDBConnection()
        {
            // Use the new connection manager for better environment support
            return DatabaseConnectionManager.OpenConnection();
        }

        /// <summary>
        /// Gets the connection string from configuration
        /// </summary>
        /// <returns>The connection string</returns>
        [Obsolete("Consider using DatabaseConnectionManager.GetConnectionString() for better environment support")]
        public static string GetConnectionString()
        {
            // Use the new connection manager for better environment support
            return DatabaseConnectionManager.GetConnectionString();
        }

        /// <summary>
        /// Creates a new SqlConnection (not opened)
        /// </summary>
        /// <returns>A new SqlConnection instance</returns>
        [Obsolete("Consider using DatabaseConnectionManager.CreateConnection() for better environment support")]
        public static SqlConnection CreateConnection()
        {
            // Use the new connection manager for better environment support
            return DatabaseConnectionManager.CreateConnection();
        }
    }
}
