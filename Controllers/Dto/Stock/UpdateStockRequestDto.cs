using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dto.Stock
{
    public class UpdateStockRequestDto
    {
        [Required]
        [MaxLength(10, ErrorMessage = "Max length is 10 chars.")]
        public string Symbol { get; set; } = string.Empty;
        [Required]
        [MaxLength(10, ErrorMessage = "Max length is 10 chars.")]
        public string CompanyName { get; set; } = string.Empty;
        [Required]
        [Range(1, 1_000_000_000)]
        public decimal Purchase { get; set; }
        [Required]
        [Range(0.001, 100)]
        public decimal LastDiv { get; set; }
        [Required]
        [MaxLength(10, ErrorMessage = "Max length is 10 chars.")]
        public string Industry { get; set; } = string.Empty;
        [Range(1, 5_000_000)]
        public long MarketCap { get; set; }
    }
}