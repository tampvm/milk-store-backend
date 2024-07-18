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
    public class AgeRangeRepository : GenericRepository<AgeRange>, IAgeRangeRepository
    {
        public AgeRangeRepository(AppDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
        {
            _context = context;
        }

        public async Task<List<AgeRange>> GetAllAgeRangeAsync()
        {
            try
            {
                List<AgeRange> ageRanges = new List<AgeRange>();
                ageRanges = await _context.AgeRanges.ToListAsync();
                if (ageRanges == null)
                {
                    throw new Exception("AgeRanges not found");
                }
                return ageRanges;
            }
            catch
            {
                throw new Exception("Get all ageRanges failed");
            }
        }
    }
}
