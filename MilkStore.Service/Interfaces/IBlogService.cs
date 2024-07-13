using MilkStore.Service.Models.ResponseModels;
using MilkStore.Service.Models.ViewModels.BogViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore.Service.Interfaces
{
    public interface IBlogService
    {
        Task<ResponseModel> GetAllBlog(int pageIndex, int pageSize);
        Task<ResponseModel> GetBlogByUserId(int pageIndex, int pageSize, int id);
        Task<ResponseModel> CreateBlog(CreateBlogDTO model);
        Task<ResponseModel> UpdateBlog(UpdateBlogDTO model, int id);
        Task<ResponseModel> DeleteBlog(int id, string deleteBy);
    }
}
