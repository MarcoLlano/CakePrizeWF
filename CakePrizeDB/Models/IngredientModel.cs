using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CakePrizeDB.Models
{
    public class IngredientModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public Guid UnitType { get; set; }
        public Guid IngredientBrand { get; set; }
        public int RetailPrice { get; set; } = 0;
        public int WholesalePrice { get; set; } = 0;
        public string DefaultPrice { get; set; } = string.Empty;
        public string Comments { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public string CreatedUser { get; set; } = string.Empty;
        public DateTime ModifiedDate { get; set; } = DateTime.Now;
        public string ModifiedUser { get; set; } = string.Empty;
    }
}
