using MilkStore.Domain.Entities;

namespace MilkStore.Repository.Interfaces;

public interface IOrderDetailRepository : IGenericRepository<OrderDetail>
{
    Task<List<OrderDetail>> GetOrderItemByOrderIdAsync(string accountId, int pageIndex, int pageSize);
}