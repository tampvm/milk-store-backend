using MilkStore.Domain.Entities;
using MilkStore.Service.Models.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore.Service.Interfaces
{
    public interface IProductService
    {
        Task<ResponseModel> GetAllProductsAsync();
        //Task<ResponseModel> GetProductsPaginationAsync (int pageIndex, int pageSize);
        Task<ResponseModel> GetProductByIdAsync(String productId);
    }
}
