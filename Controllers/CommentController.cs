using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using api.Dto.Comment;
using api.Interfaces;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [Route("api/comment")]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commentRepo;
        private readonly IStockRepository _stockRepo;
        public CommentController(ICommentRepository commentRepo, IStockRepository stockRepo)
        {
            _commentRepo = commentRepo;
            _stockRepo = stockRepo;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var comments = await _commentRepo.GetAllAsync();
            var commentDtos = comments.Select(c => c.ToCommentDto());
            return Ok(commentDtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var comment = await _commentRepo.GetByIdAsync(id);
            if (comment == null)
            {
                return NotFound();
            }
            return Ok(comment.ToCommentDto());
        }

        [HttpPost("{commentId}")]
        public async Task<IActionResult> Create([FromRoute] int commentId,
         [FromBody] CreateCommentRequestDto commentRequestDto)
        {
            if (!await _stockRepo.StockExist(commentId))
            {
                return BadRequest("Stock does not exist");
            }
            var commentModel = commentRequestDto.ToComment(commentId);

            var comment = await _commentRepo.CreateAsync(commentModel);
            return CreatedAtAction(nameof(GetById), new { id = comment.Id }, comment.ToCommentDto());
        }

        [HttpPut("{commentId}")]
        public async Task<IActionResult> Update(int commentId, [FromBody] UpdateCommentRequestDto updateCommentDto)
        {
            var comment = await _commentRepo.UpdateAsync(commentId, updateCommentDto);
            if (comment == null)
            {
                return NotFound("Comment noit found");
            }
            return Ok(comment.ToCommentDto());
        }

        [HttpDelete("{commentId}")]
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