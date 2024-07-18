using AutoMapper;
using Microsoft.AspNetCore.Identity;
using MilkStore.Domain.Entities;
using MilkStore.Domain.Enums;
using MilkStore.Repository.Interfaces;
using MilkStore.Service.Interfaces;
using MilkStore.Service.Models.ResponseModels;
using MilkStore.Service.Models.ViewModels.CartViewModel;

namespace MilkStore.Service.Services;

public class CartService : ICartService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly UserManager<Account> _userManager;
    private readonly IClaimsService _claimsService;
    public CartService(IUnitOfWork unitOfWork, IMapper mapper, UserManager<Account> userManager, IClaimsService claimsService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _userManager = userManager;
        _claimsService = claimsService;
    }
    public async Task<ResponseModel> AddProductToCartAsync(CartDTO model)
    {
        var cart = _mapper.Map<Cart>(model);
        var currentUserId = _claimsService.GetCurrentUserId();
        if (string.IsNullOrEmpty(currentUserId.ToString()))
        {
            return new ResponseModel
            {
                Success = false,
                Message = "User is not authenticated."
            };
        }

        // Set the account ID to the order
        cart.AccountId = currentUserId.ToString();
        cart.ProductId = model.ProductId;
        cart.Status = CartStatusEnum.InCart.ToString();
        await _unitOfWork.CartRepository.AddAsync(cart);
        await _unitOfWork.SaveChangeAsync();

        return new SuccessResponseModel<string>
        {
            Success = true,
            Message = "Product added to cart successfully.",
            Data = cart.Product.Name
        };
    }

    public async Task<ResponseModel> GetCartByAccountID(int pageIndex, int pageSize)
    {
        var currentUserId = _claimsService.GetCurrentUserId();
        if (string.IsNullOrEmpty(currentUserId.ToString()))
        {
            return new ResponseModel
            {
                Success = false,
                Message = "User is not authenticated."
            };
        }
        var carts = await _unitOfWork.CartRepository.GetCartsByAccountIdAsync(currentUserId.ToString(), pageIndex, pageSize);

        if (carts == null || !carts.Any())
        {
            return new ErrorResponseModel<object>
            {
                Success = false,
                Message = "No carts found for this account.",
                Errors = new List<string> { "No carts available" }
            };
        }

        var cartViewModels = _mapper.Map<List<CartDTO>>(carts);

        return new SuccessResponseModel<List<CartDTO>>
        {
            Success = true,
            Message = "Carts retrieved successfully.",
            Data = cartViewModels
        };
    }


    public Task<ResponseModel> UpdateCartByID(int Id, CartDTO model)
    {
        throw new NotImplementedException();
    }
}