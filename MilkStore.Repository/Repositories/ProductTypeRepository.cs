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

        public async Task<ProductType> GetProductTypeByIdAsync(int productTypeId)
        {
            try
            {
                ProductType productType = new ProductType();
                productType = await _context.Types
                    .Where(x => x.Id == productTypeId)
                    .FirstAsync();
                if (productType == null)
                {
                    throw new Exception("ProductType not found");
                }
                return productType;
            }
            catch
            {
                throw new Exception("Get productType by id failed");
            }
        }

        public async Task<ProductType> GetProductTypeByNameAsync(string name)
        {
            try
            {
                ProductType productType = new ProductType();
                productType = await _context.Types.Where(x => x.Name == name).FirstOrDefaultAsync();
                return productType;
            }
            catch
            {
                throw new Exception("Get productType by name failed");
            }
        }

        public async Task UpdateProductTypeAsync(ProductType productType)
        {
            try
            {
                _context.Entry(productType).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Update productType failed: " + ex.Message);
            }
        }
    }
}
