using CakePrizeDB.Constants;
using CakePrizeDB.Models;
using Microsoft.Data.SqlClient;

namespace CakePrizeDB.Repositories
{
    internal class ProductIngredientRepository
    {
        private readonly SqlConnection _connection;
        public ProductIngredientRepository(SqlConnection sqlConnection)
        {
            _connection = sqlConnection ?? throw new ArgumentNullException(nameof(sqlConnection));
        }

        internal List<ProductIngredientModel> GetAll()
        {
            var prodTypes = new List<ProductIngredientModel>();
            using var command = new SqlCommand(DatabaseQueries.ProductIngredient.GetAll, _connection);
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                prodTypes.Add(new ProductIngredientModel
                {
                    Id = reader.GetGuid(0),
                });
            }

            return prodTypes;
        }

        public ProductIngredientModel? GetById(Guid id)
        {
            using var command = new SqlCommand(DatabaseQueries.ProductIngredient.GetById, _connection);
            command.Parameters.AddWithValue("@ProductIngredientId", id);

            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return new ProductIngredientModel
                {
                    Id = reader.GetGuid(0),
                    ProductId = reader.GetGuid(1),
                    IngredientId = reader.GetGuid(2),
                    IngredientQtyPerPrep = reader.GetFloat(3)
                };
            }

            return null;
        }

        public void Insert(ProductIngredientModel productIngredient)
        {
            using var command = new SqlCommand(DatabaseQueries.ProductIngredient.Insert, _connection);
            command.Parameters.AddWithValue("@Id", productIngredient.Id);
            command.Parameters.AddWithValue("@ProductId", productIngredient.ProductId);
            command.Parameters.AddWithValue("@IngredientId", productIngredient.IngredientId);
            command.Parameters.AddWithValue("@IngredientQtyPerPrep", productIngredient.IngredientQtyPerPrep);
            command.Parameters.AddWithValue("@CreatedDate", productIngredient.CreatedDate);
            command.Parameters.AddWithValue("@CreatedUser", productIngredient.CreatedUser);
            command.Parameters.AddWithValue("@ModifiedDate", productIngredient.ModifiedDate);
            command.Parameters.AddWithValue("@ModifiedUser", productIngredient.ModifiedUser);

            command.ExecuteNonQuery();
        }
    }
}
