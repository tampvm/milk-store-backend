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
    public class ProductImageRepository : GenericRepository<ProductImage>, IProductImageRepository
    {
        public ProductImageRepository(AppDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
        {
            _context = context;
        }

        public async Task<List<ProductImage>> GetProductImageByProductIdAsync(string productId)
        {
            try
            {
                return await _context.ProductImages
                .Where(pi => pi.ProductId == productId && !pi.IsDeleted)
                .Include(pi => pi.Image) 
                .ToListAsync();
            }
            catch
            {
                throw new Exception("Get product images failed");
            }
        }

        public Task UpdateProductImageAsync(ProductImage productImage)
        {
            try
            {
                _context.Entry(productImage).State = EntityState.Modified;
                return _context.SaveChangesAsync();
            }
            catch
            {
                throw new Exception("Update product image failed");
            }
        }
    }
}
