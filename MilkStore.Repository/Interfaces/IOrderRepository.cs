using MilkStore.Domain.Entities;

namespace MilkStore.Repository.Interfaces;

public interface IOrderRepository : IGenericRepository<Order>
{
    Task<List<Order>> GetOrderByAccountIdAsync(string accountId, int pageIndex, int pageSize);
}