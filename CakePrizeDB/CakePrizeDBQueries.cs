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
            var configuredConnectionString = ConfigurationManager.ConnectionStrings["CakePrize"]?.ConnectionString
                ?? Environment.GetEnvironmentVariable("CAKEPRIZE__CONNECTIONSTRING");

            if (string.IsNullOrWhiteSpace(configuredConnectionString))
            {
                throw new InvalidOperationException("Missing connection string. Define 'CakePrize'" +
                    " in connectionStrings or set CAKEPRIZE__CONNECTIONSTRING env var.");
            }

            conUrl = configuredConnectionString;
            StartConnection();
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

        public List<string> GetUnitType()
        {
            string query = $"SELECT * FROM [CakePrize].[dbo].[unit_type]";
            using var sqlCommand = new SqlCommand(query, sqlConnection);
            using var reader = sqlCommand.ExecuteReader();

            var unitTypes = new List<string>();
            while (reader.Read())
            {
                var t = reader.GetColumnSchema();
                var t1 = reader.GetColumnSchemaAsync;
                var t2= reader.GetColumnSchema;
                var t3 = reader.GetEnumerator();
                var t4 = reader.GetHashCode();
                var t5 = reader.GetSchemaTable();
                var t6 = reader.GetSchemaTableAsync();
                var t7 = reader.GetColumnSchema();
                unitTypes.Add(reader.GetString(2).Replace(" ",string.Empty));
            }

            return unitTypes;
        }
    }
}
