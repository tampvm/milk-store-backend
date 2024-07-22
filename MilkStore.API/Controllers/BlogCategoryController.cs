using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MilkStore.Service.Interfaces;
using MilkStore.Service.Models.ViewModels.BlogCategoryViewModels;

namespace MilkStore.API.Controllers
{
  
    public class BlogCategoryController : BaseController
    {
        private readonly IBlogCategoryService _blogCategoryService;

        public BlogCategoryController(IBlogCategoryService blogService)
        {
            _blogCategoryService = blogService;
        }
        [HttpGet]
        // Get all blog categories
        public async Task<IActionResult> GetAllBlogCategories(int pageIndex = 0, int pageSize = 10)
        {
            var blogCategories = await _blogCategoryService.GetAllBlogCategory(pageIndex, pageSize);
            return Ok(blogCategories);
        }
        [HttpGet]
        // Get blog category by id
        public async Task<IActionResult> GetBlogCategoryById(int id)
        {
            var blogCategory = await _blogCategoryService.GetBlogCategoryById(id);
            return Ok(blogCategory);
        }
        [HttpPost]

        public async Task<IActionResult> CreateBlogCategory([FromBody] CreateBlogCategoryDTO model)
        {
            // Check if the model state is valid
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _blogCategoryService.CreateBlogCategory(model);

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
        public async Task<IActionResult> UpdateBlogCategory(int id, [FromBody] UpdateBlogCategoryDTO model)
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
            var response = await _blogCategoryService.UpdateBlogCategory(model, id);

            if (response.Success)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteBlogCategory(int id)
        {
            var response = await _blogCategoryService.DeleteBlogCategory(id);

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
