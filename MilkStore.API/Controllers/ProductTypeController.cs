using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MilkStore.Service.Interfaces;
using MilkStore.Service.Models.ViewModels.ProductTypeViewModels;

namespace MilkStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductTypeController : ControllerBase
    {
        private readonly IProductTypeService _productTypeService;

        public ProductTypeController(IProductTypeService productTypeService)
        {
            _productTypeService = productTypeService;
        }

        [HttpGet("GetAllProductType")]
        public async Task<IActionResult> GetAllProductTypeAsync()
        {
            var productTypes = await _productTypeService.GetAllProductTypeAsync();
            if (productTypes != null)
            {
                return Ok(productTypes);
            }
            return BadRequest(productTypes);
        }

        [HttpGet("GetProductTypeById")]
        public async Task<IActionResult> GetProductTypeByIdAsync(int id)
        {
            var productType = await _productTypeService.GetProductTypeByIdAsync(id);
            if (productType != null)
            {
                return Ok(productType);
            }
            return BadRequest(productType);
        }

        [HttpPost("CreateProductType")]
        [Authorize(Roles = "Staff, Admin")]
        public async Task<IActionResult> CreateProductTypeAsync([FromForm]CreateProductTypeDTO model)
        {
            var response = await _productTypeService.CreateProductTypeAsync(model);
            if (response != null)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPost("UpdateProductType")]
        [Authorize(Roles = "Staff, Admin")]
        public async Task<IActionResult> UpdateProductTypeAsync([FromForm]UpdateProductTypeDTO model)
        {
            var response = await _productTypeService.UpdateProductTypeAsync(model);
            if (response != null)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPost("DeleteProductType")]
        [Authorize(Roles = "Staff, Admin")]
        public async Task<IActionResult> DeleteProductTypeAsync([FromForm]DeleteProductTypeDTO model)
        {
            var response = await _productTypeService.DeleteProductTypeAsync(model);
            if (response != null)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPost("RestoreProductType")]
        [Authorize(Roles = "Staff, Admin")]
        public async Task<IActionResult> RestoreProductTypeAsync([FromForm]RestoreProductTypeDTO model)
        {
            var response = await _productTypeService.RestoreProductTypeAsync(model);
            if (response != null)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
    }
}
