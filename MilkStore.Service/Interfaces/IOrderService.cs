using MilkStore.Service.Models.ResponseModels;
using MilkStore.Service.Models.ViewModels.OrderViewDTO;

namespace MilkStore.Service.Interfaces;

public interface IOrderService
{
    Task<ResponseModel> AddProductToCartAsync(CreateOrderDTO model);
}