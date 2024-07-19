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
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Twilio.TwiML.Messaging;

namespace MilkStore.Service.Services
{
    public class CommentBlogService : ICommentBlogService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<Account> _userManager;
        public CommentBlogService(IUnitOfWork unitOfWork, IMapper mapper, UserManager<Account> userManager)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _mapper = mapper;

        }
       

        public async Task<ResponseModel> CreateComment(CreateCommentByBlogId model, string userId, int blogId)
        {
            //Create comment
            var newComment = new CommentPost
            {
                AccountId = userId,
                PostId = blogId,
                CommentText = model.CommentText,
                Active = true

            };
            await _unitOfWork.CommentBlogRepository.AddAsync(newComment);
            await _unitOfWork.SaveChangeAsync();
            return new SuccessResponseModel<object>
            {
                Success = true,
                Message = "Create comment success",
                Data = newComment
            };
        }

        public async Task<ResponseModel> DeleteCommentByBlogID(SoftDeleteCommentByBlogId model, int commentId, int blogId)
        {
           //delete comment
            var existingComment = await _unitOfWork.CommentBlogRepository.FindAsync(c => c.CommentId == commentId && c.PostId == blogId);
            if (existingComment == null)
            {
                return new ErrorResponseModel<object>
                {

                    Success = false,
                    Message = "Comment not found"

                };
            }
            _unitOfWork.CommentBlogRepository.SoftRemove(existingComment);
            await _unitOfWork.SaveChangeAsync();
            return new SuccessResponseModel<object>
            {
                Success = true,
                Message = "Update comment success"
            };

        }

        public async Task<ResponseModel> GetCommentByBlogId(int pageIndex, int pageSize, int blogId)
        {
            // Tìm bản ghi dựa trên PostId
            var existingComment = await _unitOfWork.CommentBlogRepository.GetAsync(
                filter: r => r.PostId.Equals(blogId),
                pageIndex: pageIndex,
                pageSize: pageSize
            ); 
            if (existingComment == null) {
                return new ErrorResponseModel<object>
                {

                    Success = false,
                    Message = "Blog not found comment"

                };
            }
           var comment = _mapper.Map<Pagination<GetBlogComments>>(existingComment);
            return new SuccessResponseModel<object>
            {
                Data = comment,
                Success = true,
                Message = "Get comment success"
            };
        }

        public async Task<ResponseModel> UpdateCommentByBlogID(CreateCommentByBlogId model, int commentId, int blogId)
        {
           //Update comment
            var existingComment = await  _unitOfWork.CommentBlogRepository.FindAsync(c => c.CommentId == commentId && c.PostId == blogId);
            if (existingComment == null)
            {
                return new ErrorResponseModel<object>
                {

                    Success = false,
                    Message = "Comment not found"

                };
            }
            
            existingComment.CommentText = model.CommentText;
            _unitOfWork.CommentBlogRepository.Update(existingComment);
            await _unitOfWork.SaveChangeAsync();
            return new SuccessResponseModel<object>
            {
                Success = true,
                Message = "Update comment success"
            };
        }
    }
}
