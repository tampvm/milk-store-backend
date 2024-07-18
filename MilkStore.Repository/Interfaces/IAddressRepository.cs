using MilkStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore.Repository.Interfaces
{
    public interface IAddressRepository : IGenericRepository<Address>
    {
        Task<List<Address>> GetByUserIdAsync(string userId);
        Task<Address> GetDefaultAddressOrFirstAsync(string userId);
        Task<Address?> GetDefaultAddressAsync(string userId);
    }
}
