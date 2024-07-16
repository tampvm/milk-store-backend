using Microsoft.AspNetCore.Mvc;
using MilkStore.Service.Interfaces;

namespace MilkStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        [Route("getall")]
        public async Task<IActionResult> GetAllProductAsync()
        {
            var response = await _productService.GetAllProductsAsync();
            if (response != null)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        //[HttpGet]
        //[Route("getproductspagination")]
        //public async Task<IActionResult> GetProductsPaginationAsync(int pageIndex = 0, int pageSize = 10)
        //{
        //    var response = await _productService.GetProductsPaginationAsync(pageIndex, pageSize);
        //    if (response != null)
        //    {
        //        return Ok(response);
        //    }
        //    return BadRequest(response);
        //}

        [HttpGet]
        [Route("getbyid")]
        public async Task<IActionResult> GetProductByIdAsync(String id)
        {
            var response = await _productService.GetProductByIdAsync(id);
            if (response != null)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
    }
}
