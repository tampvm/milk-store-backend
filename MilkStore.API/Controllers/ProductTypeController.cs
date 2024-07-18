using Microsoft.AspNetCore.Mvc;
using MilkStore.Service.Interfaces;

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
    }
}
