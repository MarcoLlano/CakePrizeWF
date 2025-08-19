using CakePrizeDB.Models;
using CakePrizeDB.Repositories;
using Microsoft.Data.SqlClient;

namespace CakePrizeDB.Services
{
    public class UnitTypeService
    {
        private readonly UnitTypeRepository _repository;

        public UnitTypeService(SqlConnection connection)
        {
            _repository = new UnitTypeRepository(connection);
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
        /// Gets a specific unit type by its ID
        /// </summary>
        /// <param name="id">The GUID of the unit type</param>
        /// <returns>The unit type if found, null otherwise</returns>
        public UnitTypeModel? GetUnitTypeById(Guid id)
        {
            return _repository.GetById(id);
        }

        /// <summary>
        /// Creates a new unit type
        /// </summary>
        /// <param name="name">The name of the unit type (e.g., "Grams", "Milliliters")</param>
        /// <param name="acronym">The acronym (e.g., "g", "ml")</param>
        /// <returns>The created unit type with generated ID</returns>
        public UnitTypeModel CreateUnitType(string name, string acronym)
        {
            var unitType = new UnitTypeModel
            {
                Id = Guid.NewGuid(),
                Name = name,
                Acronym = acronym
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
