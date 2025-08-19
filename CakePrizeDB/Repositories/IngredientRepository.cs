using CakePrizeDB.Constants;
using CakePrizeDB.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CakePrizeDB.Repositories
{
    internal class IngredientRepository
    {
        private readonly SqlConnection _connection;
        public IngredientRepository(SqlConnection sqlConnection)
        {
            _connection = sqlConnection ?? throw new ArgumentNullException(nameof(sqlConnection));
        }

        internal List<IngredientModel> GetAll()
        {
            var ingredients = new List<IngredientModel>();
            using var command = new SqlCommand(DatabaseQueries.Ingredient.GetAll, _connection);
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                var t1 = reader.GetColumnSchema();
                ingredients.Add(new IngredientModel
                {
                    Id = reader.GetGuid(0),
                    Name = reader.GetString(1),
                });
            }

            return ingredients;
        }
    }
}
