using Microsoft.EntityFrameworkCore;
using MilkStore.Domain.Entities;
using MilkStore.Repository.Data;
using MilkStore.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore.Repository.Repositories
{
    public class AddressRepository : GenericRepository<Address>, IAddressRepository
    {
        private readonly AppDbContext _context;
        public AddressRepository(AppDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
        {
            _context = context;
        }

        public async Task<Address> GetDefaultAddressOrFirstAsync(string userId)
        {
            // Lấy địa chỉ mặc định (isDefault = true) nếu có
            var defaultAddress = await _context.Addresses.FirstOrDefaultAsync(a => a.AccountId == userId && a.IsDefault);

            // Nếu không có địa chỉ mặc định, lấy địa chỉ đầu tiên trong danh sách
            if (defaultAddress == null)
            {
                return await _context.Addresses.OrderBy(a => a.Id).FirstOrDefaultAsync(a => a.AccountId == userId);
            }

            return defaultAddress;
        }

        public async Task<Address?> GetDefaultAddressAsync(string userId)
        {
            return await _context.Addresses.FirstOrDefaultAsync(a => a.AccountId == userId && a.IsDefault);
        }

        public async Task<List<Address>> GetByUserIdAsync(string userId)
        {
            return await _context.Addresses.Where(a => a.AccountId == userId).ToListAsync();
        }

    }
}
