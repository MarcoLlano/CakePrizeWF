using CakePrizeDB.Models;
using CakePrizeDB.Repositories;
using CakePrizeCore.libs.DBUtils;

namespace CakePrizeDB.Services
{
    public class ProductService
    {
        private readonly ProductRepository _repository;

        public ProductService()
        {
            // Use centralized connection management
            var connection = DatabaseConnectionManager.OpenConnection();
            _repository = new ProductRepository(connection);
        }

        /// <summary>
        /// Gets all products from the database
        /// </summary>
        /// <returns>List of all products</returns>
        public List<ProductModel> GetAllProducts()
        {
            return _repository.GetAll();
        }

        /// <summary>
        /// Gets a specific product by its ID
        /// </summary>
        /// <param name="id">The GUID of the product</param>
        /// <returns>The product if found, null otherwise</returns>
        public ProductModel? GetProductById(Guid id)
        {
            return _repository.GetById(id);
        }

        /// <summary>
        /// Creates a new product
        /// </summary>
        /// <param name="name">The name of the product (e.g., "Torta tres leches", "Mush de limon")</param>
        /// <returns>The created product with generated ID</returns>
        public ProductModel CreateProduct(Guid productTypeId, string name, string createdUser, string modifiedUser)
        {
            var product = new ProductModel
            {
                Id = Guid.NewGuid(),
                ProductTypeId = productTypeId,
                Name = name,
                CreatedDate = DateTime.Now,
                CreatedUser = createdUser,
                ModifiedDate = DateTime.Now,
                ModifiedUser = modifiedUser

            };

            _repository.Insert(product);
            return product;
        }
    }
}

