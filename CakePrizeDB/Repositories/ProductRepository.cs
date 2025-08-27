using CakePrizeDB.Constants;
using CakePrizeDB.Models;
using Microsoft.Data.SqlClient;

namespace CakePrizeDB.Repositories
{
    internal class ProductRepository
    {
        private readonly SqlConnection _connection;
        public ProductRepository(SqlConnection sqlConnection)
        {
            _connection = sqlConnection ?? throw new ArgumentNullException(nameof(sqlConnection));
        }

        internal List<ProductModel> GetAll()
        {
            var prodTypes = new List<ProductModel>();
            using var command = new SqlCommand(DatabaseQueries.Product.GetAll, _connection);
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                prodTypes.Add(new ProductModel
                {
                    Id = reader.GetGuid(0),
                    ProductTypeId = reader.GetGuid(1),
                    Name = reader.GetString(2),
                });
            }

            return prodTypes;
        }

        public ProductModel? GetById(Guid id)
        {
            using var command = new SqlCommand(DatabaseQueries.Product.GetById, _connection);
            command.Parameters.AddWithValue("@ProductId", id);

            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return new ProductModel
                {
                    Id = reader.GetGuid(0),
                    ProductTypeId = reader.GetGuid(1),
                    Name = reader.GetString(2),
                };
            }

            return null;
        }

        public void Insert(ProductModel product)
        {
            using var command = new SqlCommand(DatabaseQueries.Product.Insert, _connection);
            command.Parameters.AddWithValue("@Id", product.Id);
            command.Parameters.AddWithValue("@ProductTypeId", product.ProductTypeId);
            command.Parameters.AddWithValue("@Name", product.Name);
            command.Parameters.AddWithValue("@CreatedDate", product.CreatedDate);
            command.Parameters.AddWithValue("@CreatedUser", product.CreatedUser);
            command.Parameters.AddWithValue("@ModifiedDate", product.ModifiedDate);
            command.Parameters.AddWithValue("@ModifiedUser", product.ModifiedUser);

            command.ExecuteNonQuery();
        }
    }
}
