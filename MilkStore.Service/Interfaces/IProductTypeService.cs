using MilkStore.Service.Models.ResponseModels;
using MilkStore.Service.Models.ViewModels.ProductTypeViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore.Service.Interfaces
{
    public interface IProductTypeService
    {
        Task<ResponseModel> GetAllProductTypeAsync();
        Task<ResponseModel> GetProductTypeByIdAsync(int productTypeId);
        Task<ResponseModel> CreateProductTypeAsync(CreateProductTypeDTO productType);
        Task<ResponseModel> UpdateProductTypeAsync(UpdateProductTypeDTO productType);
        Task<ResponseModel> DeleteProductTypeAsync(DeleteProductTypeDTO deleteProductType);
        Task<ResponseModel> RestoreProductTypeAsync(RestoreProductTypeDTO restoreProductType);
    }
}
