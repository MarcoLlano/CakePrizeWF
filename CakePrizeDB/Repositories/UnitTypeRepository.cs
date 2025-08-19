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

        public List<UnitTypeModel> GetAll()
        {
            var unitTypes = new List<UnitTypeModel>();
            
            using var command = new SqlCommand(DatabaseQueries.UnitType.GetAll, _connection);
            using var reader = command.ExecuteReader();
            
            while (reader.Read())
            {
                unitTypes.Add(new UnitTypeModel
                {
                    Id = reader.GetGuid(0),
                    Name = reader.GetString(1),
                    Acronym = reader.GetString(2)
                });
            }
            
            return unitTypes;
        }

        public UnitTypeModel? GetById(Guid id)
        {
            using var command = new SqlCommand(DatabaseQueries.UnitType.GetById, _connection);
            command.Parameters.AddWithValue("@UnitTypeId", id);
            
            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return new UnitTypeModel
                {
                    Id = reader.GetGuid(0),
                    Name = reader.GetString(1),
                    Acronym = reader.GetString(2)
                };
            }
            
            return null;
        }

        public void Insert(UnitTypeModel unitType)
        {
            using var command = new SqlCommand(DatabaseQueries.UnitType.Insert, _connection);
            command.Parameters.AddWithValue("@Id", unitType.Id);
            command.Parameters.AddWithValue("@Name", unitType.Name);
            command.Parameters.AddWithValue("@Acronym", unitType.Acronym);
            
            command.ExecuteNonQuery();
        }
    }
}
