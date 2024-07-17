using MilkStore.Service.Models.ResponseModels;
using MilkStore.Service.Models.ViewModels.InteractionModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore.Service.Interfaces
{
    public interface ILikeBlogService 
    {
        Task<ResponseModel> GetAllLikeByBlog(int pageIndex, int pageSize);
        Task<ResponseModel> GetLikeByBlogId(int pageIndex, int pageSize, int blogId);
        Task<ResponseModel> CreateOfUpdateLike(CreateLike model,string userId, int blogId);
        Task<ResponseModel> UpdateLike(UpdateLike model, string userId, int blogId);
        
    }
}
