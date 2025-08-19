using CakePrizeDB.Models;
using CakePrizeDB.Repositories;
using Microsoft.Data.SqlClient;

namespace CakePrizeDB.Services
{
    public class IngredientService
    {
        private readonly IngredientRepository _repository;
        public IngredientService(SqlConnection sqlConnection)
        {
            _repository = new IngredientRepository(sqlConnection);
        }

        public List<IngredientModel> GetAllIngredients()
        {
            return _repository.GetAll();
        }
    }
}
