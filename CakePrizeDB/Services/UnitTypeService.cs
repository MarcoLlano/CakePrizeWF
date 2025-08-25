using CakePrizeDB.Models;
using CakePrizeDB.Repositories;
using Microsoft.Data.SqlClient;

namespace CakePrizeDB.Services
{
    public class UnitTypeService
    {
        private readonly UnitTypeRepository _repository;
        public UnitTypeService(SqlConnection sqlConnection)
        {
            _repository = new UnitTypeRepository(sqlConnection);
        }

        /// <summary>
        /// Gets all unit types from the database
        /// </summary>
        /// <returns>List of all unit types</returns>
        public List<UnitTypeModel> GetAllUnitTypes()
        {
            return _repository.GetAll();
        }

        /// <summary>
        /// Gets a specific unitType by its ID
        /// </summary>
        /// <param name="id">The GUID of the unitType</param>
        /// <returns>The unitType if found, null otherwise</returns>
        public UnitTypeModel? GetUnitTypeById(Guid id)
        {
            return _repository.GetById(id);
        }

        /// <summary>
        /// Creates a new unitType
        /// </summary>
        /// <param name="name">The name of the unitType (e.g., "Miligram", "Kilogram")</param>
        /// <param name="acronym">The acronym (e.g., "g", "ml")</param>
        /// <returns>The created unit type with generated ID</returns>
        public UnitTypeModel CreateUnitType(string name, string acronym, string createdUser, string modifiedUser)
        {
            var unitType = new UnitTypeModel
            {
                Id = Guid.NewGuid(),
                Name = name,
                Acronym = acronym,
                CreatedDate = DateTime.Now,
                CreatedUser = createdUser,
                ModifiedDate = DateTime.Now,
                ModifiedUser = modifiedUser
            };
            
            _repository.Insert(unitType);
            return unitType;
        }

        /// <summary>
        /// Gets unit types by name (case-insensitive search)
        /// </summary>
        /// <param name="searchTerm">The search term</param>
        /// <returns>List of matching unit types</returns>
        public List<UnitTypeModel> SearchUnitTypes(string searchTerm)
        {
            var allUnitTypes = _repository.GetAll();
            return allUnitTypes
                .Where(ut => ut.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                            ut.Acronym.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        /// <summary>
        /// Gets all unit type acronyms as a simple list
        /// </summary>
        /// <returns>List of acronyms</returns>
        public List<string> GetAllAcronyms()
        {
            return _repository.GetAll()
                .Select(ut => ut.Acronym)
                .ToList();
        }
    }
}
