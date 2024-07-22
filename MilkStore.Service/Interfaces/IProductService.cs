using Microsoft.AspNetCore.Http;
using MilkStore.Domain.Entities;
using MilkStore.Service.Models.ResponseModels;
using MilkStore.Service.Models.ViewModels.ProductViewModels;
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
        Task<ResponseModel> GetProductsPaginationAsync(string keySearch, int pageIndex, int pageSize);
        Task<ResponseModel> GetProductByIdAsync(String productId);
        Task<ResponseModel> CreateProductAsync(CreateProductDTO productCreateDTO);
        Task<ResponseModel> UpdateProductAsync(UpdateProductDTO productUpdateDTO);
        Task<ResponseModel> DeleteProductAsync(DeleteProductDTO productDeleteDTO);
        Task<ResponseModel> RestoreProductAsync(RestoreProductDTO productRestoreDTO);
        Task<ResponseModel> UpdateProductStatusAsync(ChangeStatusProductDTO model);
        Task<ResponseModel> GetProductBySkuAsync(string sku);
        Task<ResponseModel> GetProductsByBrandIdAsync(int brandId);
    }
}
