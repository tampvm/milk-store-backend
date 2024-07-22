using Microsoft.EntityFrameworkCore;
using MilkStore.Domain.Entities;
using MilkStore.Repository.Common;
using MilkStore.Repository.Data;
using MilkStore.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore.Repository.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(AppDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            try
            {
                List<Product> products = new List<Product>();
                products = await _context.Products
                    .Include(x => x.AgeRange)
                    .Include(x => x.Type)
                    .Include(x => x.Brand)
                    .OrderByDescending(x => x.CreatedAt)
                    .ToListAsync();

                if (products == null)
                {
                    throw new Exception("Products not found");
                }

                return products;
            }
            catch
            {
                throw new Exception("Get all products failed");
            }
        }

        public async Task<Product> GetProductByIdAsync(string productId)
        {
            try 
            {
                Product product = new Product();
                product = await _context.Products
                    .Where(x => x.Id == productId)
                    .Include(x => x.AgeRange)
                    .Include(x => x.Type)
                    .Include(x => x.Brand)
                    .FirstAsync();
                if (product == null)
                {
                    throw new Exception("Product not found");
                }
                return product;
            }
            catch
            {
                throw new Exception("Get product by id failed");
            }
        }

        public async Task<List<Product>> GetProductsByBrandIdAsync(int brandId)
        {
            try
            {
                List<Product> products = new List<Product>();
                products = await _context.Products
                    .Where(x => x.BrandId == brandId)
                    .Include(x => x.AgeRange)
                    .Include(x => x.Type)
                    .Include(x => x.Brand)
                    .ToListAsync();
                if (products == null)
                {
                    throw new Exception("Products not found");
                }
                return products;
            }
            catch
            {
                throw new Exception("Get product by brand id failed");
            }
        }

        public async Task<Product> GetProductBySKUAsync(string sku)
        {
            try {
                Product product = new Product();
                product = _context.Products
                    .Where(x => x.Sku == sku)
                    .Include(x => x.AgeRange)
                    .Include(x => x.Type)
                    .Include(x => x.Brand)
                    .First();
                if (product == null)
                {
                    throw new Exception("Product not found");
                }
                return product;
            }
            catch
            {
                throw new Exception("Get product by sku failed");
            }
        }

        public async Task<Product> GetProductByAgeRangeIdAsync(int ageRangeId)
        {
            try
            {
                Product product = new Product();
                product = await _context.Products.Where(x => x.AgeId == ageRangeId).FirstOrDefaultAsync();
                return product;
            }
            catch
            {
                throw new Exception("Get product by age range id failed");
            }
        }

        public async Task<Product> GetProductByProductTypeIdAsync(int productTypeId)
        {
            try {
                Product product = new Product();
                product = await _context.Products.Where(x => x.TypeId == productTypeId).FirstOrDefaultAsync();
                return product;
            }
            catch {
                throw new Exception("Get product by product type id failed");
            }
        }

        public Task UpdateProductAsync(Product product)
        {
            try
            {
                _context.Entry(product).State = EntityState.Modified;
                return _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Update product failed:" + ex.Message);
            }
        }
    }
}
