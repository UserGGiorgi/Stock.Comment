using System.ComponentModel.DataAnnotations.Schema;
using WWWW_Stock.DTOs.Comment;
using WWWW_Stock.Models;

namespace WWWW_Stock.DTOs.Stock
{
    public class StockDto
    {
        public int Id { get; set; }
        public string Symbol { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public decimal LastDiv { get; set; }
        public string Industry { get; set; } = string.Empty;
        public long MarketCap { get; set; }
        public List<CommentDto> comments { get; set; }
    }
}
