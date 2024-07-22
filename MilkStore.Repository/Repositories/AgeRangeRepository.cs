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

        public async Task<AgeRange> GetAgeRangeByIdAsync(int id)
        {
            try
            {
                AgeRange ageRange = new AgeRange();
                ageRange = await _context.AgeRanges.Where(x => x.Id == id).FirstOrDefaultAsync();
                if (ageRange == null)
                {
                    throw new Exception("AgeRange not found");
                }
                return ageRange;
            }
            catch
            {
                throw new Exception("Get ageRange failed");
            }
        }

        public async Task<AgeRange> GetAgeRangeByNameAsync(string name)
        {
            try
            {
                AgeRange ageRange = new AgeRange();
                ageRange = await _context.AgeRanges.Where(x => x.Name == name).FirstOrDefaultAsync();
                return ageRange;
            }
            catch
            {
                throw new Exception("Get ageRange failed");
            }
        }

        public Task UpdateAgeRangeAsync(AgeRange ageRange)
        {
            try
            {
                _context.Entry(ageRange).State = EntityState.Modified;
                return _context.SaveChangesAsync();
            }
            catch
            {
                throw new Exception("Update ageRange failed");
            }
        }
    }
}
