using CakePrizeDB.Constants;
using CakePrizeDB.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CakePrizeDB.Repositories
{
    internal class BrandRepository
    {
        private readonly SqlConnection _connection;
        public BrandRepository(SqlConnection sqlConnection)
        {
            _connection = sqlConnection ?? throw new ArgumentNullException(nameof(sqlConnection));
        }

        internal List<BrandModel> GetAll()
        {
            var brands = new List<BrandModel>();
            using var command = new SqlCommand(DatabaseQueries.Brand.GetAll, _connection);
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                brands.Add(new BrandModel
                {
                    Id = reader.GetGuid(0),
                    Name = reader.GetString(1),
                });
            }

            return brands;
        }

        public BrandModel? GetById(Guid id)
        {
            using var command = new SqlCommand(DatabaseQueries.Brand.GetById, _connection);
            command.Parameters.AddWithValue("@BrandId", id);

            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return new BrandModel
                {
                    Id = reader.GetGuid(0),
                    Name = reader.GetString(1),
                    Comments = reader.GetString(2)
                };
            }

            return null;
        }

        public void Insert(BrandModel brand)
        {
            using var command = new SqlCommand(DatabaseQueries.Brand.Insert, _connection);
            command.Parameters.AddWithValue("@Id", brand.Id);
            command.Parameters.AddWithValue("@Name", brand.Name);
            command.Parameters.AddWithValue("@Comments", brand.Comments);
            command.Parameters.AddWithValue("@CreatedDate", brand.CreatedDate);
            command.Parameters.AddWithValue("@CreatedUser", brand.CreatedUser);
            command.Parameters.AddWithValue("@ModifiedDate", brand.ModifiedDate);
            command.Parameters.AddWithValue("@ModifiedUser", brand.ModifiedUser);

            command.ExecuteNonQuery();
        }
    }
}
