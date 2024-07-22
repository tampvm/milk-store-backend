using MilkStore.Domain.Entities;
using MilkStore.Repository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore.Repository.Interfaces
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<Product> GetProductByIdAsync(String productId);
        Task UpdateProductAsync(Product product);
        Task<Product> GetProductBySKUAsync(string sku);
        Task<Product> GetProductByAgeRangeIdAsync(int ageRangeId);
        Task<Product> GetProductByProductTypeIdAsync(int productTypeId);
        Task<List<Product>> GetProductsByBrandIdAsync(int brandId);
    }
}
