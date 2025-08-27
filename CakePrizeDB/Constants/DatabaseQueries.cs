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

        public static class ProductType
        {
            public const string GetAll = "SELECT * FROM [CakePrize].[dbo].[producttype]";
            public const string GetById = "SELECT * FROM [CakePrize].[dbo].[producttype] WHERE id = @ProductTypeId";
            public const string Insert = "INSERT INTO [CakePrize].[dbo].[producttype] " +
                "(id, name, created_date, created_user, modified_date, modified_user) VALUES " +
                "(@Id, @Name, @CreatedDate, @CreatedUser, @ModifiedDate, @ModifiedUser)";
        }

        public static class Product
        {
            public const string GetAll = "SELECT * FROM [CakePrize].[dbo].[product]";
            public const string GetById = "SELECT * FROM [CakePrize].[dbo].[product] WHERE id = @ProductId";
            public const string Insert = "INSERT INTO [CakePrize].[dbo].[product] " +
                "(id, product_type_id, name, created_date, created_user, modified_date, modified_user) VALUES " +
                "(@Id, @ProductTypeId, @Name, @CreatedDate, @CreatedUser, @ModifiedDate, @ModifiedUser)";
        }

        public static class ProductIngredient
        {
            public const string GetAll = "SELECT * FROM [CakePrize].[dbo].[product_ingredient]";
            public const string GetById = "SELECT * FROM [CakePrize].[dbo].[product_ingredient] WHERE id = @ProductIngredientId";
            public const string Insert = "INSERT INTO [CakePrize].[dbo].[product_ingredient] " +
                "(id, product_id, ingredient_id, ingredient_qty_per_prep, created_date, created_user, modified_date, modified_user) VALUES " +
                "(@Id, @ProductId, @IngredientId, @IngredientQtyPerPrep, @CreatedDate, @CreatedUser, @ModifiedDate, @ModifiedUser)";
        }

        public static class ProductPhoto
        {
            public const string GetAll = "SELECT * FROM [CakePrize].[dbo].[product_photo]";
            public const string GetById = "SELECT * FROM [CakePrize].[dbo].[product_photo] WHERE id = @ProductPhotoId";
            public const string Insert = "INSERT INTO [CakePrize].[dbo].[product_photo] " +
                "(id, product_id, name, src, image, created_date, created_user, modified_date, modified_user) VALUES " +
                "(@Id, @ProductId, @Name, @Src, @Image, @CreatedDate, @CreatedUser, @ModifiedDate, @ModifiedUser)";
        }

        public static class ProductSize
        {
            public const string GetAll = "SELECT * FROM [CakePrize].[dbo].[product_size]";
            public const string GetById = "SELECT * FROM [CakePrize].[dbo].[product_size] WHERE id = @ProductSizeId";
            public const string Insert = "INSERT INTO [CakePrize].[dbo].[product_size] " +
                "(id, product_id, product_portions, size, comments, created_date, created_user, modified_date, modified_user) VALUES " +
                "(@Id, @ProductId, @ProductPortions, @Size, @Comments, @CreatedDate, @CreatedUser, @ModifiedDate, @ModifiedUser)";
        }
    }
}
