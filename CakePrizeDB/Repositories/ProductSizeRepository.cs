using CakePrizeDB.Constants;
using CakePrizeDB.Models;
using Microsoft.Data.SqlClient;
using System.Data;

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
                int idIdx = reader.GetOrdinal("id");
                int productIdIdx = reader.GetOrdinal("product_id");
                int portionsIdx = reader.GetOrdinal("product_portions");
                int sizeIdx = reader.GetOrdinal("size");
                int commentsIdx = reader.GetOrdinal("comments");

                prodSize.Add(new ProductSizeModel
                {
                    Id = reader.GetGuid(idIdx),
                    ProductId = reader.GetGuid(productIdIdx),
                    Portions = ReadInt(reader, portionsIdx),
                    Size = reader.GetString(sizeIdx),
                    Comments = reader.GetString(commentsIdx)
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
                int idIdx = reader.GetOrdinal("id");
                int productIdIdx = reader.GetOrdinal("product_id");
                int portionsIdx = reader.GetOrdinal("product_portions");
                int sizeIdx = reader.GetOrdinal("size");
                int commentsIdx = reader.GetOrdinal("comments");

                return new ProductSizeModel
                {
                    Id = reader.GetGuid(idIdx),
                    ProductId = reader.GetGuid(productIdIdx),
                    Portions = ReadInt(reader, portionsIdx),
                    Size = reader.GetString(sizeIdx),
                    Comments = reader.GetString(commentsIdx)
                };
            }

            return null;
        }

        private static int ReadInt(SqlDataReader reader, int ordinal)
        {
            if (reader.IsDBNull(ordinal))
            {
                return 0;
            }
            object val = reader.GetValue(ordinal);
            if (val is int i) return i;
            if (val is long l) return (int)l;
            if (val is short s) return s;
            if (val is byte b) return b;
            if (val is string str && int.TryParse(str, out var parsed)) return parsed;
            return Convert.ToInt32(val);
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

        internal ProductSizeModel GetProductSizeByProductId(Guid? lastSelectedProductId)
        {
            using var command = new SqlCommand(DatabaseQueries.ProductSize.GetByProductId, _connection);
            command.Parameters.AddWithValue("@ProductId", lastSelectedProductId);

            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                int idIdx = reader.GetOrdinal("id");
                int productIdIdx = reader.GetOrdinal("product_id");
                int portionsIdx = reader.GetOrdinal("product_portions");
                int sizeIdx = reader.GetOrdinal("size");
                int commentsIdx = reader.GetOrdinal("comments");

                return new ProductSizeModel
                {
                    Id = reader.GetGuid(idIdx),
                    ProductId = reader.GetGuid(productIdIdx),
                    Portions = ReadInt(reader, portionsIdx),
                    Size = reader.GetString(sizeIdx),
                    Comments = reader.GetString(commentsIdx)
                };
            }

            return null;
        }
    }
}
