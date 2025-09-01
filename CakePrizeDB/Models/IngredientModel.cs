namespace CakePrizeDB.Models
{
    public class IngredientModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public Guid UnitTypeId { get; set; }
        public Guid? BrandId { get; set; }
        public float RetailPrice { get; set; } = 0;
        public float WholesalePrice { get; set; } = 0;
        public string DefaultPrice { get; set; } = string.Empty;
        public string Comments { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public string CreatedUser { get; set; } = string.Empty;
        public DateTime ModifiedDate { get; set; } = DateTime.Now;
        public string ModifiedUser { get; set; } = string.Empty;
    }
}
