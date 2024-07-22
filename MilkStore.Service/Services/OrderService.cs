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
    int orderCode = int.Parse(DateTimeOffset.Now.ToString("ffffff"));
    
    PaymentData paymentData = new PaymentData(orderCode, order.TotalAmount, "thanh toanbs", items1, "https://www.youtube.com/watch?v=Z8vDU6vUTj4", "http://localhost:5095/swagger/index.html");
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

    public async Task<ResponseModel> GetAllOrderAsync(string? orderId, string? status, int pageIndex, int pageSize)
    {
        // Xác định điều kiện lọc trạng thái
        var statusFilter = string.IsNullOrEmpty(status) || status == "All" ? (string?)null : status;

        // Lấy danh sách đơn hàng từ repository
        var orderEntities = await _unitOfWork.OrderRepository.GetAsync(
            filter: o => (string.IsNullOrEmpty(orderId) || o.Id.Contains(orderId)) &&
                         (statusFilter == null || o.Status.Contains(statusFilter)),
            includeProperties: "OrderDetails,OrderDetails.Product,OrderDetails.Product.ProductImages,Account,AccountVoucher.Voucher,Points", // Bao gồm các liên kết cần thiết
            pageIndex: pageIndex,
            pageSize: pageSize
        );

        // Chuyển đổi các thực thể đơn hàng thành DTO
        var viewOrderHistories = orderEntities.Items.Select(o => new ViewOrderHistoryDTO
        {
            OrderId = o.Id,
            OrderDate = o.CreatedAt, // OrderDate từ CreatedDate
            Customer = new CustomerDTO
            {
                Name = o.Account.LastName + " " + o.Account.FirstName,
                Phone = o.Account.PhoneNumber
            },
            TotalAmount = o.TotalAmount,
            Status = o.Status,
            Type = o.Type,
            PaymentMethod = o.PaymentMethod,
            PaymentStatus = o.PaymentStatus,
            Recipient = new RecipientDTO
            {
                Name = o.Account.LastName + " " + o.Account.FirstName,
                Phone = o.Account.PhoneNumber,
                Address = o.ShippingAddress
            },
            Payment = new PaymentDTO
            {
                Cash = "0 ₫",
                VnpayQR = "0 ₫",
                Momo = "0 ₫",
                Paypal = "0 ₫",
                Subtotal = o.TotalAmount.ToString(),
                Discount = o.Discount.ToString(),
                ShippingFee = "Miễn phí",
                Coupon = o.AccountVoucher != null ? o.AccountVoucher.Voucher.Code : "0 ₫", // Lấy mã voucher
                Points = o.PointUsed.HasValue ? o.PointUsed.Value.ToString() : "0",
                Total = o.TotalAmount.ToString()
            },
            Products = o.OrderDetails.Select(d => new ProductDTO
            {
                Id = d.ProductId,
                Image = d.Product.ProductImages.FirstOrDefault()?.Image.ImageUrl ?? "",
                Name = d.Product.Name,
                Quantity = d.Quantity,
                UnitPrice = d.UnitPrice,
                TotalPrice = d.UnitPrice * d.Quantity
            }).ToList()
        }).ToList();

        // Tạo đối tượng phân trang
        var pagination = new Pagination<ViewOrderHistoryDTO>
        {
            TotalItemsCount = await _unitOfWork.OrderRepository.CountAsync(
                filter: o => (string.IsNullOrEmpty(orderId) || o.Id.Contains(orderId)) &&
                             (statusFilter == null || o.Status.Contains(statusFilter))
            ),
            PageSize = pageSize,
            PageIndex = pageIndex,
            Items = viewOrderHistories
        };

        return new SuccessResponseModel<Pagination<ViewOrderHistoryDTO>>
        {
            Success = true,
            Message = "Orders retrieved successfully.",
            Data = pagination
        };
    }

    public async Task<ResponseModel> GetOrderByIdAsync(string orderId)
    {
        var orderEntity = await _unitOfWork.OrderRepository.GetByIdAsync(
            id: orderId,
            includeProperties: "OrderDetails,OrderDetails.Product,OrderDetails.Product.ProductImages,Account,AccountVoucher.Voucher,Points"
        );

        // Kiểm tra nếu không tìm thấy đơn hàng
        if (orderEntity == null)
        {
            return new ResponseModel
            {
                Success = false,
                Message = "Order not found."
            };
        }

        // Chuyển đổi thực thể đơn hàng thành DTO
        var viewOrderHistoryDetail = new ViewOrderHistoryDetailDTO
        {
            OrderId = orderEntity.Id,
            OrderDate = orderEntity.CreatedAt, // OrderDate từ CreatedDate
            Status = orderEntity.Status,
            Customer = new CustomerDTO
            {
                Name = orderEntity.Account.LastName + " " + orderEntity.Account.FirstName,
                Phone = orderEntity.Account.PhoneNumber
            },
            Recipient = new RecipientDTO
            {
                Name = orderEntity.Account.LastName + " " + orderEntity.Account.FirstName,
                Phone = orderEntity.Account.PhoneNumber,
                Address = orderEntity.ShippingAddress
            },
            Payment = new PaymentDTO
            {
                Cash = "0 ₫",
                VnpayQR = "0 ₫",
                Momo = "0 ₫",
                Paypal = "0 ₫",
                Subtotal = orderEntity.TotalAmount.ToString(),
                Discount = orderEntity.Discount.ToString(),
                ShippingFee = "Miễn phí",
                Coupon = orderEntity.AccountVoucher != null ? orderEntity.AccountVoucher.Voucher.Code : "0 ₫", // Lấy mã voucher
                Points = orderEntity.PointUsed.HasValue ? orderEntity.PointUsed.Value.ToString() : "0",
                Total = orderEntity.TotalAmount.ToString()
            },
            Products = orderEntity.OrderDetails.Select(d => new ProductDTO
            {
                Id = d.ProductId,
                Image = d.Product.ProductImages.FirstOrDefault()?.Image.ImageUrl ?? "",
                Name = d.Product.Name,
                Quantity = d.Quantity,
                UnitPrice = d.UnitPrice,
                TotalPrice = d.UnitPrice * d.Quantity
            }).ToList()
        };

        return new SuccessResponseModel<ViewOrderHistoryDetailDTO>
        {
            Success = true,
            Message = "Order retrieved successfully.",
            Data = viewOrderHistoryDetail
        };
    }

    //public async Task<ResponseModel> GetOrderByUserIdAsync(string userId, int pageIndex, int pageSize)
    //{
    //    // Lấy danh sách đơn hàng của người dùng từ repository
    //    var orderEntities = await _unitOfWork.OrderRepository.GetAsync(
    //        filter: o => o.AccountId == userId,
    //        includeProperties: "",
    //        pageIndex: pageIndex,
    //        pageSize: pageSize
    //    );

    //    // Chuyển đổi các thực thể đơn hàng thành DTO
    //    var viewOrderHistoriesByUser = orderEntities.Items.Select(o => new ViewOrderHistoryByUserDTO
    //    {
    //        OrderId = o.Id,
    //        OrderDate = o.CreatedAt, // OrderDate từ CreatedDate
    //        TotalAmount = o.TotalAmount,
    //        Status = o.Status
    //    }).ToList();

    //    return new SuccessResponseModel<List<ViewOrderHistoryByUserDTO>>
    //    {
    //        Success = true,
    //        Message = "Orders retrieved successfully.",
    //        Data = viewOrderHistoriesByUser
    //    };
    //}
    public async Task<ResponseModel> GetOrderByUserIdAsync(string userId, int pageIndex, int pageSize)
    {
        // Xác định tổng số đơn hàng của người dùng để phân trang
        var totalItemsCount = await _unitOfWork.OrderRepository.CountAsync(o => o.AccountId == userId);

        // Lấy danh sách đơn hàng của người dùng từ repository
        var orderEntities = await _unitOfWork.OrderRepository.GetAsync(
            filter: o => o.AccountId == userId,
            includeProperties: "",
            pageIndex: pageIndex,
            pageSize: pageSize
        );

        // Chuyển đổi các thực thể đơn hàng thành DTO
        var viewOrderHistoriesByUser = orderEntities.Items.Select(o => new ViewOrderHistoryByUserDTO
        {
            OrderId = o.Id,
            OrderDate = o.CreatedAt, // OrderDate từ CreatedDate
            TotalAmount = o.TotalAmount,
            Status = o.Status
        }).ToList();

        // Tạo đối tượng phân trang
        var pagination = new Pagination<ViewOrderHistoryByUserDTO>
        {
            TotalItemsCount = totalItemsCount,
            PageSize = pageSize,
            PageIndex = pageIndex,
            Items = viewOrderHistoriesByUser
        };

        return new SuccessResponseModel<Pagination<ViewOrderHistoryByUserDTO>>
        {
            Success = true,
            Message = "Orders retrieved successfully.",
            Data = pagination
        };
    }

    public async Task<ResponseModel> ChangeStatusAsync(string orderId, string status)
    {
        // Lấy đơn hàng từ repository
        var order = await _unitOfWork.OrderRepository.GetByIdAsync(orderId);
        if (order == null)
        {
            return new ErrorResponseModel<object>
            {
                Success = false,
                Message = "Order not found.",
                Errors = new List<string> { "Order not found" }
            };
        }

        // Cập nhật trạng thái đơn hàng
        order.Status = status;
        _unitOfWork.OrderRepository.Update(order);
        await _unitOfWork.SaveChangeAsync();

        return new SuccessResponseModel<string>
        {
            Success = true,
            Message = "Order status updated successfully.",
            Data = order.Status
        };
    }

}