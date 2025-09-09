using CakePrizeCore.libs.DBUtils;
using CakePrizeDB.Models;
using CakePrizeDB.Repositories;
using System.Xml.Linq;
using static CakePrizeDB.Constants.DatabaseQueries;

namespace CakePrizeDB.Services
{
    public class IngredientService
    {
        private readonly IngredientRepository _repository;
        public IngredientService()
        {
            // Use centralized connection management
            var connection = DatabaseConnectionManager.OpenConnection();
            _repository = new IngredientRepository(connection);
        }

        /// <summary>
        /// Gets all ingredients from the database
        /// </summary>
        /// <returns>List of all ingredients</returns>
        public List<IngredientModel> GetAllIngredients()
        {
            return _repository.GetAll();
        }

        /// <summary>
        /// Gets a specific ingredient by its ID
        /// </summary>
        /// <param name="id">The GUID of the ingredient</param>
        /// <returns>The ingredient if found, null otherwise</returns>
        public IngredientModel? GetIngredientById(Guid id)
        {
            return _repository.GetById(id);
        }

        /// <summary>
        /// Creates a new ingredient
        /// </summary>
        /// <param name="name">The name of the ingredient (e.g., "Powder", "Milk")</param>
        /// <param name="acronym">The acronym (e.g., "g", "ml")</param>
        /// <returns>The created unit type with generated ID</returns>
        public IngredientModel CreateIngredient(string name, Guid unitTypeId, Guid? brandId, float retailPrice,
            float wholesalePrice, string defaultPrice, int packQty, string comments, string createdUser, string modifiedUser)
        {
            if(unitTypeId != Guid.Empty)
            {
                var ingredient = new IngredientModel
                {
                    Id = Guid.NewGuid(),
                    Name = name,
                    UnitTypeId = unitTypeId,
                    BrandId = brandId,
                    RetailPrice = retailPrice,
                    WholesalePrice = wholesalePrice,
                    DefaultPrice = defaultPrice,
                    PackQty = packQty,
                    Comments = comments,
                    CreatedDate = DateTime.Now,
                    CreatedUser = createdUser,
                    ModifiedDate = DateTime.Now,
                    ModifiedUser = modifiedUser

                };

                _repository.Insert(ingredient);
                return ingredient;
            }
            throw new Exception("Not possible to add new ingredient, please review the payload");
        }

        public IngredientModel UpdateIngredient(Guid? ingId, string name, Guid unitTypeId, Guid? ingredientBrandId, float retailPrice, float wholesalePrice,
            string selectedPrice, int packQty, string comments, string modifiedUser)
        {
            var ingredient = new IngredientModel
            {
                Id = ingId,
                Name = name,
                UnitTypeId = unitTypeId,
                BrandId = ingredientBrandId,
                RetailPrice = retailPrice,
                WholesalePrice = wholesalePrice,
                DefaultPrice = selectedPrice,
                PackQty = packQty,
                Comments = comments,
                ModifiedDate = DateTime.Now,
                ModifiedUser = modifiedUser
            };
            _repository.Update(ingredient);
            return ingredient;
        }
    }
}

