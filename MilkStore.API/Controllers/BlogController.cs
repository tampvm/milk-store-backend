using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MilkStore.Service.Interfaces;
using MilkStore.Service.Services;

namespace MilkStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private readonly IBlogService _blogService;

        public BlogController(IBlogService blogService)
        {
            _blogService = blogService;
        }

        [HttpGet]

        public async Task<IActionResult> GetAllBlogs(int pageIndex = 0, int pageSize = 10)
        {
            var roles = await _blogService.GetAllBlog(pageIndex, pageSize);
            return Ok(roles);
        }
        
    }
}
