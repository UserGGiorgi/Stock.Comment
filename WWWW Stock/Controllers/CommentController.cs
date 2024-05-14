using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WWWW_Stock.Data;
using WWWW_Stock.DTOs.Comment;
using WWWW_Stock.Extension;
using WWWW_Stock.Interface;
using WWWW_Stock.Mappers;
using WWWW_Stock.Models;

namespace WWWW_Stock.Controllers
{
    [Route("[Controller]")]
    [ApiController]
    public class CommentController:ControllerBase
    {
        private readonly ICommentRepository _commentRepo;
        private readonly IStockRepository _stockRepo;
        private readonly UserManager<AppUser> _userManager;

        public CommentController(ICommentRepository commentRepo
            ,IStockRepository stockRepo
            ,UserManager<AppUser> userManager)
        {          
            _commentRepo=commentRepo;
            _stockRepo=stockRepo;
            _userManager=userManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var comments= await _commentRepo.GetAllAsync();
            var commentDto = comments.Select(s => s.ToCommentDto());
            
            return Ok(commentDto);
        }
        [HttpGet]
        [Route("{id:int}")]

        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var commentModel = await _commentRepo.GetByIdAsync(id);
           
            if (commentModel == null) return NotFound();

            return Ok(commentModel);
        }
        [HttpPost("{stockId:int}")]
        public async Task<IActionResult> Create([FromRoute]int stockId,CreateCommentDto createDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!await _stockRepo.StockExists(stockId))
            {
                return BadRequest("Stock Doesn't Exists");                
            }
            var userName=User.GetUsername();//Username For Comment
            var appUser=await _userManager.FindByNameAsync(userName);

            var commentModel = createDto.ToCommentFromCreate(stockId);
            commentModel.AppUserId = appUser.Id;

            await _commentRepo.CreateAsync(commentModel);
            return CreatedAtAction(nameof(GetById), new {id=commentModel.Id},commentModel.ToCommentDto());
        }
        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromRoute]int id, [FromBody]UpdateCommentRequestDto updateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var comment = await _commentRepo.UpdateAsync(id,updateDto.ToCommentFromUpdate());
            if (comment == null) return BadRequest("Comment Doesn't Exist");
            
            return Ok(comment.ToCommentDto());
        }
        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute]int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var comment= await _commentRepo.DeleteAsync(id);
            if (comment == null) return BadRequest("Comment Doesn't Exist");

            return Ok(comment);
        }

    }
}
