using MilkStore.Service.Models.ResponseModels;
using MilkStore.Service.Models.ViewModels.OrderViewDTO;

namespace MilkStore.Service.Interfaces;

public interface IOrderService
{
    Task<ResponseModel> AddProductToCartAsync(CreateOrderDTO model);
    Task<ResponseModel> GetOrderDetail(int orderId, int pageIndex, int pageSize);
    Task<ResponseModel> GetAllOrder(int pageIndex, int pageSize);
    Task<ResponseModel> CheckPaymentStatus(string orderId);

    Task<ResponseModel> GetAllOrderAsync(string? orderId, string? status, int pageIndex, int pageSize);
    Task<ResponseModel> GetOrderByIdAsync(string orderId);
    Task<ResponseModel> GetOrderByUserIdAsync(string userId, int pageIndex, int pageSize);
    Task<ResponseModel> ChangeStatusAsync(string orderId, string status);
}