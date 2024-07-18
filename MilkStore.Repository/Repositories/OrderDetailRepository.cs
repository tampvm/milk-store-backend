using MilkStore.Domain.Entities;
using MilkStore.Repository.Data;
using MilkStore.Repository.Interfaces;

namespace MilkStore.Repository.Repositories;

public class OrderDetailRepository : GenericRepository<OrderDetail>, IOrderDetailRepository
{
    private readonly AppDbContext _context;
    public OrderDetailRepository(AppDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
    {
        _context = context;
    }

    public async Task<List<OrderDetail>> GetOrderItemByOrderIdAsync(string accountId, int pageIndex, int pageSize)
    {
        var points = _context.OrderDetails.Where(p => p.OrderId == accountId).Skip(pageIndex * pageSize).Take(pageSize).ToList();
        return points;
    }
}