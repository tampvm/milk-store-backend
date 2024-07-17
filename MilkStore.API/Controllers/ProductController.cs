using Microsoft.AspNetCore.Mvc;
using MilkStore.Service.Interfaces;
using MilkStore.Service.Models.ViewModels.ProductViewModels;

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
        [Route("GetAllProducts")]
        public async Task<IActionResult> GetAllProductAsync()
        {
            var response = await _productService.GetAllProductsAsync();
            if (response != null)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet]
        [Route("GetProductsPagination")]
        public async Task<IActionResult> GetProductsPaginationAsync(int pageIndex = 0, int pageSize = 10)
        {
            var response = await _productService.GetProductsPaginationAsync(pageIndex, pageSize);
            if (response != null)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet]
        [Route("GetProductById")]
        public async Task<IActionResult> GetProductByIdAsync(String id)
        {
            var response = await _productService.GetProductByIdAsync(id);
            if (response != null)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPost]
        [Route("CreateProduct")]
        public async Task<IActionResult> CreateProductAsync([FromForm]CreateProductDTO model, IFormFile image, IFormFile thumbnail)
        {
            var response = await _productService.CreateProductAsync(model, image, thumbnail);
            if (response != null)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
    }
}
