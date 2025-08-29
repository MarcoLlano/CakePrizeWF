using CakePrizeDB.Constants;
using CakePrizeDB.Models;
using Microsoft.Data.SqlClient;
using System.Drawing;
using System.IO;

namespace CakePrizeDB.Repositories
{
    internal class ProductPhotoRepository
    {
        private readonly SqlConnection _connection;
        public ProductPhotoRepository(SqlConnection sqlConnection)
        {
            _connection = sqlConnection ?? throw new ArgumentNullException(nameof(sqlConnection));
        }

        internal List<ProductPhotoModel> GetAll()
        {
            var prodTypes = new List<ProductPhotoModel>();
            using var command = new SqlCommand(DatabaseQueries.ProductPhoto.GetAll, _connection);
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                prodTypes.Add(new ProductPhotoModel
                {
                    Id = reader.GetGuid(0),
                    ProductId = reader.GetGuid(1),
                    Name = reader.GetString(2),
                    Src = reader.GetString(3),
                    Image = ConvertBytesToImage(reader.GetValue(4) as byte[]),
                });
            }

            return prodTypes;
        }

        public ProductPhotoModel? GetById(Guid id)
        {
            using var command = new SqlCommand(DatabaseQueries.ProductPhoto.GetById, _connection);
            command.Parameters.AddWithValue("@ProductPhotoId", id);

            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return new ProductPhotoModel
                {
                    Id = reader.GetGuid(0),
                    ProductId = reader.GetGuid(1),
                    Name = reader.GetString(2),
                    Src = reader.GetString(3),
                    Image = ConvertBytesToImage(reader.GetValue(4) as byte[]),
                };
            }

            return null;
        }

        public ProductPhotoModel? GetByProductId(Guid id)
        {
            using var command = new SqlCommand(DatabaseQueries.ProductPhoto.GetByProductId, _connection);
            command.Parameters.AddWithValue("@ProductId", id);

            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return new ProductPhotoModel
                {
                    Id = reader.GetGuid(0),
                    ProductId = reader.GetGuid(1),
                    Name = reader.GetString(2),
                    Src = reader.GetString(3),
                    Image = ConvertBytesToImage(reader.GetValue(4) as byte[]),
                };
            }

            return null;
        }

        public void Insert(ProductPhotoModel productPhoto)
        {
            using var command = new SqlCommand(DatabaseQueries.ProductPhoto.Insert, _connection);
            command.Parameters.AddWithValue("@Id", productPhoto.Id);
            command.Parameters.AddWithValue("@ProductId", productPhoto.ProductId);
            command.Parameters.AddWithValue("@Name", productPhoto.Name);
            command.Parameters.AddWithValue("@Src", productPhoto.Src);
            command.Parameters.AddWithValue("@Image", ConvertImageToBytes(productPhoto.Image));
            command.Parameters.AddWithValue("@CreatedDate", productPhoto.CreatedDate);
            command.Parameters.AddWithValue("@CreatedUser", productPhoto.CreatedUser);
            command.Parameters.AddWithValue("@ModifiedDate", productPhoto.ModifiedDate);
            command.Parameters.AddWithValue("@ModifiedUser", productPhoto.ModifiedUser);

            command.ExecuteNonQuery();
        }

        /// <summary>
        /// Converts an Image object to a byte array for database storage
        /// </summary>
        private byte[]? ConvertImageToBytes(Image? image)
        {
            if (image == null) return null;

            using var memoryStream = new MemoryStream();
            image.Save(memoryStream, image.RawFormat);
            return memoryStream.ToArray();
        }

        /// <summary>
        /// Converts a byte array from the database back to an Image object
        /// </summary>
        private Image? ConvertBytesToImage(byte[]? imageBytes)
        {
            if (imageBytes == null || imageBytes.Length == 0) return null;

            try
            {
                using var memoryStream = new MemoryStream(imageBytes);
                return Image.FromStream(memoryStream);
            }
            catch
            {
                return null;
            }
        }
    }
}
