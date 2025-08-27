using CakePrizeDB.Models;
using CakePrizeDB.Repositories;
using Microsoft.Data.SqlClient;

namespace CakePrizeDB.Services
{
    public class ProductIngredientService
    {
        private readonly ProductIngredientRepository _repository;

        public ProductIngredientService(SqlConnection sqlConnection)
        {
            _repository = new ProductIngredientRepository(sqlConnection);
        }

        /// <summary>
        /// Gets all product ingredients from the database
        /// </summary>
        /// <returns>List of all product ingredients</returns>
        public List<ProductIngredientModel> GetAllProductIngredients()
        {
            return _repository.GetAll();
        }

        /// <summary>
        /// Gets a specific product-ingredient by its ID
        /// </summary>
        /// <param name="id">The GUID of the product-ingredient</param>
        /// <returns>The product ingredient if found, null otherwise</returns>
        public ProductIngredientModel? GetProductIngredientById(Guid id)
        {
            return _repository.GetById(id);
        }

        /// <summary>
        /// Creates a new product-ingredient link
        /// </summary>
        /// <returns>The created product-ingredient link with generated ID</returns>
        public ProductIngredientModel CreateProductIngredient(Guid productId, Guid ingredientId, float ingrQtyPerPrep,
            string createdUser, string modifiedUser)
        {
            var product = new ProductIngredientModel
            {
                Id = Guid.NewGuid(),
                ProductId = productId,
                IngredientId = ingredientId,
                IngredientQtyPerPrep = ingrQtyPerPrep,
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
