using System.ComponentModel.DataAnnotations;

namespace WWWW_Stock.DTOs.Comment
{
    public class UpdateCommentRequestDto
    {
        [Required]
        [MinLength(5, ErrorMessage = "Title Must Be Atleast 5 Character")]
        [MaxLength(300, ErrorMessage = "Yout Title Can't Be Over 300 Characters")]
        public string Title { get; set; } = string.Empty;
        [Required]
        [MinLength(5, ErrorMessage = "Title Must Be Atleast 5 Character")]
        [MaxLength(300, ErrorMessage = "Yout Content Can't Be Over 300 Characters")]
        public string Content { get; set; } = string.Empty;
    }
}
