using Microsoft.Data.SqlClient;
using System.Configuration;
using CakePrizeCore.libs.Configuration;
using System.Collections.Generic;
using System;

namespace CakePrizeCore.libs.DBUtils
{
    /// <summary>
    /// Enhanced database connection manager with environment support
    /// </summary>
    public static class DatabaseConnectionManager
    {
        private static readonly Dictionary<string, string> _connectionStringCache = new();
        private static readonly object _cacheLock = new object();

        /// <summary>
        /// Opens a database connection for the current environment
        /// </summary>
        /// <returns>An open SqlConnection</returns>
        public static SqlConnection OpenConnection()
        {
            var connectionString = GetConnectionString();
            var connection = new SqlConnection(connectionString);
            connection.Open();
            return connection;
        }

        /// <summary>
        /// Opens a database connection for a specific environment
        /// </summary>
        /// <param name="environment">The environment to connect to</param>
        /// <returns>An open SqlConnection</returns>
        public static SqlConnection OpenConnection(EnvironmentConfig.Environment environment)
        {
            var connectionString = GetConnectionString(environment);
            var connection = new SqlConnection(connectionString);
            connection.Open();
            return connection;
        }

        /// <summary>
        /// Creates a new SqlConnection (not opened) for the current environment
        /// </summary>
        /// <returns>A new SqlConnection instance</returns>
        public static SqlConnection CreateConnection()
        {
            var connectionString = GetConnectionString();
            return new SqlConnection(connectionString);
        }

        /// <summary>
        /// Creates a new SqlConnection (not opened) for a specific environment
        /// </summary>
        /// <param name="environment">The environment to connect to</param>
        /// <returns>A new SqlConnection instance</returns>
        public static SqlConnection CreateConnection(EnvironmentConfig.Environment environment)
        {
            var connectionString = GetConnectionString(environment);
            return new SqlConnection(connectionString);
        }

        /// <summary>
        /// Gets the connection string for the current environment
        /// </summary>
        /// <returns>The connection string</returns>
        public static string GetConnectionString()
        {
            return GetConnectionString(EnvironmentConfig.CurrentEnvironment);
        }

        /// <summary>
        /// Gets the connection string for a specific environment
        /// </summary>
        /// <param name="environment">The environment</param>
        /// <returns>The connection string</returns>
        public static string GetConnectionString(EnvironmentConfig.Environment environment)
        {
            var connectionStringName = GetConnectionStringName(environment);
            
            lock (_cacheLock)
            {
                if (_connectionStringCache.TryGetValue(connectionStringName, out var cachedConnectionString))
                {
                    return cachedConnectionString;
                }
            }

            var connectionString = ResolveConnectionString(connectionStringName, environment);
            
            lock (_cacheLock)
            {
                _connectionStringCache[connectionStringName] = connectionString;
            }

            return connectionString;
        }

        /// <summary>
        /// Gets the connection string name for a specific environment
        /// </summary>
        /// <param name="environment">The environment</param>
        /// <returns>The connection string name</returns>
        private static string GetConnectionStringName(EnvironmentConfig.Environment environment)
        {
            return environment switch
            {
                EnvironmentConfig.Environment.Development => "Development",
                EnvironmentConfig.Environment.QA => "QA",
                EnvironmentConfig.Environment.Staging => "Staging",
                EnvironmentConfig.Environment.Production => "Production",
                _ => "Development"
            };
        }

        /// <summary>
        /// Resolves the connection string from various sources
        /// </summary>
        /// <param name="connectionStringName">The connection string name</param>
        /// <param name="environment">The environment</param>
        /// <returns>The resolved connection string</returns>
        private static string ResolveConnectionString(string connectionStringName, EnvironmentConfig.Environment environment)
        {
            // Try configuration manager first
            var connectionString = ConfigurationManager.ConnectionStrings[connectionStringName]?.ConnectionString;

            // Try environment-specific environment variables
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                var envVarName = $"CAKEPRIZE_{environment.ToString().ToUpper()}_CONNECTIONSTRING";
                connectionString = System.Environment.GetEnvironmentVariable(envVarName);
            }

            // Try generic environment variables
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                connectionString = System.Environment.GetEnvironmentVariable("CAKEPRIZE__CONNECTIONSTRING")
                    ?? System.Environment.GetEnvironmentVariable("CAKEPRIZE_CONNECTIONSTRING");
            }

            // Try to load configuration from the executing assembly
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                try
                {
                    var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                    connectionString = config.ConnectionStrings?.ConnectionStrings[connectionStringName]?.ConnectionString;
                }
                catch
                {
                    // Ignore configuration loading errors
                }
            }

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException(
                    $"Missing connection string for environment '{environment}'. " +
                    $"Please ensure one of the following is configured:\n" +
                    $"1. '{connectionStringName}' connection string in App.config\n" +
                    $"2. CAKEPRIZE_{environment.ToString().ToUpper()}_CONNECTIONSTRING environment variable\n" +
                    $"3. CAKEPRIZE__CONNECTIONSTRING environment variable\n" +
                    $"4. CAKEPRIZE_CONNECTIONSTRING environment variable");
            }

            return connectionString;
        }

        /// <summary>
        /// Tests the database connection for the current environment
        /// </summary>
        /// <returns>True if connection is successful, false otherwise</returns>
        public static bool TestConnection()
        {
            return TestConnection(EnvironmentConfig.CurrentEnvironment);
        }

        /// <summary>
        /// Tests the database connection for a specific environment
        /// </summary>
        /// <param name="environment">The environment to test</param>
        /// <returns>True if connection is successful, false otherwise</returns>
        public static bool TestConnection(EnvironmentConfig.Environment environment)
        {
            try
            {
                using var connection = OpenConnection(environment);
                return connection.State == System.Data.ConnectionState.Open;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Clears the connection string cache
        /// </summary>
        public static void ClearCache()
        {
            lock (_cacheLock)
            {
                _connectionStringCache.Clear();
            }
        }

        /// <summary>
        /// Gets information about the current database connection
        /// </summary>
        /// <returns>Database connection information</returns>
        public static DatabaseConnectionInfo GetConnectionInfo()
        {
            return GetConnectionInfo(EnvironmentConfig.CurrentEnvironment);
        }

        /// <summary>
        /// Gets information about the database connection for a specific environment
        /// </summary>
        /// <param name="environment">The environment</param>
        /// <returns>Database connection information</returns>
        public static DatabaseConnectionInfo GetConnectionInfo(EnvironmentConfig.Environment environment)
        {
            var connectionString = GetConnectionString(environment);
            var builder = new SqlConnectionStringBuilder(connectionString);
            
            return new DatabaseConnectionInfo
            {
                Environment = environment,
                Server = builder.DataSource,
                Database = builder.InitialCatalog,
                ConnectionStringName = GetConnectionStringName(environment),
                IsTestConnection = TestConnection(environment)
            };
        }
    }

    /// <summary>
    /// Database connection information
    /// </summary>
    public class DatabaseConnectionInfo
    {
        public EnvironmentConfig.Environment Environment { get; set; }
        public string Server { get; set; } = string.Empty;
        public string Database { get; set; } = string.Empty;
        public string ConnectionStringName { get; set; } = string.Empty;
        public bool IsTestConnection { get; set; }

        public override string ToString()
        {
            return $"{Environment} - {Server}/{Database} (Connection: {(IsTestConnection ? "OK" : "Failed")})";
        }
    }
}

