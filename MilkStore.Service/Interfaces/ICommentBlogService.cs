using MilkStore.Service.Models.ResponseModels;
using MilkStore.Service.Models.ViewModels.InteractionModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore.Service.Interfaces
{
    public interface ICommentBlogService
    {
        Task<ResponseModel> CreateComment(CreateCommentByBlogId model, string userId, int blogId);
        Task<ResponseModel> GetCommentByBlogId(int pageIndex, int pageSize, int blogId);
        Task<ResponseModel> UpdateCommentByBlogID(CreateCommentByBlogId model, int commentId, int blogId);
        Task<ResponseModel> DeleteCommentByBlogID(SoftDeleteCommentByBlogId model, int commentId, int blogId);

    }
}
