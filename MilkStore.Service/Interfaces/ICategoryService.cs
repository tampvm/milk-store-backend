using MilkStore.Service.Models.ResponseModels;
using MilkStore.Service.Models.ViewModels.CategoryViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore.Service.Interfaces
{
    public interface ICategoryService 
    {
        Task<ResponseModel> GetAllCategory(int pageIndex, int pageSize);
        Task<ResponseModel> GetCategoryById(int id);
        Task<ResponseModel> CreateCategory(CreateCategoryDTO model);
        Task<ResponseModel> UpdateCategory(UpdateCategoryDTO model, int id);
        Task<ResponseModel> DeleteCategory(int id);

    }
}
