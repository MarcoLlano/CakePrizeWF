using CakePrizeDB.Models;
using CakePrizeDB.Repositories;
using Microsoft.Data.SqlClient;

namespace CakePrizeDB.Services
{
    public class ProductTypeService
    {
        private readonly ProductTypeRepository _repository;

        public ProductTypeService(SqlConnection sqlConnection)
        {
            _repository = new ProductTypeRepository(sqlConnection);
        }

        /// <summary>
        /// Gets all product types from the database
        /// </summary>
        /// <returns>List of all product types</returns>
        public List<ProductTypeModel> GetAllProductTypes()
        {
            return _repository.GetAll();
        }

        /// <summary>
        /// Gets a specific productType by its ID
        /// </summary>
        /// <param name="id">The GUID of the product type</param>
        /// <returns>The product type if found, null otherwise</returns>
        public ProductTypeModel? GetProductTypeById(Guid id)
        {
            return _repository.GetById(id);
        }

        /// <summary>
        /// Creates a new product type
        /// </summary>
        /// <param name="name">The name of the product type (e.g., "Cupcake", "Empanada")</param>
        /// <returns>The created product type with generated ID</returns>
        public ProductTypeModel CreateProductType(string name, string createdUser, string modifiedUser)
        {
            var productType = new ProductTypeModel
            {
                Id = Guid.NewGuid(),
                Name = name,
                CreatedDate = DateTime.Now,
                CreatedUser = createdUser,
                ModifiedDate = DateTime.Now,
                ModifiedUser = modifiedUser

            };

            _repository.Insert(productType);
            return productType;
        }
    }
}
