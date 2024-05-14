using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using WWWW_Stock.Data;
using WWWW_Stock.DTOs.Comment;
using WWWW_Stock.DTOs.Stock;
using WWWW_Stock.Helpers;
using WWWW_Stock.Interface;
using WWWW_Stock.Models;

namespace WWWW_Stock.Repository
{
    public class StockRepository : IStockRepository
    {
        private readonly ApplicationDbContext _context;
        public StockRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Stock> CreateAsync(Stock stockModel)
        {
           await _context.Stocks.AddAsync(stockModel);
            await _context.SaveChangesAsync();
            return stockModel;
        }

        public async Task<Stock?> DeleteAsync(int Id)
        {
           var stockModel= await _context.Stocks.FirstOrDefaultAsync(x => x.Id == Id);

            if (stockModel == null) return null;

           _context.Stocks.Remove(stockModel);
           await _context.SaveChangesAsync();
            return stockModel;
        }

        public async Task<List<Stock>> GetAllAsync(QueryObject query)     //Filtering
        {
            var stocks= _context.Stocks.Include(c=>c.Comments).ThenInclude(x=>x.AppUser).AsQueryable(); 

            if(!string.IsNullOrWhiteSpace(query.CompanyName))//by CompanyName
            {
                stocks=stocks.Where(s=>s.CompanyName.Contains(query.CompanyName));
            }

            if (!string.IsNullOrWhiteSpace(query.Symbol))    //By Symbol
            {
                stocks = stocks.Where(s => s.Symbol.Contains(query.Symbol));
            }

            if(!string.IsNullOrWhiteSpace(query.SortBy))  //Sorting Only For Symbol
            {
                if(query.SortBy.Equals("Symbol",StringComparison.OrdinalIgnoreCase))
                {
                    stocks = query.IsDecsending ? stocks.OrderByDescending(s=>s.Symbol):stocks.OrderBy(s=>s.Symbol);                        
                }
            }

            var skipNumber = (query.PageNumber - 1) * query.PageSize;//Pagination


            return await stocks.Skip(skipNumber).Take(query.PageSize).ToListAsync();
        }

        public async Task<Stock?> GetByIdAsync(int id)
        {
            return await _context.Stocks.Include(c => c.Comments).FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<Stock?> GetBySymbolAsync(string symbol)
        {
            return await _context.Stocks.FirstOrDefaultAsync(i => i.Symbol == symbol);
        }

        public async Task<bool> StockExists(int StockId)
        {
            return await _context.Stocks.AnyAsync(s=>s.Id == StockId); 
        }

        public async Task<Stock?> UpdateAsync(int Id, UpdateStockDto stockDto)
        {
            var existingStock=await _context.Stocks.FirstOrDefaultAsync(s=>s.Id == Id);
            if (existingStock == null) return null;

            //Manual Mapping
            existingStock.Symbol = stockDto.Symbol;
            existingStock.MarketCap = stockDto.MarketCap;
            existingStock.LastDiv = stockDto.LastDiv;
            existingStock.CompanyName = stockDto.CompanyName;
            existingStock.Industry = stockDto.Industry;
            await _context.SaveChangesAsync();

            return existingStock;
        }
    }
}
