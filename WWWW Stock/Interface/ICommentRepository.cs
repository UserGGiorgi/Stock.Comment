using WWWW_Stock.DTOs.Comment;
using WWWW_Stock.DTOs.Stock;
using WWWW_Stock.Models;

namespace WWWW_Stock.Interface
{
    public interface ICommentRepository
    {
        Task<List<Comment>> GetAllAsync();
        Task<Comment?> GetByIdAsync(int id);
        Task<Comment> CreateAsync(Comment commentModel);
        Task<Comment?> UpdateAsync(int Id, Comment commentModel);
        Task<Comment?> DeleteAsync(int Id);

    }
}
