using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using WWWW_Stock.Data;
using WWWW_Stock.Interface;
using WWWW_Stock.Models;

namespace WWWW_Stock.Repository
{
    public class PortfolioRepository : IPortfolioRepository
    {
        private readonly ApplicationDbContext _context;

        public PortfolioRepository(ApplicationDbContext context)
        {
            _context=context;
        }

        public async Task<Portfolio> CreateAsync(Portfolio portfolio)
        {
            await _context.Portfolios.AddAsync(portfolio);
            await _context.SaveChangesAsync();
            return portfolio;
        }

        public async Task<Portfolio> DeletePortfolio(AppUser appUser,string symbol)
        {
            var portfolioModel=await _context.Portfolios.FirstOrDefaultAsync(x=>x.AppUserId==appUser.Id&& x.Stock.Symbol.ToLower()==symbol.ToLower());
            if (portfolioModel == null) return null;
            _context.Portfolios.Remove(portfolioModel);
            await _context.SaveChangesAsync();
            return portfolioModel;
        }

        public async Task<List<Stock>> GetUserPortfolios(AppUser user)
        {
            return await _context.Portfolios.Where(x => x.AppUserId == user.Id)
                .Select(stock => new Stock
                {
                  Id=stock.StockId,
                  Symbol=stock.Stock.Symbol,
                  CompanyName=stock.Stock.CompanyName,
                  LastDiv=stock.Stock.LastDiv,
                  Industry=stock.Stock.Industry,
                  MarketCap=stock.Stock.MarketCap
                }).ToListAsync();

        }
    }
}
