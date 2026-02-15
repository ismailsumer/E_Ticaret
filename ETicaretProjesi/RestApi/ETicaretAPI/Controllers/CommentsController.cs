using ETicaretAPI.DTOs;
using ETicaretAPI.Services;
using ETicaretAPI.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ETicaretAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentService _commentService;
        public CommentsController(ICommentService commentService) { _commentService = commentService; }

        private int UserId => int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

        [HttpGet("product/{productId}")]
        public async Task<IActionResult> GetProductComments(int productId)
            => Ok(await _commentService.GetProductCommentsAsync(productId));

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddComment(CommentCreateDto dto)
            => Ok(await _commentService.AddCommentAsync(UserId, dto));

        [HttpGet("all")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllComments()
            => Ok(await _commentService.GetAllCommentsAsync());

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            var result = await _commentService.DeleteCommentAsync(id);
            if (!result.Success) return NotFound(result);
            return Ok(result);
        }
    }
}
