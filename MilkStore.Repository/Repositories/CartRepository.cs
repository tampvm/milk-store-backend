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
        var points = await _context.Carts
            .Where(p => p.AccountId == accountId && p.Status == CartStatusEnum.InCart.ToString())
            .Skip(pageIndex * pageSize)
            .Take(pageSize)
            .ToListAsync();
        return points;
    }
}