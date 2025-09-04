using CakePrizeCore.libs.Configuration;

namespace CakePrizeDB.Constants
{
    public static class DatabaseQueries
    {
        /// <summary>
        /// Gets the current database name based on the environment
        /// </summary>
        private static string GetCurrentDatabaseName()
        {
            return EnvironmentConfig.GetDatabaseName();
        }
        public static class UnitType
        {
            public static string GetAll => $"SELECT id, name, acronym FROM [{GetCurrentDatabaseName()}].[dbo].[unit_type]";
            public static string GetById => $"SELECT id, name, acronym FROM [{GetCurrentDatabaseName()}].[dbo].[unit_type] WHERE id = @UnitTypeId";
            public static string Insert => $"INSERT INTO [{GetCurrentDatabaseName()}].[dbo].[unit_type] (id, name, acronym) VALUES (@Id, @Name, @Acronym)";
            public static string Update => $"UPDATE [{GetCurrentDatabaseName()}].[dbo].[unit_type] SET name = @Name, acronym = @Acronym WHERE id = @Id";
            public static string Delete => $"DELETE FROM [{GetCurrentDatabaseName()}].[dbo].[unit_type] WHERE id = @Id";
        }

        public static class Ingredient
        {
            public static string GetAll => $"SELECT * FROM [{GetCurrentDatabaseName()}].[dbo].[ingredient]";
            public static string GetById => $"SELECT * FROM [{GetCurrentDatabaseName()}].[dbo].[ingredient] WHERE id = @IngredientId";
            public static string Insert => $"INSERT INTO [{GetCurrentDatabaseName()}].[dbo].[ingredient] (id, name, unit_type_id, brand_id, " +
                "retail_price_1k, wholesale_price_1k, default_selected_price, pack_qty, comments, created_date, created_user, modified_date," +
                " modified_user) VALUES (@Id, @Name, @UnitTypeId, @BrandId, @RetailPrice1K, @WholesalePrice1K, @DefaultPrice, @PackQty" +
                ", @Comments, @CreatedDate, @CreatedUser, @ModifiedDate, @ModifiedUser)";
        }

        public static class Brand
        {
            public static string GetAllBrands => $"SELECT * FROM [{GetCurrentDatabaseName()}].[dbo].[brand]";
            public static string GetBrandById => $"SELECT * FROM [{GetCurrentDatabaseName()}].[dbo].[brand] WHERE id = @BrandId";
            public static string InsertBrand => $"INSERT INTO [{GetCurrentDatabaseName()}].[dbo].[brand] " +
                "(id, name, comments, created_date, created_user, modified_date, modified_user) VALUES " +
                "(@Id, @Name, @Comments, @CreatedDate, @CreatedUser, @ModifiedDate, @ModifiedUser)";

            public static string UpdateBrand => $"UPDATE [{GetCurrentDatabaseName()}].[dbo].[brand] SET name = @Name," +
                $" comments = @Comments, modified_date = @ModifiedDate, modified_user = @ModifiedUser WHERE id = @Id";
        }

        public static class ProductType
        {
            public static string GetAll => $"SELECT * FROM [{GetCurrentDatabaseName()}].[dbo].[producttype]";
            public static string GetById => $"SELECT * FROM [{GetCurrentDatabaseName()}].[dbo].[producttype] WHERE id = @ProductTypeId";
            public static string Insert => $"INSERT INTO [{GetCurrentDatabaseName()}].[dbo].[producttype] " +
                "(id, name, created_date, created_user, modified_date, modified_user) VALUES " +
                "(@Id, @Name, @CreatedDate, @CreatedUser, @ModifiedDate, @ModifiedUser)";
        }

        public static class Product
        {
            public static string GetAll => $"SELECT * FROM [{GetCurrentDatabaseName()}].[dbo].[product]";
            public static string GetById => $"SELECT * FROM [{GetCurrentDatabaseName()}].[dbo].[product] WHERE id = @ProductId";
            public static string Insert => $"INSERT INTO [{GetCurrentDatabaseName()}].[dbo].[product] " +
                "(id, product_type_id, name, created_date, created_user, modified_date, modified_user) VALUES " +
                "(@Id, @ProductTypeId, @Name, @CreatedDate, @CreatedUser, @ModifiedDate, @ModifiedUser)";
        }

        public static class ProductIngredient
        {
            public static string GetAll => $"SELECT * FROM [{GetCurrentDatabaseName()}].[dbo].[product_ingredient]";
            public static string GetById => $"SELECT * FROM [{GetCurrentDatabaseName()}].[dbo].[product_ingredient] WHERE id = @ProductIngredientId";
            public static string GetByProductId => $"SELECT * FROM [{GetCurrentDatabaseName()}].[dbo].[product_ingredient] WHERE product_id = @ProductId";
            public static string Insert => $"INSERT INTO [{GetCurrentDatabaseName()}].[dbo].[product_ingredient] " +
                "(id, product_id, ingredient_id, ingredient_qty_per_prep, created_date, created_user, modified_date, modified_user) VALUES " +
                "(@Id, @ProductId, @IngredientId, @IngredientQtyPerPrep, @CreatedDate, @CreatedUser, @ModifiedDate, @ModifiedUser)";
        }

        public static class ProductPhoto
        {
            public static string GetAll => $"SELECT * FROM [{GetCurrentDatabaseName()}].[dbo].[product_photo]";
            public static string GetById => $"SELECT * FROM [{GetCurrentDatabaseName()}].[dbo].[product_photo] WHERE id = @ProductPhotoId";
            public static string GetByProductId => $"SELECT * FROM [{GetCurrentDatabaseName()}].[dbo].[product_photo] WHERE product_id = @ProductId";
            public static string Insert => $"INSERT INTO [{GetCurrentDatabaseName()}].[dbo].[product_photo] " +
                "(id, product_id, name, src, image, created_date, created_user, modified_date, modified_user) VALUES " +
                "(@Id, @ProductId, @Name, @Src, @Image, @CreatedDate, @CreatedUser, @ModifiedDate, @ModifiedUser)";
        }

        public static class ProductSize
        {
            public static string GetAll => $"SELECT * FROM [{GetCurrentDatabaseName()}].[dbo].[product_size]";
            public static string GetById => $"SELECT * FROM [{GetCurrentDatabaseName()}].[dbo].[product_size] WHERE id = @ProductSizeId";
            public static string Insert => $"INSERT INTO [{GetCurrentDatabaseName()}].[dbo].[product_size] " +
                "(id, product_id, product_portions, size, comments, created_date, created_user, modified_date, modified_user) VALUES " +
                "(@Id, @ProductId, @ProductPortions, @Size, @Comments, @CreatedDate, @CreatedUser, @ModifiedDate, @ModifiedUser)";
        }

        public static class Logs
        {
            public static string GetAll => $"SELECT * FROM [{GetCurrentDatabaseName()}].[dbo].[logs]";
            public static string GetById => $"SELECT * FROM [{GetCurrentDatabaseName()}].[dbo].[logs] WHERE id = @ProductSizeId";
            public static string GetByType => $"SELECT * FROM [{GetCurrentDatabaseName()}].[dbo].[logs] WHERE type = @Type";
            public static string Insert => $"INSERT INTO [{GetCurrentDatabaseName()}].[dbo].[logs] " +
                "(id, type, description, created_date, created_user) VALUES " +
                "(@Id, @Type, @Description, @CreatedDate, @CreatedUser)";
        }
    }
}
