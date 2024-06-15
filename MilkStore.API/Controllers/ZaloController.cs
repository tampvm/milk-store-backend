using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MilkStore.Service.Interfaces;
using static MilkStore.Service.Models.ResponseModels.ZaloResponseModel;

namespace MilkStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ZaloController : ControllerBase
    {
        private readonly IZaloService _zaloService;

        public ZaloController(IZaloService zaloService)
        {
            _zaloService = zaloService;
        }

        [HttpPost]
        public async Task<IActionResult> SendZaloMessage(string phoneNumber)
        {
            var result = await _zaloService.SendVerificationCodeAsync(phoneNumber);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
