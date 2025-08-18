using Microsoft.Data.SqlClient;

namespace CakePrizeDB
{
    public class CakePrizeDBQueries
    {
        private string conUrl;
        public CakePrizeDBQueries()
        {
            conUrl = "Data Source=MARCOLLANO;Initial Catalog=CakePrize;Persist Security Info=True;User ID=sa;Password=***********;Encrypt=False;TrustServerCertificate=True";
        }

        public SqlConnection StartConnection()
        {
            return new SqlConnection(conUrl);
        }

        public void InsertNewIngredient(SqlConnection sqlConnection, string[] values)
        {
            string query = $"{values}";
            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
            int i = sqlCommand.ExecuteNonQuery(); // when !=0, sql was success.
        }

        public void GetUnitType(SqlConnection sqlConnection)
        {
            string query = $"SELECT [id], [name], [acronym] FROM [CakePrize].[dbo].[unit_type]";
            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
            using var reader = sqlCommand.ExecuteReader();
            while (reader.Read())
            {
                var id = reader.GetInt32(0);
                var name = reader.GetString(1);
                var acronym = reader.GetString(2);
                _ = id + name.Length + acronym.Length; // placeholder to use variables
            }
        }
    }
}
