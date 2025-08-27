using CakePrizeDB.Constants;
using CakePrizeDB.Models;
using Microsoft.Data.SqlClient;

namespace CakePrizeDB.Repositories
{
    internal class ProductSizeRepository
    {
        private readonly SqlConnection _connection;
        public ProductSizeRepository(SqlConnection sqlConnection)
        {
            _connection = sqlConnection ?? throw new ArgumentNullException(nameof(sqlConnection));
        }

        internal List<ProductSizeModel> GetAll()
        {
            var prodSize = new List<ProductSizeModel>();
            using var command = new SqlCommand(DatabaseQueries.ProductSize.GetAll, _connection);
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                prodSize.Add(new ProductSizeModel
                {
                    Id = reader.GetGuid(0),
                    ProductId = reader.GetGuid(1),
                    Portions = reader.GetString(2),
                    Size = reader.GetString(3),
                    Comments = reader.GetString(4)
                });
            }

            return prodSize;
        }

        public ProductSizeModel? GetById(Guid id)
        {
            using var command = new SqlCommand(DatabaseQueries.ProductSize.GetById, _connection);
            command.Parameters.AddWithValue("@ProductSizeId", id);

            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return new ProductSizeModel
                {
                    Id = reader.GetGuid(0),
                    ProductId = reader.GetGuid(1),
                    Portions = reader.GetString(2),
                    Size = reader.GetString(3),
                    Comments = reader.GetString(4)
                };
            }

            return null;
        }

        public void Insert(ProductSizeModel productSize)
        {
            using var command = new SqlCommand(DatabaseQueries.ProductSize.Insert, _connection);
            command.Parameters.AddWithValue("@Id", productSize.Id);
            command.Parameters.AddWithValue("@ProductId", productSize.ProductId);
            command.Parameters.AddWithValue("@ProductPortions", productSize.Portions);
            command.Parameters.AddWithValue("@Size", productSize.Size);
            command.Parameters.AddWithValue("@Comments", productSize.Comments);
            command.Parameters.AddWithValue("@CreatedDate", productSize.CreatedDate);
            command.Parameters.AddWithValue("@CreatedUser", productSize.CreatedUser);
            command.Parameters.AddWithValue("@ModifiedDate", productSize.ModifiedDate);
            command.Parameters.AddWithValue("@ModifiedUser", productSize.ModifiedUser);

            command.ExecuteNonQuery();
        }
    }
}
