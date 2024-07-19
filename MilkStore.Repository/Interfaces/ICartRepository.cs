using MilkStore.Domain.Entities;

namespace MilkStore.Repository.Interfaces;

public interface ICartRepository : IGenericRepository<Cart>
{
    Task<List<Cart>> GetCartsByAccountIdAsync(string accountId, int pageIndex, int pageSize);
    Task<Cart> GetCartItemAsync(string accountId, string productId, string status);
}