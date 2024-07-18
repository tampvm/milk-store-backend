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

        // Check if there is an existing cart item with the same AccountId, ProductId, and Status InCart
        var existingCartItem = await _unitOfWork.CartRepository
            .GetCartItemAsync(cart.AccountId, cart.ProductId, CartStatusEnum.InCart.ToString());

        if (existingCartItem != null)
        {
            // If the cart item exists, update the quantity
            existingCartItem.Quanity += model.Quanity; // Update this to the appropriate increment if Quantity is 1 in the model
            _unitOfWork.CartRepository.Update(existingCartItem);
        }
        else
        {
            // If the cart item does not exist, add a new cart item
            cart.Status = CartStatusEnum.InCart.ToString();
            await _unitOfWork.CartRepository.AddAsync(cart);
        }

        await _unitOfWork.SaveChangeAsync();

        return new SuccessResponseModel<string>
        {
            Success = true,
            Message = "Product added to cart successfully.",
            Data = model.ProductId
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


    public async Task<ResponseModel> UpdateCartByID(int id, CartDTO model , int khonglatang1lagiam)
    {
        // Lấy mục giỏ hàng hiện tại dựa trên Id
        var cartItem = await _unitOfWork.CartRepository.GetByIdAsync(id);
        if (cartItem == null)
        {
            return new ResponseModel
            {
                Success = false,
                Message = "Cart item not found."
            };
        }

        // Kiểm tra trạng thái của mục giỏ hàng
        if (cartItem.Status != CartStatusEnum.InCart.ToString())
        {
            return new ResponseModel
            {
                Success = false,
                Message = "Only items with status 'InCart' can be updated."
            };
        }

        // Cập nhật số lượng sản phẩm
        if (khonglatang1lagiam == 1)
        {
            cartItem.Quanity -= model.Quanity;
        }
        else
        {
            cartItem.Quanity += model.Quanity;
        }
        
        var quantity = cartItem.Quanity - model.Quanity;
        if (quantity <= 0)
        {
            _unitOfWork.CartRepository.Delete(id);
        }
        else
        {
            _unitOfWork.CartRepository.Update(cartItem);
        }
        

        // Cập nhật mục giỏ hàng trong cơ sở dữ liệu
        //_unitOfWork.CartRepository.Update(cartItem);
        await _unitOfWork.SaveChangeAsync();

        return new SuccessResponseModel<string>
        {
            Success = true,
            Message = "Cart item updated successfully.",
            Data = cartItem.ProductId.ToString()
        };
    }

}