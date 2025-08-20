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
            public const string Insert = "INSERT INTO [CakePrize].[dbo].[ingredient] (id, name, unit_type_id, brand_id, " +
                "retail_price_1k, wholesale_price_1k, default_selected_price, comments, created_date, created_user, modified_date," +
                " modified_user) VALUES (@Id, @Name, @UnitTypeId, @BrandId, @RetailPrice1K, @WholesalePrice1K, @DefaultPrice, @Comments," +
                " @CreatedDate, @CreatedUser, @ModifiedDate, @ModifiedUser)";
        }

        public static class Brand
        {
            public const string GetAll = "SELECT * FROM [CakePrize].[dbo].[brand]";
            public const string GetById = "SELECT * FROM [CakePrize].[dbo].[brand] WHERE id = @BrandId";
            public const string Insert = "INSERT INTO [CakePrize].[dbo].[brand] " +
                "(id, name, comments, created_date, created_user, modified_date, modified_user) VALUES " +
                "(@Id, @Name, @Comments, @CreatedDate, @CreatedUser, @ModifiedDate, @ModifiedUser)";
        }
    }
}
