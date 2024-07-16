using AutoMapper;
using Microsoft.AspNetCore.Identity;
using MilkStore.Domain.Entities;
using MilkStore.Domain.Enums;
using MilkStore.Repository.Interfaces;
using MilkStore.Service.Interfaces;
using MilkStore.Service.Models.ResponseModels;
using MilkStore.Service.Models.ViewModels.OrderViewDTO;

namespace MilkStore.Service.Services;

public class OrderService : IOrderService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly UserManager<Account> _userManager;
    private readonly IClaimsService _claimsService;
    public OrderService(IUnitOfWork unitOfWork, IMapper mapper, UserManager<Account> userManager, IClaimsService claimsService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _userManager = userManager;
        _claimsService = claimsService;
    }
    public async Task<ResponseModel> AddProductToCartAsync(CreateOrderDTO model)
    {
        // Map the incoming model to an Order entity
        var order = _mapper.Map<Order>(model);
        order.Status = OrderStatusEnums.InCart.ToString();
        order.PaymentStatus = OrderPaymentStatusEnums.UnPaid.ToString();
        order.Discount = 0;
        order.PointUsed = 0;
        order.PointSaved = 0;

        // Retrieve the current user's account ID
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
        order.AccountId = currentUserId.ToString();
        AddOrderDetailDTO addOrderDetailDto = new AddOrderDetailDTO();
        addOrderDetailDto.OrderId = order.Id;
        addOrderDetailDto.ProductId = model.ProductId;
        addOrderDetailDto.Quantity = 1;
        // await _unitOfWork.Prod
        // addOrderDetailDto.UnitPrice

        // Save the order to the database using the UnitOfWork
        await _unitOfWork.OrderRepository.AddAsync(order);
        await _unitOfWork.SaveChangeAsync();

        return new SuccessResponseModel<string>
        {
            Success = true,
            Message = "Product added to cart successfully.",
            Data = order.Id
        };
    }
}