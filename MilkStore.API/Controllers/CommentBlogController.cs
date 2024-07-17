using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MilkStore.Service.Interfaces;
using MilkStore.Service.Models.ViewModels.InteractionModels;
using MilkStore.Service.Services;

namespace MilkStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentBlogController : ControllerBase
    {
        private readonly ICommentBlogService _commentblogService;

        public CommentBlogController(ICommentBlogService commentblogService)
        {
            _commentblogService = commentblogService;
        }

        [HttpGet("likes/{blogId}")]
        public async Task<IActionResult> GetLikeByBlogId([FromRoute] int blogId, [FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _commentblogService.GetCommentByBlogId(pageIndex, pageSize, blogId);
            return Ok(result);
        }
        //Create comment
        [HttpPost("create", Name = "CreateComment")]
        public async Task<IActionResult> CreateComment([FromBody] CreateCommentByBlogId model, string userId, int blogId)
        {
            // Check if the model state is valid
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _commentblogService.CreateComment(model, userId, blogId);

            if (response.Success)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }
        }

        [HttpPut("update/{commentId}/{blogId}")]
        public async Task<IActionResult> UpdateComment([FromRoute] int commentId, [FromRoute] int blogId, [FromBody] CreateCommentByBlogId model)
        {
            if (model == null)
            {
                return BadRequest("Update model cannot be null.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Call the service to update the comment
            var response = await _commentblogService.UpdateCommentByBlogID(model, commentId, blogId);

            if (response.Success)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }
        }
        //Delete comment
        [HttpPut("delete/{commentId}/{blogId}")]
        public async Task<IActionResult> DeleteComment([FromBody] SoftDeleteCommentByBlogId model, int commentId, int blogId)
        {
            var response = await _commentblogService.DeleteCommentByBlogID(model, commentId, blogId);
            if (response.Success)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }
        }

    }
}
