using CakePrizeDB.Services;
using CakePrizeDB.Models;
using CakePrizeDB.Repositories;
using Microsoft.Data.SqlClient;
using System.Configuration;

namespace CakePrizeDB.Examples
{
    /// <summary>
    /// Example demonstrating how to use the new database structure
    /// </summary>
    public class UsageExample
    {
        public static void DemonstrateUsage()
        {
            // 1. Get connection string from configuration
            var connectionString = ConfigurationManager.ConnectionStrings["CakePrize"]?.ConnectionString
                ?? throw new InvalidOperationException("Connection string not found");

            // 2. Create connection
            using var connection = new SqlConnection(connectionString);
            connection.Open();

            // 3. Create service (which uses repository internally)
            var unitTypeService = new UnitTypeService(connection);

            try
            {
                // 4. Example: Get all unit types
                Console.WriteLine("=== Getting All Unit Types ===");
                var allUnitTypes = unitTypeService.GetAllUnitTypes();
                foreach (var unitType in allUnitTypes)
                {
                    Console.WriteLine($"ID: {unitType.Id}, Name: {unitType.Name}, Acronym: {unitType.Acronym}");
                }

                // 5. Example: Create a new unit type
                Console.WriteLine("\n=== Creating New Unit Type ===");
                var newUnitType = unitTypeService.CreateUnitType("Kilograms", "kg", "Marco", "Marco");
                Console.WriteLine($"Created: ID: {newUnitType.Id}, Name: {newUnitType.Name}, Acronym: {newUnitType.Acronym}");

                // 6. Example: Get unit type by ID
                Console.WriteLine("\n=== Getting Unit Type by ID ===");
                var retrievedUnitType = unitTypeService.GetUnitTypeById(newUnitType.Id);
                if (retrievedUnitType != null)
                {
                    Console.WriteLine($"Retrieved: {retrievedUnitType.Name} ({retrievedUnitType.Acronym})");
                }

                // 7. Example: Search unit types
                Console.WriteLine("\n=== Searching Unit Types ===");
                var searchResults = unitTypeService.SearchUnitTypes("gram");
                foreach (var result in searchResults)
                {
                    Console.WriteLine($"Found: {result.Name} ({result.Acronym})");
                }

                // 8. Example: Get all acronyms
                Console.WriteLine("\n=== Getting All Acronyms ===");
                var acronyms = unitTypeService.GetAllAcronyms();
                Console.WriteLine($"Available acronyms: {string.Join(", ", acronyms)}");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// Example showing how to use the repository directly
        /// </summary>
        public static void DemonstrateDirectRepositoryUsage()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["CakePrize"]?.ConnectionString
                ?? throw new InvalidOperationException("Connection string not found");

            using var connection = new SqlConnection(connectionString);
            connection.Open();

            // Create repository directly
            var repository = new UnitTypeRepository(connection);

            Console.WriteLine("=== Direct Repository Usage ===");
            
            // Get all unit types
            var unitTypes = repository.GetAll();
            Console.WriteLine($"Total unit types: {unitTypes.Count}");

            // Create a new unit type
            var newUnitType = new UnitTypeModel
            {
                Id = Guid.NewGuid(),
                Name = "Pounds",
                Acronym = "lb"
            };

            repository.Insert(newUnitType);
            Console.WriteLine($"Inserted: {newUnitType.Name} ({newUnitType.Acronym})");

            // Retrieve by ID
            var retrieved = repository.GetById(newUnitType.Id);
            if (retrieved != null)
            {
                Console.WriteLine($"Retrieved: {retrieved.Name} ({retrieved.Acronym})");
            }
        }

        /// <summary>
        /// Example showing how to handle errors and connection management
        /// </summary>
        public static void DemonstrateErrorHandling()
        {
            try
            {
                var connectionString = ConfigurationManager.ConnectionStrings["CakePrize"]?.ConnectionString
                    ?? throw new InvalidOperationException("Connection string not found");

                using var connection = new SqlConnection(connectionString);
                
                // Ensure connection is open
                if (connection.State != System.Data.ConnectionState.Open)
                {
                    connection.Open();
                }

                var service = new UnitTypeService(connection);

                // Try to get a non-existent unit type
                var nonExistentId = Guid.NewGuid();
                var result = service.GetUnitTypeById(nonExistentId);
                
                if (result == null)
                {
                    Console.WriteLine($"Unit type with ID {nonExistentId} not found (expected behavior)");
                }

                // Try to create a unit type with invalid data
                try
                {
                    var invalidUnitType = service.CreateUnitType("", "", "Marco", "Marco"); // Empty name and acronym
                    Console.WriteLine("Created unit type with empty values (this might not be desired)");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Expected error when creating invalid unit type: {ex.Message}");
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Database error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General error: {ex.Message}");
            }
        }
    }
}
