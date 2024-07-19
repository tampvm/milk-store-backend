using MilkStore.Domain.Entities;
using MilkStore.Service.Models.ResponseModels;
using MilkStore.Service.Models.ViewModels.CartViewModel;

namespace MilkStore.Service.Interfaces;

public interface ICartService
{
    Task<ResponseModel> AddProductToCartAsync(CartDTO model);
    Task<ResponseModel> GetCartByAccountID(int pageIndex, int pageSize);
    Task<ResponseModel> UpdateCartByID(int Id, CartDTO model, int khonglatang1lagiam);
}