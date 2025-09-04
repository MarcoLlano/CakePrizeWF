using CakePrizeCore.libs.DBUtils;
using CakePrizeDB.Models;
using CakePrizeDB.Repositories;

namespace CakePrizeDB.Services
{
    public class BrandService
    {
        private readonly BrandRepository _repository;
        public BrandService()
        {
            var connection = DatabaseConnectionManager.OpenConnection();
            _repository = new BrandRepository(connection);
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
        public BrandModel? GetBrandById(Guid? id)
        {
            return _repository.GetById(id);
        }

        /// <summary>
        /// Creates a new brand
        /// </summary>
        /// <param name="name">The name of the brand (e.g., "Celinda", "Pil")</param>
        /// <returns>The created brand with generated ID</returns>
        public BrandModel CreateBrand(string name, string comments, string createdUser, string modifiedUser)
        {
            var brand = new BrandModel
            {
                Id = Guid.NewGuid(),
                Name = name,
                Comments = comments,
                CreatedDate = DateTime.Now,
                CreatedUser = createdUser,
                ModifiedDate = DateTime.Now,
                ModifiedUser = modifiedUser

            };

            _repository.Insert(brand);
            return brand;
        }

        /// <summary>
        /// Updates an existing brand
        /// </summary>
        /// <param name="brandId">The id of the brand</param>
        /// <param name="name">The name of the brand (e.g., "Celinda", "Pil")</param>
        /// <returns>The brand updated</returns>
        public BrandModel UpdateBrand(Guid? brandId, string name, string comments, string modifiedUser, DateTime brandModifiedDate)
        {
            var brand = new BrandModel
            {
                Id = brandId,
                Name = name,
                Comments = comments,
                ModifiedUser = modifiedUser,
                ModifiedDate = brandModifiedDate,
            };

            _repository.Update(brand);
            return brand;
        }
    }
}

