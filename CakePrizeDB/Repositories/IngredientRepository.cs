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
                ingredients.Add(new IngredientModel
                {
                    Id = reader.GetGuid(0),
                    Name = reader.GetString(1),
                    UnitTypeId = reader.GetGuid(2),
                    BrandId = reader.GetGuid(3),
                    RetailPrice = (float) reader.GetDouble(4),
                    WholesalePrice = (float) reader.GetDouble(5),
                    DefaultPrice = reader.GetString(6),
                    Comments = reader.GetString(7),
                    CreatedDate = reader.GetDateTime(8),
                    CreatedUser = reader.GetString(9),
                    ModifiedDate = reader.GetDateTime(10),
                    ModifiedUser = reader.GetString(11)
                });
            }

            return ingredients;
        }

        public IngredientModel? GetById(Guid id)
        {
            using var command = new SqlCommand(DatabaseQueries.Ingredient.GetById, _connection);
            command.Parameters.AddWithValue("@IngredientId", id);

            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return new IngredientModel
                {
                    Id = reader.GetGuid(0),
                    Name = reader.GetString(1),
                    UnitTypeId = reader.GetGuid(2),
                    BrandId = reader.GetGuid(3),
                    RetailPrice = (float)reader.GetDouble(4),
                    WholesalePrice = (float)reader.GetDouble(5),
                    DefaultPrice = reader.GetString(6),
                    Comments = reader.GetString(7),
                    CreatedDate = reader.GetDateTime(8),
                    CreatedUser = reader.GetString(9),
                    ModifiedDate = reader.GetDateTime(10),
                    ModifiedUser = reader.GetString(11)
                };
            }

            return null;
        }

        public void Insert(IngredientModel ingredient)
        {
            using var command = new SqlCommand(DatabaseQueries.Ingredient.Insert, _connection);
            command.Parameters.AddWithValue("@Id", ingredient.Id);
            command.Parameters.AddWithValue("@Name", ingredient.Name);
            command.Parameters.AddWithValue("@UnitTypeId", ingredient.UnitTypeId);
            command.Parameters.AddWithValue("@BrandId", ingredient.BrandId);
            command.Parameters.AddWithValue("@RetailPrice1K", ingredient.RetailPrice);
            command.Parameters.AddWithValue("@WholesalePrice1K", ingredient.WholesalePrice);
            command.Parameters.AddWithValue("@DefaultPrice", ingredient.DefaultPrice);
            command.Parameters.AddWithValue("@Comments", ingredient.Comments);
            command.Parameters.AddWithValue("@CreatedDate", ingredient.CreatedDate);
            command.Parameters.AddWithValue("@CreatedUser", ingredient.CreatedUser);
            command.Parameters.AddWithValue("@ModifiedDate", ingredient.ModifiedDate);
            command.Parameters.AddWithValue("@ModifiedUser", ingredient.ModifiedUser);
            command.ExecuteNonQuery();
        }
    }
}
