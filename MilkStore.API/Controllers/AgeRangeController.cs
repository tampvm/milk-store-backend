using Microsoft.AspNetCore.Mvc;
using MilkStore.Service.Interfaces;

namespace MilkStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgeRangeController : ControllerBase
    {
        private readonly IAgeRangeService _ageRangeService;

        public AgeRangeController(IAgeRangeService ageRangeService)
        {
            _ageRangeService = ageRangeService;
        }

        [HttpGet("GetAllAgeRange")]
        public async Task<IActionResult> GetAllAgeRangeAsync()
        {
            var ageRanges = await _ageRangeService.GetAllAgeRangeAsync();
            if (ageRanges != null)
            {
                return Ok(ageRanges);
            }
            return BadRequest();
        }
    }
}
