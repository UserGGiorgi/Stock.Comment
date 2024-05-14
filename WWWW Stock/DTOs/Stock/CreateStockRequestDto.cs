using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WWWW_Stock.DTOs.Stock
{
    public class CreateStockRequestDto
    {
        [Required]
        [MaxLength(10,ErrorMessage ="Company Symbol Cannot be Over 10 Characters")]
        public string Symbol { get; set; } = string.Empty;
        [Required]
        [MaxLength(10, ErrorMessage = "CompanyName Symbol Cannot be Over 10 Characters")]
        public string CompanyName { get; set; } = string.Empty;
        [Required]
        [Range(0.001,100)]
        public decimal LastDiv { get; set; }
        [Required]
        [MaxLength(10, ErrorMessage = "Industry Cannot be Over 10 Characters")]
        public string Industry { get; set; } = string.Empty;
        [Required]
        [Range(1, 500000000)]
        public long MarketCap { get; set; }
    }
}
