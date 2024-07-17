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
                products = await _context.Products.ToListAsync();

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
                product = await _context.Products.FirstOrDefaultAsync(x => x.Id == productId);
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

        //public async Task<Pagination<Product>> GetProductsPaginationAsync(int pageIndex, int pageSize)
        //{
        //    try
        //    {
        //        var products = await _context.Products.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
        //        var totalItemsCount = await _context.Products.CountAsync();
        //        return new Pagination<Product>(products, totalItemsCount, pageIndex, pageSize);
        //    }
        //    catch
        //    {
        //        throw new Exception("Get products pagination failed");
        //    }
        //}
    }
}
