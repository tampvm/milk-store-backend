using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MilkStore.Service.Interfaces;
using MilkStore.Service.Models.ViewModels.AgeRangeViewModels;

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

        [HttpGet("GetAgeRangeById")]
        public async Task<IActionResult> GetAgeRangeByIdAsync(int id)
        {
            var ageRange = await _ageRangeService.GetAgeRangeByIdAsync(id);
            if (ageRange != null)
            {
                return Ok(ageRange);
            }
            return BadRequest();
        }

        [HttpPost("CreateAgeRange")]
        [Authorize(Roles = "Staff, Admin")]
        public async Task<IActionResult> CreateAgeRangeAsync([FromForm]CreateAgeRangeDTO model)
        {
            var ageRange = await _ageRangeService.CreateAgeRangeAsync(model);
            if (ageRange != null)
            {
                return Ok(ageRange);
            }
            return BadRequest();
        }

        [HttpPost("UpdateAgeRange")]
        [Authorize(Roles = "Staff, Admin")]
        public async Task<IActionResult> UpdateAgeRangeAsync([FromForm]UpdateAgeRangeDTO model)
        {
            var ageRange = await _ageRangeService.UpdateAgeRangeAsync(model);
            if (ageRange != null)
            {
                return Ok(ageRange);
            }
            return BadRequest();
        }

        [HttpPost("DeleteAgeRange")]
        [Authorize(Roles = "Staff, Admin")]
        public async Task<IActionResult> DeleteAgeRangeAsync([FromForm]DeleteAgeRangeDTO model)
        {
            var ageRange = await _ageRangeService.DeleteAgeRangeAsync(model);
            if (ageRange != null)
            {
                return Ok(ageRange);
            }
            return BadRequest();
        }

        [HttpPost("RestoreAgeRange")]
        [Authorize(Roles = "Staff, Admin")]
        public async Task<IActionResult> RestoreAgeRangeAsync([FromForm]RestoreAgeRangeDTO model)
        {
            var ageRange = await _ageRangeService.RestoreAgeRangeAsync(model);
            if (ageRange != null)
            {
                return Ok(ageRange);
            }
            return BadRequest();
        }
    }
}
