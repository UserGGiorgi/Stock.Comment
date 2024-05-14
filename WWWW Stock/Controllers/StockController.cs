using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WWWW_Stock.Data;
using WWWW_Stock.DTOs.Stock;
using WWWW_Stock.Helpers;
using WWWW_Stock.Interface;
using WWWW_Stock.Mappers;

namespace WWWW_Stock.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class StockController:ControllerBase
    {
        private readonly IStockRepository _stockRepo;
        private readonly ApplicationDbContext _context;

        public StockController(ApplicationDbContext context,IStockRepository stockRepo)
        {
            _stockRepo=stockRepo;
            _context = context;
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll([FromQuery]QueryObject query) 
        {
            if(!ModelState.IsValid) 
                return BadRequest(ModelState);

            var stocks = await _stockRepo.GetAllAsync(query);

            var stockDto=stocks.Select(s=>s.ToStockDto()).ToList();
            return Ok(stockDto);
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute]int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var stock =await _context.Stocks.FindAsync(id);
            if (stock == null)
            {
                return NotFound();
            }

            return Ok(stock.ToStockDto());
        }
        
        [HttpPost]
        public async Task<IActionResult> Create([FromBody]CreateStockRequestDto StockDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var stockModel=StockDto.ToStockFromCreateDto();
            await _stockRepo.CreateAsync(stockModel);
            return CreatedAtAction(nameof(GetById), new { id = stockModel.Id }, stockModel.ToStockDto());
        }
        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromRoute]int id, [FromBody]UpdateStockDto updateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var stockModel=await _stockRepo.UpdateAsync(id, updateDto);
            
            if (stockModel == null) return BadRequest();

            return Ok(stockModel.ToStockDto());
        }
        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute]int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var stockModel=await _stockRepo.DeleteAsync(id);
            
            if(stockModel == null) return NotFound();
            
            return NoContent();
        }
    }
}
