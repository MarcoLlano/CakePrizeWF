using CakePrizeDB.Models;
using CakePrizeDB.Repositories;
using CakePrizeCore.libs.DBUtils;

namespace CakePrizeDB.Services
{
    public class ProductSizeService
    {
        private readonly ProductSizeRepository _repository;

        public ProductSizeService()
        {
            // Use centralized connection management
            var connection = DatabaseConnectionManager.OpenConnection();
            _repository = new ProductSizeRepository(connection);
        }

        /// <summary>
        /// Gets all product sizes from the database
        /// </summary>
        /// <returns>List of all product sizes</returns>
        public List<ProductSizeModel> GetAllProductSizes()
        {
            return _repository.GetAll();
        }

        /// <summary>
        /// Gets a specific product size by its ID
        /// </summary>
        /// <param name="id">The GUID of the product size</param>
        /// <returns>The product size if found, null otherwise</returns>
        public ProductSizeModel? GetProductSizeById(Guid id)
        {
            return _repository.GetById(id);
        }

        /// <summary>
        /// Creates a new product size
        /// </summary>
        /// <returns>The created product size with generated ID</returns>
        public ProductSizeModel CreateProductSize(Guid productId, int portions, string size, string comments, string createdUser, string modifiedUser)
        {
            var productSize = new ProductSizeModel
            {
                Id = Guid.NewGuid(),
                ProductId = productId,
                Portions = portions,
                Size = size,
                Comments = comments,
                CreatedDate = DateTime.Now,
                CreatedUser = createdUser,
                ModifiedDate = DateTime.Now,
                ModifiedUser = modifiedUser

            };

            _repository.Insert(productSize);
            return productSize;
        }
    }
}

