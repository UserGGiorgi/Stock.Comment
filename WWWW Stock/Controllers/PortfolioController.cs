using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WWWW_Stock.Extension;
using WWWW_Stock.Interface;
using WWWW_Stock.Models;

namespace WWWW_Stock.Controllers
{
    [Route("api/portfolio")]
    [ApiController]
    public class PortfolioController:ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IStockRepository _stockRepo;
        private readonly IPortfolioRepository _portfolioRepo;

        public PortfolioController(UserManager<AppUser> userManager
            ,IStockRepository stockRepo
            ,IPortfolioRepository portfolioRepo
            )
        {
            _userManager = userManager;
            _stockRepo=stockRepo;
            _portfolioRepo=portfolioRepo;
        }
        [HttpGet]
        [Authorize]//username:GioGio   passwprd :1500Givik@.1
        public async Task<IActionResult> GetUserPortfolio()
        {
            var Username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(Username);
            var usesPortfolio = await _portfolioRepo.GetUserPortfolios(appUser);
            return Ok(usesPortfolio);
        }
        [HttpPost]
        [Authorize]

        public async Task<IActionResult> AddPortfolio(string symbol)
        {
            var userName= User.GetUsername();
            var appUser=await _userManager.FindByNameAsync(userName);   
            
            var stock=await _stockRepo.GetBySymbolAsync(symbol);
            if (stock == null) return BadRequest("Stock Not Found");

            var portfolio= await _portfolioRepo.GetUserPortfolios(appUser); //just to check if portfolio exists
            if (portfolio.Any(x => x.Symbol.ToLower() == symbol.ToLower())) return BadRequest("Cannot Add Same Stock To Portfolio");

            var portfoliomodel = new Portfolio
            { AppUserId=appUser.Id,
                 StockId=stock.Id,
            };

            await _portfolioRepo.CreateAsync(portfoliomodel);

            if (portfoliomodel == null)
            {
                return StatusCode(500, "Could Not Create");
            }
            else
            {
                return Created();
            }
        }
        
        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> DeletePortfolio(string symbol)
        {
            var userName= User.GetUsername();
            var appUser=await _userManager.FindByNameAsync(userName);

            var userPortfolio = await _portfolioRepo.GetUserPortfolios(appUser);

            var filteredStock= userPortfolio.Where(x => x.Symbol.ToLower() == symbol.ToLower()).ToList();

            if(filteredStock.Count()==1)
            {
                await _portfolioRepo.DeletePortfolio(appUser,symbol);
            }
            else
            {
                return BadRequest("Stock Is Not In Your Portfolio");
            }
            return Ok();
        }

    }
}
