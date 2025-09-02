namespace CakePrizeDB.Models
{
    public class LogsModel
    {
        public Guid Id { get; set; }
        public string Type { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public string CreatedUser { get; set; } = string.Empty;
    }
}
