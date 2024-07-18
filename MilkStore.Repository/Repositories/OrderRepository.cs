using Microsoft.EntityFrameworkCore;
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
        var sqlQuery = @"
        SELECT *
        FROM [Order]
        WHERE [AccountId] = {0}
       
    ";

        var points = await _context.Orders
            .FromSqlRaw(sqlQuery, accountId, pageIndex * pageSize, pageSize)
            .ToListAsync();

        return points;
    }
}