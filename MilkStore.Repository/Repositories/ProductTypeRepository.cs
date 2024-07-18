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
    public class ProductTypeRepository : GenericRepository<ProductType>, IProductTypeRepository
    {
        public ProductTypeRepository(AppDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
        {
            _context = context;
        }

        public async Task<List<ProductType>> GetAllProductTypesAsync()
        {
            try
            {
                List<ProductType> productTypes = new List<ProductType>();
                productTypes = await _context.Types.ToListAsync();
                if (productTypes == null)
                {
                    throw new Exception("ProductTypes not found");
                }
                return productTypes;
            }
            catch
            {
                throw new Exception("Get all productTypes failed");
            }
        }
    }
}
