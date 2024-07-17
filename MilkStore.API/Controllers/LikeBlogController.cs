using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MilkStore.Service.Interfaces;
using MilkStore.Service.Models.ViewModels.InteractionModels;
using MilkStore.Service.Services;

namespace MilkStore.API.Controllers
{
    
    public class LikeBlogController : BaseController
    {
        private readonly ILikeBlogService _likeblogService;

        public LikeBlogController(ILikeBlogService likeblogService)
        {
            _likeblogService = likeblogService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateLike([FromBody] CreateLike model, string userId, int blogId)
        {
            var response = await _likeblogService.CreateOfUpdateLike(model, userId, blogId);

            if (response.Success)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }
           
        }

        [HttpGet]
        public async Task<IActionResult> GetLikeByBlogId(int blogId, int pageIndex = 0, int pageSize = 10)
        {
            var result = await _likeblogService.GetLikeByBlogId(pageIndex, pageSize, blogId);
            return Ok(result);
        }
    }
}
