namespace CakePrizeDB.Models
{
    public class ProductIngredientModel
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public Guid IngredientId { get; set; }
        public float IngredientQtyPerPrep { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public string CreatedUser { get; set; } = string.Empty;
        public DateTime ModifiedDate { get; set; } = DateTime.Now;
        public string ModifiedUser { get; set; } = string.Empty;
    }
}
