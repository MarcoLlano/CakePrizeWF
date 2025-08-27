namespace CakePrizeDB.Models
{
    public class ProductSizeModel
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public string Portions { get; set; } = string.Empty;
        public string Size { get; set; } = string.Empty;
        public string Comments { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public string CreatedUser { get; set; } = string.Empty;
        public DateTime ModifiedDate { get; set; } = DateTime.Now;
        public string ModifiedUser { get; set; } = string.Empty;
    }
}
