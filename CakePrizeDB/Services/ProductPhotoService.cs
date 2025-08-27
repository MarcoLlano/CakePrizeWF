using CakePrizeDB.Models;
using CakePrizeDB.Repositories;
using Microsoft.Data.SqlClient;
using System.Drawing;

namespace CakePrizeDB.Services
{
    public class ProductPhotoService
    {
        private readonly ProductPhotoRepository _repository;

        public ProductPhotoService(SqlConnection sqlConnection)
        {
            _repository = new ProductPhotoRepository(sqlConnection);
        }

        /// <summary>
        /// Gets all product photos from the database
        /// </summary>
        /// <returns>List of all product photos</returns>
        public List<ProductPhotoModel> GetAllProductPhotos()
        {
            return _repository.GetAll();
        }

        /// <summary>
        /// Gets a specific product photo by its ID
        /// </summary>
        /// <param name="id">The GUID of the product photo</param>
        /// <returns>The product photo if found, null otherwise</returns>
        public ProductPhotoModel? GetProductPhotoById(Guid id)
        {
            return _repository.GetById(id);
        }

        /// <summary>
        /// Creates a new product photo
        /// </summary>
        /// <param name="name">The name of the product photo (e.g., "Tortatresleches.png", "Mushdelimon.jpg")</param>
        /// <returns>The created product photo with generated ID</returns>
        public ProductPhotoModel CreateProductPhoto(Guid productId, string name, string src, Image image, string createdUser, string modifiedUser)
        {
            var productPhoto = new ProductPhotoModel
            {
                Id = Guid.NewGuid(),
                ProductId = productId,
                Name = name,
                Src = src,
                Image = image,
                CreatedDate = DateTime.Now,
                CreatedUser = createdUser,
                ModifiedDate = DateTime.Now,
                ModifiedUser = modifiedUser

            };

            _repository.Insert(productPhoto);
            return productPhoto;
        }
    }
}
