using Microsoft.EntityFrameworkCore;
using WWWW_Stock.Data;
using WWWW_Stock.DTOs.Comment;
using WWWW_Stock.Interface;
using WWWW_Stock.Models;

namespace WWWW_Stock.Repository
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationDbContext _context;

        public CommentRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<Comment>> GetAllAsync()
        {
            return await _context.Comments.Include(x=>x.AppUser).ToListAsync();
        }
        public async Task<Comment?> GetByIdAsync(int id)
        {
            return await _context.Comments.Include(x => x.AppUser).FirstOrDefaultAsync(y=>y.Id == id);
        }
        public async Task<Comment> CreateAsync(Comment commentModel)
        {
           await _context.Comments.AddAsync(commentModel);
            await _context.SaveChangesAsync();
            return commentModel;
        }

        public async Task<Comment?> UpdateAsync(int Id, Comment commentModel)
        {
            var existingComment=await _context.Comments.FindAsync(Id);
            if (existingComment==null)  return null;
            
            existingComment.Title = commentModel.Title;
            existingComment.Content = commentModel.Content;
            await _context.SaveChangesAsync();
            return existingComment;
        }

        public async Task<Comment?> DeleteAsync(int Id)
        {
            var  comment=await _context.Comments.FirstOrDefaultAsync(s=>s.Id==Id);
            if(comment==null) return null;

             _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
            return comment;
        }
    }
}
