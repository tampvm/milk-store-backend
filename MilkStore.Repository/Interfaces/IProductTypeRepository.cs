using MilkStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore.Repository.Interfaces
{
    public interface IProductTypeRepository : IGenericRepository<ProductType>
    {
        Task<List<ProductType>> GetAllProductTypesAsync();
        Task UpdateProductTypeAsync(ProductType productType);
        Task<ProductType> GetProductTypeByIdAsync(int productTypeId);
        Task<ProductType> GetProductTypeByNameAsync(string name);
    }
}
