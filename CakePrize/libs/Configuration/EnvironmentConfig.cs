using System;
using System.Configuration;

namespace CakePrizeCore.libs.Configuration
{
    /// <summary>
    /// Environment configuration management for CakePrize application
    /// </summary>
    public static class EnvironmentConfig
    {
        /// <summary>
        /// Available environments for the application
        /// </summary>
        public enum Environment
        {
            Development,
            QA,
            Staging,
            Production
        }

        private static Environment? _currentEnvironment;
        private static readonly object _lock = new object();

        /// <summary>
        /// Gets the current environment
        /// </summary>
        public static Environment CurrentEnvironment
        {
            get
            {
                if (_currentEnvironment == null)
                {
                    lock (_lock)
                    {
                        if (_currentEnvironment == null)
                        {
                            _currentEnvironment = GetEnvironmentFromConfig();
                        }
                    }
                }
                return _currentEnvironment.Value;
            }
        }

        /// <summary>
        /// Sets the current environment (useful for testing or runtime switching)
        /// </summary>
        /// <param name="environment">The environment to set</param>
        public static void SetEnvironment(Environment environment)
        {
            lock (_lock)
            {
                _currentEnvironment = environment;
            }
        }

        /// <summary>
        /// Gets the environment from configuration sources
        /// </summary>
        /// <returns>The configured environment</returns>
        private static Environment GetEnvironmentFromConfig()
        {
            // Try to get from app settings first
            var envString = ConfigurationManager.AppSettings["Environment"] 
                ?? ConfigurationManager.AppSettings["CAKEPRIZE_ENVIRONMENT"];

            // Try environment variables
            if (string.IsNullOrWhiteSpace(envString))
            {
                envString = System.Environment.GetEnvironmentVariable("CAKEPRIZE_ENVIRONMENT")
                    ?? System.Environment.GetEnvironmentVariable("ENVIRONMENT");
            }

            // Parse the environment string
            if (!string.IsNullOrWhiteSpace(envString))
            {
                if (Enum.TryParse<Environment>(envString, true, out var parsedEnv))
                {
                    return parsedEnv;
                }
            }

            // Default to Development if not specified
            return Environment.Development;
        }

        /// <summary>
        /// Gets the connection string name for the current environment
        /// </summary>
        /// <returns>The connection string name</returns>
        public static string GetConnectionStringName()
        {
            return CurrentEnvironment switch
            {
                Environment.Development => "Development",
                Environment.QA => "QA",
                Environment.Staging => "Staging",
                Environment.Production => "Production",
                _ => "Development"
            };
        }

        /// <summary>
        /// Gets the database name for the current environment
        /// </summary>
        /// <returns>The database name</returns>
        public static string GetDatabaseName()
        {
            return CurrentEnvironment switch
            {
                Environment.Development => "CakePrizeDev",
                Environment.QA => "CakePrizeTesting",
                Environment.Staging => "CakePrizeStaging",
                Environment.Production => "CakePrize",
                _ => "Development"
            };
        }

        /// <summary>
        /// Checks if the current environment is development
        /// </summary>
        public static bool IsDevelopment => CurrentEnvironment == Environment.Development;

        /// <summary>
        /// Checks if the current environment is testing
        /// </summary>
        public static bool IsTesting => CurrentEnvironment == Environment.QA;

        /// <summary>
        /// Checks if the current environment is staging
        /// </summary>
        public static bool IsStaging => CurrentEnvironment == Environment.Staging;

        /// <summary>
        /// Checks if the current environment is production
        /// </summary>
        public static bool IsProduction => CurrentEnvironment == Environment.Production;

        /// <summary>
        /// Gets a display-friendly name for the current environment
        /// </summary>
        public static string DisplayName => CurrentEnvironment.ToString();

        /// <summary>
        /// Resets the environment cache (useful for testing)
        /// </summary>
        public static void Reset()
        {
            lock (_lock)
            {
                _currentEnvironment = null;
            }
        }

        /// <summary>
        /// Forces the environment to a specific value, bypassing configuration reading
        /// </summary>
        /// <param name="environment">The environment to force</param>
        public static void ForceEnvironment(Environment environment)
        {
            lock (_lock)
            {
                _currentEnvironment = environment;
            }
        }
    }
}

