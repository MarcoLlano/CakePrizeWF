using CakePrizeDB.Constants;
using CakePrizeDB.Models;
using Microsoft.Data.SqlClient;

namespace CakePrizeDB.Repositories
{
    internal class ProductTypeRepository
    {
        private readonly SqlConnection _connection;
        public ProductTypeRepository(SqlConnection sqlConnection)
        {
            _connection = sqlConnection ?? throw new ArgumentNullException(nameof(sqlConnection));
        }

        internal List<ProductTypeModel> GetAll()
        {
            var prodTypes = new List<ProductTypeModel>();
            using var command = new SqlCommand(DatabaseQueries.ProductType.GetAll, _connection);
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                prodTypes.Add(new ProductTypeModel
                {
                    Id = reader.GetGuid(0),
                    Name = reader.GetString(1),
                });
            }

            return prodTypes;
        }

        public ProductTypeModel? GetById(Guid id)
        {
            using var command = new SqlCommand(DatabaseQueries.ProductType.GetById, _connection);
            command.Parameters.AddWithValue("@ProductTypeId", id);

            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return new ProductTypeModel
                {
                    Id = reader.GetGuid(0),
                    Name = reader.GetString(1),
                };
            }

            return null;
        }

        public void Insert(ProductTypeModel prodType)
        {
            using var command = new SqlCommand(DatabaseQueries.ProductType.Insert, _connection);
            command.Parameters.AddWithValue("@Id", prodType.Id);
            command.Parameters.AddWithValue("@Name", prodType.Name);
            command.Parameters.AddWithValue("@CreatedDate", prodType.CreatedDate);
            command.Parameters.AddWithValue("@CreatedUser", prodType.CreatedUser);
            command.Parameters.AddWithValue("@ModifiedDate", prodType.ModifiedDate);
            command.Parameters.AddWithValue("@ModifiedUser", prodType.ModifiedUser);

            command.ExecuteNonQuery();
        }
    }
}
