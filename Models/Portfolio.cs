using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models
{
    [Table("Portoflios")]
    public class Portfolio
    {
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public int StockId { get; set; }
        public Stock Stock { get; set; }
    }
}