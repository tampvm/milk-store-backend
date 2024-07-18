using MilkStore.Domain.Entities;
using MilkStore.Repository.Data;
using MilkStore.Repository.Interfaces;

namespace MilkStore.Repository.Repositories;

public class OrderRepository : GenericRepository<Order>, IOrderRepository
{
    private readonly AppDbContext _context;
    public OrderRepository(AppDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
    {
        _context = context;
    }

    public async Task<List<Order>> GetOrderByAccountIdAsync(string accountId, int pageIndex, int pageSize)
    {
        var points = _context.Orders.Where(p => p.AccountId == accountId).Skip(pageIndex * pageSize).Take(pageSize).ToList();
        return points;
    }
}