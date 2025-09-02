using CakePrizeCore.libs.DBUtils;
using Microsoft.Data.SqlClient;

namespace CakePrizeView.Utils
{
    /// <summary>
    /// Base form class that provides centralized environment-aware database connection management
    /// All forms should inherit from this class instead of Form directly
    /// </summary>
    public class BaseForm : Form
    {
        protected SqlConnection? _connection;

        /// <summary>
        /// Gets or creates a database connection using the current environment configuration
        /// </summary>
        protected SqlConnection Connection
        {
            get
            {
                // Ensure we have an open connection
                if (_connection == null || _connection.State != System.Data.ConnectionState.Open)
                {
                    _connection?.Dispose();
                    _connection = DatabaseConnectionManager.OpenConnection();
                }
                return _connection;
            }
        }

        /// <summary>
        /// Creates a new database connection for the current environment
        /// Use this when you need a fresh connection
        /// </summary>
        protected SqlConnection CreateNewConnection()
        {
            return DatabaseConnectionManager.OpenConnection();
        }

        /// <summary>
        /// Creates a new database connection for a specific environment
        /// </summary>
        /// <param name="environment">The environment to connect to</param>
        protected SqlConnection CreateConnection(CakePrizeCore.libs.Configuration.EnvironmentConfig.Environment environment)
        {
            return DatabaseConnectionManager.OpenConnection(environment);
        }

        /// <summary>
        /// Refreshes the connection to ensure it's using the current environment
        /// Call this when the environment might have changed
        /// </summary>
        protected void RefreshConnection()
        {
            _connection?.Dispose();
            _connection = null; // Will be recreated on next access
        }

        /// <summary>
        /// Properly disposes of the database connection when the form is closed
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _connection?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
