using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MilkStore.Service.Interfaces;
using MilkStore.Service.Models.ViewModels.InteractionModels;
using MilkStore.Service.Services;

namespace MilkStore.API.Controllers
{
    
    public class CommentBlogController : BaseController
    {
        private readonly ICommentBlogService _commentblogService;

        public CommentBlogController(ICommentBlogService commentblogService)
        {
            _commentblogService = commentblogService;
        }

        [HttpGet]
        public async Task<IActionResult> GetLCommentyBlogId(int blogId, int pageIndex = 0, int pageSize = 10)
        {
            var result = await _commentblogService.GetCommentByBlogId(pageIndex, pageSize, blogId);
            return Ok(result);
        }
        //Create comment
        [HttpPost]
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

        [HttpPut]
        public async Task<IActionResult> UpdateComment(int commentId,  int blogId, [FromBody] CreateCommentByBlogId model)
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
        [HttpPut]
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
