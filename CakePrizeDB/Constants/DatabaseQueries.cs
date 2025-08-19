namespace CakePrizeDB.Constants
{
    public static class DatabaseQueries
    {
        public static class UnitType
        {
            public const string GetAll = "SELECT id, name, acronym FROM [CakePrize].[dbo].[unit_type]";
            public const string GetById = "SELECT id, name, acronym FROM [CakePrize].[dbo].[unit_type] WHERE id = @UnitTypeId";
            public const string Insert = "INSERT INTO [CakePrize].[dbo].[unit_type] (id, name, acronym) VALUES (@Id, @Name, @Acronym)";
            public const string Update = "UPDATE [CakePrize].[dbo].[unit_type] SET name = @Name, acronym = @Acronym WHERE id = @Id";
            public const string Delete = "DELETE FROM [CakePrize].[dbo].[unit_type] WHERE id = @Id";
        }

        public static class Ingredient
        {
            public const string GetAll = "SELECT * FROM [CakePrize].[dbo].[ingredient]";
            public const string GetById = "SELECT * FROM [CakePrize].[dbo].[ingredient] WHERE id = @IngredientId";
            public const string Insert = "INSERT INTO [CakePrize].[dbo].[ingredient] (name, unit_type_id, price_per_unit) VALUES (@Name, @UnitTypeId, @PricePerUnit)";
        }
    }
}
