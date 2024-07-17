using Microsoft.AspNetCore.Mvc;
using MilkStore.Service.Interfaces;
using MilkStore.Service.Models.ResponseModels;
using MilkStore.Service.Models.ViewModels.OrderViewDTO;

namespace MilkStore.API.Controllers;

public class OrderController : BaseController
{
    // GET
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
}