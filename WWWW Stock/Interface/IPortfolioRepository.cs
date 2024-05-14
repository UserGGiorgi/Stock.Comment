using WWWW_Stock.Models;

namespace WWWW_Stock.Interface
{
    public interface IPortfolioRepository
    {
        Task<List<Stock>> GetUserPortfolios(AppUser user);
        Task<Portfolio> CreateAsync(Portfolio portfolio);
        Task<Portfolio> DeletePortfolio(AppUser appUser ,string sumbol);
    }
}
