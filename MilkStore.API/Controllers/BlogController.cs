using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MilkStore.Service.Interfaces;
using MilkStore.Service.Models.ViewModels.BogViewModel;
using MilkStore.Service.Services;

namespace MilkStore.API.Controllers
{
  
    public class BlogController : BaseController
    {
        private readonly IBlogService _blogService;

        public BlogController(IBlogService blogService)
        {
            _blogService = blogService;
        }

        [HttpGet]

        public async Task<IActionResult> GetAllBlogs(int pageIndex = 0, int pageSize = 10)
        {
            var blogs = await _blogService.GetAllBlog(pageIndex, pageSize);
            return Ok(blogs);
        }
        [HttpPost]
        public async Task<IActionResult> CreateBlog([FromBody] CreateBlogDTO model)
        {
            // Check if the model state is valid
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _blogService.CreateBlog(model);

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
        public async Task<IActionResult> UpdateBlog(int id, [FromBody] UpdateBlogDTO model)
        {
            if (model == null)
            {
                return BadRequest("Update model cannot be null.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Call the service to update the blog
            var response = await _blogService.UpdateBlog(model, id);

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
        public async Task<IActionResult> DeleteBlog(int id, string deleteBy)
        {
            var response = await _blogService.DeleteBlog(id, deleteBy);

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
        public async Task<IActionResult> GetBlogByUserId(string id, int postId, int pageIndex = 0, int pageSize = 10)
        {
            var blogs = await _blogService.GetBlogByUserId(pageIndex, pageSize, id, postId);
            return Ok(blogs);
        }
        //[HttpGet("getByUserIdWithoutImg/{id}")]
        //public async Task<IActionResult> GetBlogByUserIdWithoutImg(int id)
        //{
        //    var blogs = await _blogService.GetBlogByUserIdWithouImg(id);
        //    return Ok(blogs);
        //}
        [HttpPost]
        public async Task<IActionResult> CreateBlogImg([FromBody] CreateBlogImgDTO model)
        {
            // Check if the model state is valid
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _blogService.CreateBlogImg(model);

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
        public async Task<IActionResult> GetBlogByBlogId(int blogId)
        {
            var blogs = await _blogService.GetBlogByBlogId(blogId);
            return Ok(blogs);
        }


    }
}
