using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dto.Comment
{
    public class CreateCommentRequestDto
    {
        [Required]
        [MinLength(5, ErrorMessage = "Title must be at least 5 chars length.")]
        [MaxLength(288, ErrorMessage = "Max length is 288 chars.")]
        public string Title { get; set; } = string.Empty;
        [Required]
        [MinLength(5, ErrorMessage = "Content must be at least 5 chars length.")]
        [MaxLength(288, ErrorMessage = "Max length is 288 chars.")]
        public string Content { get; set; } = string.Empty;
    }
}