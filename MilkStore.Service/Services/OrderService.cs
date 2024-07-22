using AutoMapper;
using Microsoft.AspNetCore.Identity;
using MilkStore.Domain.Entities;
using MilkStore.Domain.Enums;
using MilkStore.Repository.Common;
using MilkStore.Repository.Interfaces;
using MilkStore.Service.Interfaces;
using MilkStore.Service.Models.ResponseModels;
using MilkStore.Service.Models.ViewModels.OrderViewDTO;
using Net.payOS;
using Net.payOS.Types;

namespace MilkStore.Service.Services;

public class OrderService : IOrderService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly UserManager<Account> _userManager;
    private readonly IClaimsService _claimsService;
    private readonly PayOS _payOS;
    public OrderService(IUnitOfWork unitOfWork, IMapper mapper, UserManager<Account> userManager, IClaimsService claimsService, PayOS payOs)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _userManager = userManager;
        _claimsService = claimsService;
        _payOS = payOs;
    }
    private static Random random = new Random();

    public static string GenerateRandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }
    public async Task<ResponseModel> AddProductToCartAsync(CreateOrderDTO model)
{
    decimal totalAmount = 0;
    List<ItemData> items1 = new List<ItemData>();
    List<OrderDetail> orderDetails = new List<OrderDetail>();
    

    foreach (var cartId in model.cartIds)
    {
        // Get cart item by cartId
        var cartItem = await _unitOfWork.CartRepository.GetByIdAsync(cartId);
        if (cartItem == null)
        {
            return new ResponseModel
            {
                Success = false,
                Message = $"Cart item with ID {cartId} not found."
            };
        }

        var pointUser = await _unitOfWork.PointRepository.GetPointsByAccountIdAsync(_claimsService.GetCurrentUserId()
            .ToString(),1,1);
        
        cartItem.Status = CartStatusEnum.InOrder.ToString();
        _unitOfWork.CartRepository.Update(cartItem);

        // Get price of the product
        var product = await _unitOfWork.ProductRepository.GetByIdAsync(cartItem.ProductId);
        if (product == null)
        {
            return new ResponseModel
            {
                Success = false,
                Message = $"Product with ID {cartItem.ProductId} not found."
            };
        }
        

        // Add price of the product to total amount
        // OrderDetail orderDetail = new OrderDetail();
        // orderDetail
        
        decimal pointUsed = model.PointUsed ?? 0; // Handle nullable PointUsed
        //int pointUsedInt = model.PointUsed ?? 0;
        // if ( Decimal.Parse(pointUser) < pointUsed)
        // {
        //     
        // }
        //if(pointUser.Sum())
        totalAmount += (product.Price * cartItem.Quanity) - pointUsed;
        int orderCode1 = int.Parse(DateTimeOffset.Now.ToString("ffffff"));
        ItemData item1 = new ItemData(product.Name, cartItem.Quanity, Convert.ToInt32(totalAmount));
        items1.Add(item1);
        OrderDetail orderDetail = new OrderDetail
        {
            ProductId = product.Id,
            Quantity = cartItem.Quanity,
            UnitPrice = product.Price
            // Add any other relevant properties from cartItem or product
        };
        orderDetails.Add(orderDetail);
    }
    
    

    // Assign total amount to model
    //model.TotalAmount = Convert.ToInt32(totalAmount);

    // Map the incoming model to an Order entity
    var order = _mapper.Map<Order>(model);
    //order.Id = GenerateRandomString(10);
    order.Status = OrderStatusEnums.Waiting.ToString();
    order.PaymentStatus = OrderPaymentStatusEnums.UnPaid.ToString();
    order.CreatedAt = DateTime.Now;
    order.Type = OrderTypeEnums.Order.ToString();
    
    order.PointSaved = order.TotalAmount / 100;
    order.TotalAmount = Convert.ToInt32(totalAmount); 

    

    // Retrieve the current user's account ID
    var currentUserId = _claimsService.GetCurrentUserId().ToString();
    if (string.IsNullOrEmpty(currentUserId))
    {
        return new ResponseModel
        {
            Success = false,
            Message = "User is not authenticated."
        };
    }

    // Set the account ID to the order
    order.AccountId = currentUserId;

    if (!string.IsNullOrEmpty(model.VoucherCode))
    {
        var voucher = await _unitOfWork.VoucherRepository.GetVoucherByCodeAsync(model.VoucherCode);
        if (voucher != null && voucher.Code.Equals(model.VoucherCode, StringComparison.OrdinalIgnoreCase))
        {
            // Validate voucher (e.g., check if it's still valid, not expired, etc.)
            
                var accountVoucher = await _unitOfWork.VoucherRepository.AddAccountVoucher(
                    currentUserId, 
                    voucher.Id, 
                    DateTime.Now.ToString(),
                    AccountVoucherStatusEnums.Used.ToString()
                );
                order.AccountVoucherId = accountVoucher.Id;

                // Apply voucher discount
                decimal discountAmount = CalculateDiscountAmount(voucher, totalAmount);
                totalAmount -= discountAmount;
                order.TotalAmount = Convert.ToInt32(totalAmount);

                // Increment UsedCount
                voucher.UsedCount = (voucher.UsedCount ?? 0) + 1;
                _unitOfWork.VoucherRepository.Update(voucher);
            
        }
        else
        {
            return new ResponseModel
            {
                Success = false,
                Message = "The provided voucher code does not exist or is incorrect."
            };
        }
    }

    int orderCode = int.Parse(DateTimeOffset.Now.ToString("ffffff"));
    String confirmTransactionUrl = String.Format("http://localhost:5173/confirm-transaction/%s", orderCode);
    PaymentData paymentData = new PaymentData(orderCode, order.TotalAmount, "thanh toanbs", items1, $"http://localhost:5173/confirm-transaction/{orderCode}", $"http://localhost:5173/confirm-transaction/{orderCode}");
    CreatePaymentResult createPayment = await _payOS.createPaymentLink(paymentData);
    order.Id = createPayment.orderCode.ToString();

    // Save the order to the database using the UnitOfWork
    await _unitOfWork.OrderRepository.AddAsync(order);
    foreach (var detail in orderDetails)
    {
        detail.OrderId = order.Id; // Assuming OrderId is a foreign key in OrderDetail
        await _unitOfWork.OrderDetailRepository.AddAsync(detail);
    }

    Point point = new Point();
    point.AccountId = order.AccountId;
    point.OrderId = order.Id;
    point.TransactionType = "received point";
    point.Points = order.PointSaved;
    await _unitOfWork.PointRepository.AddAsync(point);
    await _unitOfWork.SaveChangeAsync();

    return new SuccessResponseModel<string>
    {
        Success = true,
        Message = "Order created successfully.",
        Data = order.Id + " " + createPayment.checkoutUrl
    };
}
    private bool IsVoucherValid(Voucher voucher)
    {
        // Implement your voucher validation logic here
        // For example:
        return voucher.Code == "Active" 
               && voucher.StartDate <= DateTime.Now 
               && voucher.EndDate >= DateTime.Now 
               && (!voucher.UsageLimit.HasValue || voucher.UsedCount < voucher.UsageLimit);
    }

    private decimal CalculateDiscountAmount(Voucher voucher, decimal totalAmount)
    {
        if (voucher.DiscountType == "Percentage")
        {
            return totalAmount * (voucher.DiscountValue / 100);
        }
        else // Assume it's a fixed amount
        {
            return Math.Min(voucher.DiscountValue, totalAmount); // Ensure discount doesn't exceed total amount
        }
    }

    public async Task<ResponseModel> CheckPaymentStatus(string orderId)
    {
        PaymentLinkInformation paymentLinkInformation = await _payOS.getPaymentLinkInformation(int.Parse(orderId) );
        if (paymentLinkInformation.status.Equals("PAID"))
        {
            var order1 = await _unitOfWork.OrderRepository.GetByIdAsync(orderId);
            order1.PaymentStatus = OrderPaymentStatusEnums.Paid.ToString();
            order1.Status = OrderStatusEnums.Preparing.ToString();
            await _unitOfWork.SaveChangeAsync();
            return new SuccessResponseModel<string>
            {
                Success = true,
                Message = "Order Paid successfully.",
                Data = order1.Id
            };
        }
        else
        {
            return new SuccessResponseModel<string>
            {
                Success = true,
                Message = "Order Paid fail.",
                Data = null
            };
        }
    }

    public async Task<ResponseModel> GetAllOrder(int pageIndex, int pageSize)
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
        var carts = await _unitOfWork.OrderRepository.GetOrderByAccountIdAsync(currentUserId.ToString(), pageIndex, pageSize);

        if (carts == null || !carts.Any())
        {
            return new ErrorResponseModel<object>
            {
                Success = false,
                Message = "No order found for this account.",
                Errors = new List<string> { "No order available" }
            };
        }

        var cartViewModels = _mapper.Map<List<CreateOrderDTO>>(carts);

        return new SuccessResponseModel<List<CreateOrderDTO>>
        {
            Success = true,
            Message = "Order retrieved successfully.",
            Data = cartViewModels
        };
    }

    public async Task<ResponseModel> GetOrderDetail(int orderId, int pageIndex, int pageSize)
    {
        var orderDetails = await _unitOfWork.OrderDetailRepository.GetOrderItemByOrderIdAsync(orderId.ToString(), pageIndex, pageSize);

        if (orderDetails == null || orderDetails.Count == 0)
        {
            return new ResponseModel
            {
                Success = false,
                Message = "Order details not found."
            };
        }

        return new SuccessResponseModel<List<OrderDetail>>
        {
            Success = true,
            Message = "Order details retrieved successfully.",
            Data = orderDetails
        };
    }
}