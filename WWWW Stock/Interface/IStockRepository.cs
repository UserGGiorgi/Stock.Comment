using WWWW_Stock.DTOs.Comment;
using WWWW_Stock.DTOs.Stock;
using WWWW_Stock.Helpers;
using WWWW_Stock.Models;

namespace WWWW_Stock.Interface
{
    public interface IStockRepository
    {
        Task<List<Stock>> GetAllAsync(QueryObject query);
        Task<Stock?> GetByIdAsync(int id);// FirstOrDefault Can Be NULL
        Task<Stock?> GetBySymbolAsync(string symbol);
        Task<Stock> CreateAsync(Stock stockModel);
        Task<Stock?> UpdateAsync(int Id,UpdateStockDto stockDto);
        Task<Stock?> DeleteAsync(int Id);
        Task<bool> StockExists(int StockId);

    }
}
