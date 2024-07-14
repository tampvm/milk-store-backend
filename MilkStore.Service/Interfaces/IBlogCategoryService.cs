using MilkStore.Service.Models.ResponseModels;
using MilkStore.Service.Models.ViewModels.BlogCategoryViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore.Service.Interfaces
{
    public interface IBlogCategoryService 
    {
        Task<ResponseModel> GetAllBlogCategory(int pageIndex, int pageSize);
        Task<ResponseModel> GetBlogCategoryById(int id);
        Task<ResponseModel> CreateBlogCategory(CreateBlogCategoryDTO model);
        Task<ResponseModel> UpdateBlogCategory(UpdateBlogCategoryDTO model, int id);
        Task<ResponseModel> DeleteBlogCategory(int id);
    }
}
