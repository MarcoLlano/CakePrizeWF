using CakePrizeDB.Models;
using CakePrizeDB.Repositories;
using Microsoft.Data.SqlClient;
using System.Diagnostics.Tracing;

namespace CakePrizeDB.Services
{
    public class BrandService
    {
        private readonly BrandRepository _repository;
        public BrandService(SqlConnection sqlConnection)
        {
            _repository = new BrandRepository(sqlConnection);
        }

        /// <summary>
        /// Gets all brands from the database
        /// </summary>
        /// <returns>List of all brands</returns>
        public List<BrandModel> GetAllBrands()
        {
            return _repository.GetAll();
        }

        /// <summary>
        /// Gets a specific brand by its ID
        /// </summary>
        /// <param name="id">The GUID of the brand</param>
        /// <returns>The brand if found, null otherwise</returns>
        public BrandModel? GetBrandById(Guid id)
        {
            return _repository.GetById(id);
        }

        /// <summary>
        /// Creates a new unit type
        /// </summary>
        /// <param name="name">The name of the unit type (e.g., "Grams", "Milliliters")</param>
        /// <param name="acronym">The acronym (e.g., "g", "ml")</param>
        /// <returns>The created unit type with generated ID</returns>
        public BrandModel CreateBrand(string name, string comments, string createdUser, string modifiedUser)
        {
            var unitType = new BrandModel
            {
                Id = Guid.NewGuid(),
                Name = name,
                Comments = comments,
                CreatedDate = DateTime.Now,
                CreatedUser = createdUser,
                ModifiedDate = DateTime.Now,
                ModifiedUser = modifiedUser

            };

            _repository.Insert(unitType);
            return unitType;
        }
    }
}
