using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Roles = "Staff, Admin")]
        [Route("CreateProductImage")]
        public async Task<IActionResult> CreateProductImageAsync([FromForm]CreateProductImageDTO model, List<IFormFile> imageFiles, IFormFile thumbnailFile)
        {
            var response = await _productImageService.CreateProductImageAsync(model, imageFiles, thumbnailFile);
            if (response != null)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPost]
        [Authorize(Roles = "Staff, Admin")]
        [Route("UpdateProductImage")]
        public async Task<IActionResult> UpdateProductImageAsync([FromForm]UpdateProductImageDTO model, List<IFormFile>? imageFiles = null, IFormFile? thumbnailFile = null)
        {
            var response = await _productImageService.UpdateProductImageAsync(model, imageFiles, thumbnailFile);
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
