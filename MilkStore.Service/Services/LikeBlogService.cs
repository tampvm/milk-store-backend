using AutoMapper;
using Microsoft.AspNetCore.Identity;
using MilkStore.Domain.Entities;
using MilkStore.Repository.Common;
using MilkStore.Repository.Interfaces;
using MilkStore.Service.Interfaces;
using MilkStore.Service.Models.ResponseModels;
using MilkStore.Service.Models.ViewModels.InteractionModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore.Service.Services
{
    public class LikeBlogService : ILikeBlogService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<Account> _userManager;
        public LikeBlogService(IUnitOfWork unitOfWork, IMapper mapper, UserManager<Account> userManager)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _mapper = mapper;

        }

        public async Task<ResponseModel> CreateOfUpdateLike(CreateLike model, string userId, int blogId)
        {
            // Tìm bản ghi dựa trên AccountId và PostId
            var existingLike = await _unitOfWork.LikeRepository.FindAsync(
                l => l.AccountId == userId && l.PostId == blogId
            );

            if (existingLike != null)
            {
                // Nếu bản ghi đã tồn tại, cập nhật IsLike
                existingLike.IsLike = false; // Hoặc giá trị nào đó tùy theo yêu cầu
                existingLike.UnLikeAt = DateTime.UtcNow; // Cập nhật thời gian un-like
                _unitOfWork.LikeRepository.Update(existingLike);
            }
            else
            {
                // Nếu không tìm thấy bản ghi, tạo bản ghi mới
                var newLike = new LikePost
                {
                    AccountId = userId,
                    PostId = blogId,
                    IsLike = true, // Hoặc giá trị nào đó tùy theo yêu cầu
                    LikeAt = DateTime.UtcNow
                };
                await _unitOfWork.LikeRepository.AddAsync(newLike);
            }

            // Lưu các thay đổi vào cơ sở dữ liệu
            await _unitOfWork.SaveChangeAsync();

            return new SuccessResponseModel<object>
            {
                Success = true,
                Message = "Like status updated successfully."
            };
        }
    
    

        public Task<ResponseModel> GetAllLikeByBlog(int pageIndex, int pageSize)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseModel> GetLikeByBlogId(int pageIndex, int pageSize, int blogId)
        {
            //get all like by blog id
            var likes = await _unitOfWork.LikeRepository.GetAsync(x => x.PostId.Equals(blogId));
            if (likes == null)
            {
                return new ErrorResponseModel<object>
                {
                    Success = false,
                    Message = "Blog not found like"
                };

            }
            var likeViewModel =  _mapper.Map< Pagination<GetBlogLike>>(likes);
            return new SuccessResponseModel<object>
            {
                Success = true,
                Message = "Blog found like",
                Data = likeViewModel
            };
        }

        public Task<ResponseModel> UpdateLike(UpdateLike model, string userId, int blogId)
        {
            throw new NotImplementedException();
        }
    }
}
