using Microsoft.AspNetCore.Mvc;
using MilkStore.Repository.Interfaces;
using MilkStore.Service.Interfaces;
using MilkStore.Service.Models.ResponseModels;
using MilkStore.Service.Models.ViewModels.CartViewModel;

namespace MilkStore.API.Controllers;

public class CartController : BaseController
{
    private readonly ICartService _cartService;

    public CartController(ICartService cartService)
    {
        _cartService = cartService;
    }
    // GET
    [HttpPost("add-to-cart")]
    public async Task<IActionResult> AddProductToCart([FromBody] CartDTO model)
    {
        if (model == null)
        {
            return BadRequest(new ResponseModel
            {
                Success = false,
                Message = "Invalid order data."
            });
        }

        var result = await _cartService.AddProductToCartAsync(model);

        if (!result.Success)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }
    [HttpGet("get-by-account-id")]
    public async Task<IActionResult> GetCartByAccountID([FromQuery] int pageIndex, [FromQuery] int pageSize)
    {

        var result = await _cartService.GetCartByAccountID(pageIndex, pageSize);

        if (!result.Success)
        {
            return NotFound(result);
        }

        return Ok(result);
    }
}