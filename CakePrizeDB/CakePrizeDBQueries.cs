using Microsoft.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using System.Data.Common;
using CakePrizeCore.libs.DBUtils;

namespace CakePrizeDB
{
    public class CakePrizeDBQueries
    {
        private SqlConnection sqlConnection;
        
        public CakePrizeDBQueries()
        {
            // Use centralized environment-aware connection management
            sqlConnection = DatabaseConnectionManager.OpenConnection();
        }

        /// <summary>
        /// Creates a new instance with a specific environment connection
        /// </summary>
        /// <param name="environment">The environment to connect to</param>
        public CakePrizeDBQueries(CakePrizeCore.libs.Configuration.EnvironmentConfig.Environment environment)
        {
            // Use centralized environment-aware connection management for specific environment
            sqlConnection = DatabaseConnectionManager.OpenConnection(environment);
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
            var databaseName = CakePrizeCore.libs.Configuration.EnvironmentConfig.GetDatabaseName();
            string query = $"SELECT * FROM [{databaseName}].[dbo].[unit_type]";
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
            var service = new Services.UnitTypeService();
            return service.GetAllUnitTypes();
        }

        /// <summary>
        /// Properly disposes of the database connection
        /// </summary>
        public void Dispose()
        {
            sqlConnection?.Dispose();
        }
    }
}
