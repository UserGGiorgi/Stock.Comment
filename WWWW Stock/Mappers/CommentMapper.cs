using System.Runtime.CompilerServices;
using WWWW_Stock.DTOs.Comment;
using WWWW_Stock.Models;

namespace WWWW_Stock.Mappers
{
    public static class CommentMapper
    {
        public static CommentDto ToCommentDto(this Comment comment)
        {
            return new CommentDto
            {
                Id = comment.Id,
                StockId = comment.StockId,
                Title = comment.Title,
                Content = comment.Content,
                createdBy = comment.AppUser.UserName,
                CreatedOn = comment.CreatedOn,
            };        
        }
        public static Comment ToCommentFromCreate(this CreateCommentDto commentDto,int stockId )
        {
            return new Comment
            {
                StockId = stockId, 
                Title = commentDto.Title,
                Content = commentDto.Content
            };
        }
        public static Comment ToCommentFromUpdate(this UpdateCommentRequestDto updateDto)
        {
            return new Comment
            {                
                Title = updateDto.Title,
                Content = updateDto.Content
            };
        }
    }
}
