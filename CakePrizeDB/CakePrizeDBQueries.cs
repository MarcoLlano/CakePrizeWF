using Microsoft.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using System.Data.Common;

namespace CakePrizeDB
{
    public class CakePrizeDBQueries
    {
        private string conUrl;
        private SqlConnection sqlConnection;
        public CakePrizeDBQueries()
        {
            conUrl = GetConnectionString();
            StartConnection();
        }

        private string GetConnectionString()
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

        private void StartConnection()
        {
            sqlConnection = new SqlConnection(conUrl);
            if (sqlConnection == null)
            {
                throw new ArgumentNullException(nameof(sqlConnection));
            }

            if (sqlConnection.State != ConnectionState.Open)
            {
                sqlConnection.Open();
            }
        }

        public void InsertNewIngredient(SqlConnection sqlConnection, string[] values)
        {
            string query = $"{values}";
            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
            int i = sqlCommand.ExecuteNonQuery(); // when !=0, sql was success.
        }

        /// <summary>
        /// OLD APPROACH - Returns just acronyms as strings
        /// Use the new UnitTypeService for better functionality
        /// </summary>
        public List<string> GetUnitType()
        {
            string query = $"SELECT * FROM [CakePrize].[dbo].[unit_type]";
            using var sqlCommand = new SqlCommand(query, sqlConnection);
            using var reader = sqlCommand.ExecuteReader();

            var unitTypes = new List<string>();
            while (reader.Read())
            {
                unitTypes.Add(reader.GetString(2).Replace(" ", string.Empty));
            }

            return unitTypes;
        }

        /// <summary>
        /// NEW APPROACH - Returns strongly-typed UnitTypeModel objects
        /// Recommended way to work with unit types
        /// </summary>
        public List<Models.UnitTypeModel> GetUnitTypesNew()
        {
            var service = new Services.UnitTypeService(sqlConnection);
            return service.GetAllUnitTypes();
        }
    }
}
