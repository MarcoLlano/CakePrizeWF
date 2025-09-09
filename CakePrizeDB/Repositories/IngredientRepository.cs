using CakePrizeDB.Constants;
using CakePrizeDB.Models;
using CakePrizeDB.Services;
using Microsoft.Data.SqlClient;

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
            using var command = new SqlCommand(DatabaseQueries.Ingredient.GetAllIngredients, _connection);
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                ingredients.Add(new IngredientModel
                {
                    Id = reader.GetGuid(0),
                    Name = reader.GetString(1),
                    UnitTypeId = reader.GetGuid(2),
                    BrandId = reader.IsDBNull(3) ? null : reader.GetGuid(3),
                    RetailPrice = (float) reader.GetDouble(4),
                    WholesalePrice = (float) reader.GetDouble(5),
                    DefaultPrice = reader.GetString(6),
                    PackQty = reader.GetInt32(7),
                    Comments = reader.GetString(8),
                    CreatedDate = reader.GetDateTime(9),
                    CreatedUser = reader.GetString(10),
                    ModifiedDate = reader.GetDateTime(11),
                    ModifiedUser = reader.GetString(12)
                });
            }

            return ingredients;
        }

        public IngredientModel? GetById(Guid id)
        {
            try
            {
                using var command = new SqlCommand(DatabaseQueries.Ingredient.GetIngredientById, _connection);
                command.Parameters.AddWithValue("@IngredientId", id);

                using var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    return new IngredientModel
                    {
                        Id = reader.GetGuid(0),
                        Name = reader.GetString(1),
                        UnitTypeId = reader.GetGuid(2),
                        BrandId = reader.IsDBNull(3) ? null : reader.GetGuid(3),
                        RetailPrice = (float)reader.GetDouble(4),
                        WholesalePrice = (float)reader.GetDouble(5),
                        DefaultPrice = reader.GetString(6),
                        PackQty = reader.GetInt32(7),
                        Comments = reader.GetString(8),
                        CreatedDate = reader.GetDateTime(9),
                        CreatedUser = reader.GetString(10),
                        ModifiedDate = reader.GetDateTime(11),
                        ModifiedUser = reader.GetString(12)
                    };
                }

                return null;
            }
            catch (System.Data.SqlTypes.SqlNullValueException e)
            {
                return null;
            }
            
        }

        public void Insert(IngredientModel ingredient)
        {
            using var command = new SqlCommand(DatabaseQueries.Ingredient.InsertNewIngredient, _connection);
            command.Parameters.AddWithValue("@Id", ingredient.Id);
            command.Parameters.AddWithValue("@Name", ingredient.Name);
            command.Parameters.AddWithValue("@UnitTypeId", ingredient.UnitTypeId);
            
            // Handle nullable BrandId - use DBNull.Value when null
            if (ingredient.BrandId.HasValue)
            {
                command.Parameters.AddWithValue("@BrandId", ingredient.BrandId.Value);
            }
            else
            {
                command.Parameters.AddWithValue("@BrandId", DBNull.Value);
            }
            
            command.Parameters.AddWithValue("@RetailPrice1K", ingredient.RetailPrice);
            command.Parameters.AddWithValue("@WholesalePrice1K", ingredient.WholesalePrice);
            command.Parameters.AddWithValue("@DefaultPrice", ingredient.DefaultPrice);
            command.Parameters.AddWithValue("@PackQty", ingredient.PackQty);
            command.Parameters.AddWithValue("@Comments", ingredient.Comments);
            command.Parameters.AddWithValue("@CreatedDate", ingredient.CreatedDate);
            command.Parameters.AddWithValue("@CreatedUser", ingredient.CreatedUser);
            command.Parameters.AddWithValue("@ModifiedDate", ingredient.ModifiedDate);
            command.Parameters.AddWithValue("@ModifiedUser", ingredient.ModifiedUser);
            command.ExecuteNonQuery();
        }

        internal void Update(IngredientModel ingredient)
        {
            using var command = new SqlCommand(DatabaseQueries.Ingredient.UpdateIngredient, _connection);
            command.Parameters.AddWithValue("@Id", ingredient.Id);
            command.Parameters.AddWithValue("@Name", ingredient.Name);
            command.Parameters.AddWithValue("@UnitTypeId", ingredient.UnitTypeId);

            // Handle nullable BrandId - use DBNull.Value when null
            if (ingredient.BrandId.HasValue)
            {
                command.Parameters.AddWithValue("@BrandId", ingredient.BrandId.Value);
            }
            else
            {
                command.Parameters.AddWithValue("@BrandId", DBNull.Value);
            }

            command.Parameters.AddWithValue("@RetailPrice1K", ingredient.RetailPrice);
            command.Parameters.AddWithValue("@WholesalePrice1K", ingredient.WholesalePrice);
            command.Parameters.AddWithValue("@DefaultPrice", ingredient.DefaultPrice);
            command.Parameters.AddWithValue("@PackQty", ingredient.PackQty);
            command.Parameters.AddWithValue("@Comments", ingredient.Comments);
            command.Parameters.AddWithValue("@ModifiedDate", ingredient.ModifiedDate);
            command.Parameters.AddWithValue("@ModifiedUser", ingredient.ModifiedUser);
            command.ExecuteNonQuery();
        }
    }
}
