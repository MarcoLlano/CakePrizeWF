using CakePrizeDB.Models;
using CakePrizeDB.Repositories;
using CakePrizeCore.libs.DBUtils;
using CakePrizeCore.libs.Configuration;

namespace CakePrizeDB.Services
{
    /// <summary>
    /// Enhanced ProductIngredientService with environment-aware database connections
    /// </summary>
    public class EnhancedProductIngredientService
    {
        private readonly EnvironmentConfig.Environment _environment;

        public EnhancedProductIngredientService() : this(EnvironmentConfig.CurrentEnvironment)
        {
        }

        public EnhancedProductIngredientService(EnvironmentConfig.Environment environment)
        {
            _environment = environment;
        }

        /// <summary>
        /// Gets all product ingredients from the database for the current environment
        /// </summary>
        /// <returns>List of all product ingredients</returns>
        public List<ProductIngredientModel> GetAllProductIngredients()
        {
            using var connection = DatabaseConnectionManager.OpenConnection(_environment);
            var repository = new ProductIngredientRepository(connection);
            return repository.GetAll();
        }

        /// <summary>
        /// Gets a specific product-ingredient by its ID for the current environment
        /// </summary>
        /// <param name="id">The GUID of the product-ingredient</param>
        /// <returns>The product ingredient if found, null otherwise</returns>
        public ProductIngredientModel? GetProductIngredientById(Guid id)
        {
            using var connection = DatabaseConnectionManager.OpenConnection(_environment);
            var repository = new ProductIngredientRepository(connection);
            return repository.GetById(id);
        }

        /// <summary>
        /// Gets product ingredients by product ID for the current environment
        /// </summary>
        /// <param name="id">The GUID of the product</param>
        /// <returns>The product ingredients for the given product id if found, null otherwise</returns>
        public List<ProductIngredientModel>? GetProductIngredientByProductId(Guid id)
        {
            using var connection = DatabaseConnectionManager.OpenConnection(_environment);
            var repository = new ProductIngredientRepository(connection);
            return repository.GetByProductId(id);
        }

        /// <summary>
        /// Creates a new product-ingredient link in the current environment
        /// </summary>
        /// <param name="productId">The product ID</param>
        /// <param name="ingredientId">The ingredient ID</param>
        /// <param name="ingrQtyPerPrep">The ingredient quantity per preparation</param>
        /// <param name="createdUser">The user who created the record</param>
        /// <param name="modifiedUser">The user who last modified the record</param>
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

            using var connection = DatabaseConnectionManager.OpenConnection(_environment);
            var repository = new ProductIngredientRepository(connection);
            repository.Insert(product);
            return product;
        }

        /// <summary>
        /// Gets the current environment this service is configured for
        /// </summary>
        public EnvironmentConfig.Environment Environment => _environment;

        /// <summary>
        /// Tests the database connection for the current environment
        /// </summary>
        /// <returns>True if connection is successful, false otherwise</returns>
        public bool TestConnection()
        {
            return DatabaseConnectionManager.TestConnection(_environment);
        }

        /// <summary>
        /// Gets database connection information for the current environment
        /// </summary>
        /// <returns>Database connection information</returns>
        public DatabaseConnectionInfo GetConnectionInfo()
        {
            return DatabaseConnectionManager.GetConnectionInfo(_environment);
        }
    }
}


