using Microsoft.EntityFrameworkCore;
using MilkStore.Domain.Entities;
using MilkStore.Domain.Enums;
using MilkStore.Repository.Data;
using MilkStore.Repository.Interfaces;

namespace MilkStore.Repository.Repositories;

public class CartRepository : GenericRepository<Cart>, ICartRepository
{
    private readonly AppDbContext _context;
    public CartRepository(AppDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
    {
        _context = context;
    }

    public async Task<List<Cart>> GetCartsByAccountIdAsync(string accountId, int pageIndex, int pageSize)
    {
        var sqlQuery = @"
        SELECT * 
        FROM [Cart]
        WHERE [AccountId] = {0} AND [Status] = {1}
        ";

        var points = await _context.Carts
            .FromSqlRaw(sqlQuery, accountId, "InCart", pageIndex * pageSize, pageSize)
            .ToListAsync();

        return points;
    }
    public async Task<Cart> GetCartItemAsync(string accountId, string productId, string status)
    {
        return await _context.Carts
            .SingleOrDefaultAsync(c => c.AccountId == accountId && c.ProductId == productId && c.Status == status);
    }
}