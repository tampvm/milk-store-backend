using Microsoft.AspNetCore.Mvc;
using MilkStore.Service.Interfaces;
using MilkStore.Service.Models.ViewModels.ProductImageViewModels;

namespace MilkStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductImageController : ControllerBase
    {
        private readonly IProductImageService _productImageService;

        public ProductImageController(IProductImageService productImageService) {
            _productImageService = productImageService;
        }

        [HttpGet]
        [Route("GetProductImagesById")]
        public async Task<IActionResult> GetProductImagesAsync(string productImageId)
        {
            var response = await _productImageService.GetProductImagesAsync(productImageId);
            if (response != null)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPost]
        [Route("CreateProductImage")]
        public async Task<IActionResult> CreateProductImageAsync([FromForm]CreateProductImageDTO model, IFormFile imageFile, IFormFile thumbnailFile)
        {
            var response = await _productImageService.CreateProductImageAsync(model, imageFile, thumbnailFile);
            if (response != null)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPost]
        [Route("UpdateProductImage")]
        public async Task<IActionResult> UpdateProductImageAsync([FromForm]UpdateProductImageDTO model, IFormFile? imageFile = null, IFormFile? thumbnailFile = null)
        {
            var response = await _productImageService.UpdateProductImageAsync(model, imageFile, thumbnailFile);
            if (response != null)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPost]
        [Route("DeleteProductImage")]
        public async Task<IActionResult> DeleteProductImageAsync([FromForm]DeleteProductImageDTO model)
        {
            var response = await _productImageService.DeleteProductImageAsync(model);
            if (response != null)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
    }
}
