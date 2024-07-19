using Microsoft.AspNetCore.Mvc;
using MilkStore.Service.Interfaces;
using MilkStore.Service.Models.ResponseModels;
using MilkStore.Service.Models.ViewModels.OrderViewDTO;
using System.Threading.Tasks;

namespace MilkStore.API.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        [HttpPost("add-to-cart")]
        public async Task<IActionResult> AddProductToCart([FromBody] CreateOrderDTO model)
        {
            if (model == null)
            {
                return BadRequest(new ResponseModel
                {
                    Success = false,
                    Message = "Invalid order data."
                });
            }

            var result = await _orderService.AddProductToCartAsync(model);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpGet("detail/{orderId}")]
        public async Task<IActionResult> GetOrderDetail(int orderId, [FromQuery] int pageIndex, [FromQuery] int pageSize)
        {
            var result = await _orderService.GetOrderDetail(orderId, pageIndex, pageSize);

            if (!result.Success)
            {
                return NotFound(result);
            }

            return Ok(result);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllOrder([FromQuery] int pageIndex, [FromQuery] int pageSize)
        {
            var result = await _orderService.GetAllOrder(pageIndex, pageSize);

            if (!result.Success)
            {
                return NotFound(result);
            }

            return Ok(result);
        }

        [HttpGet("payment/status/{orderId}")]
        public async Task<IActionResult> CheckPaymentStatus(string orderId)
        {
            var result = await _orderService.CheckPaymentStatus(orderId);

            if (!result.Success)
            {
                return NotFound(result);
            }

            return Ok(result);
        }
    }
}