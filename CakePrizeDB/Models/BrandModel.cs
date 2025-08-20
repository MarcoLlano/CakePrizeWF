namespace CakePrizeDB.Models
{
    public class BrandModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Comments { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public string CreatedUser { get; set; } = string.Empty;
        public DateTime ModifiedDate { get; set; } = DateTime.Now;
        public string ModifiedUser { get; set; } = string.Empty;
    }
}
