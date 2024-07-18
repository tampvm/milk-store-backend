using Microsoft.EntityFrameworkCore;
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

    public async Task<List<OrderDetail>> GetOrderItemByOrderIdAsync(string orderId, int pageIndex, int pageSize)
    {
        var sqlQuery = @"
        SELECT *
        FROM [OrderDetail]
        WHERE [OrderId] = {0}
        
    ";

        var points = await _context.OrderDetails
            .FromSqlRaw(sqlQuery, orderId, pageIndex * pageSize, pageSize)
            .ToListAsync();

        return points;
    }

}