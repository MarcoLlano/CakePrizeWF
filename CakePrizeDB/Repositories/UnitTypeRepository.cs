using CakePrizeDB.Constants;
using CakePrizeDB.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace CakePrizeDB.Repositories
{
    public class UnitTypeRepository
    {
        private readonly SqlConnection _connection;

        public UnitTypeRepository(SqlConnection connection)
        {
            _connection = connection ?? throw new ArgumentNullException(nameof(connection));
        }

        public List<UnitType> GetAll()
        {
            var unitTypes = new List<UnitType>();
            
            using var command = new SqlCommand(DatabaseQueries.UnitType.GetAll, _connection);
            using var reader = command.ExecuteReader();
            
            while (reader.Read())
            {
                unitTypes.Add(new UnitType
                {
                    Id = reader.GetGuid(0),
                    Name = reader.GetString(1),
                    Acronym = reader.GetString(2)
                });
            }
            
            return unitTypes;
        }

        public UnitType? GetById(Guid id)
        {
            using var command = new SqlCommand(DatabaseQueries.UnitType.GetById, _connection);
            command.Parameters.AddWithValue("@UnitTypeId", id);
            
            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return new UnitType
                {
                    Id = reader.GetGuid(0),
                    Name = reader.GetString(1),
                    Acronym = reader.GetString(2)
                };
            }
            
            return null;
        }

        public void Insert(UnitType unitType)
        {
            using var command = new SqlCommand(DatabaseQueries.UnitType.Insert, _connection);
            command.Parameters.AddWithValue("@Id", unitType.Id);
            command.Parameters.AddWithValue("@Name", unitType.Name);
            command.Parameters.AddWithValue("@Acronym", unitType.Acronym);
            
            command.ExecuteNonQuery();
        }
    }
}
