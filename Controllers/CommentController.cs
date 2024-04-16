using api.Data;
using api.Dto.Comment;
using api.Extensions;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [Route("api/comment")]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commentRepo;
        private readonly IStockRepository _stockRepo;
        private readonly UserManager<AppUser> _userManager;
        public CommentController(ICommentRepository commentRepo, IStockRepository stockRepo, UserManager<AppUser> userManager)
        {
            _commentRepo = commentRepo;
            _stockRepo = stockRepo;
            _userManager = userManager;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var comments = await _commentRepo.GetAllAsync();
            var commentDtos = comments.Select(c => c.ToCommentDto());
            return Ok(commentDtos);
        }

        [HttpGet("{commentID:int}")]
        public async Task<IActionResult> GetById(int commentId)
        {
            var comment = await _commentRepo.GetByIdAsync(commentId);
            if (comment == null)
            {
                return NotFound();
            }
            return Ok(comment.ToCommentDto());
        }

        [HttpPost("{stockId:int}")]
        [Authorize]
        public async Task<IActionResult> Create([FromRoute] int stockId,
         [FromBody] CreateCommentRequestDto commentRequestDto)
        {
            if (!await _stockRepo.StockExist(stockId))
            {
                return BadRequest("Stock does not exist");
            }
            var commentModel = commentRequestDto.ToComment(stockId);

            var userEmail = User.GetEmail();
            var appUser = await _userManager.FindByEmailAsync(userEmail);
            commentModel.AppUserId = appUser.Id;

            var comment = await _commentRepo.CreateAsync(commentModel);
            return CreatedAtAction(nameof(GetById), new { commentId = comment.Id }, comment.ToCommentDto());
        }

        [HttpPut("{commentId:int}")]
        public async Task<IActionResult> Update(int commentId, [FromBody] UpdateCommentRequestDto updateCommentDto)
        {
            var comment = await _commentRepo.UpdateAsync(commentId, updateCommentDto);
            if (comment == null)
            {
                return NotFound("Comment noit found");
            }
            return Ok(comment.ToCommentDto());
        }

        [HttpDelete("{commentId:int}")]
        public async Task<IActionResult> DeleteById(int commentId)
        {
            Models.Comment? comment = await _commentRepo.DeleteAsync(commentId);
            if (comment == null)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}