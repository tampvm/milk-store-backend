using MilkStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore.Repository.Interfaces
{
    public interface IAgeRangeRepository : IGenericRepository<AgeRange>
    {
        Task<List<AgeRange>> GetAllAgeRangeAsync();
        Task<AgeRange> GetAgeRangeByIdAsync(int id);
        Task UpdateAgeRangeAsync(AgeRange ageRange);
        Task<AgeRange> GetAgeRangeByNameAsync(string name);
    }
}
