using AutoMapper;
using Microsoft.AspNetCore.Identity;
using MilkStore.Domain.Entities;
using MilkStore.Repository.Common;
using MilkStore.Repository.Interfaces;
using MilkStore.Repository.Repositories;
using MilkStore.Service.Interfaces;
using MilkStore.Service.Models.ResponseModels;
using MilkStore.Service.Models.ViewModels.BogViewModel;
using MilkStore.Service.Models.ViewModels.RoleViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore.Service.Services
{
    public class BlogService : IBlogService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;


        public BlogService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;

        }
        public async Task<ResponseModel> GetAllBlog(int pageIndex, int pageSize)
        {
            var blogs = await _unitOfWork.BlogRepostiory.GetAsync(
              filter: r => r.Status.Equals(true),
              pageIndex: pageIndex,
              pageSize: pageSize
              );
            var blogDTO = _mapper.Map<Pagination<ViewBlogModel>>(blogs);
            return new SuccessResponseModel<object>
            {
                Success = true,
                Message = "Blog retrieved successfully.",
                Data = blogDTO
            };

        }
        public async Task<ResponseModel> GetBlogByUserId(int pageIndex, int pageSize, int id)
        {
            var blogs = await _unitOfWork.BlogRepostiory.GetByIdAsync(id);
             
             
            var blogDTO = _mapper.Map<Pagination<ViewBlogModel>>(blogs);
            return new SuccessResponseModel<object>
            {
                Success = true,
                Message = "Blog retrieved successfully.",
                Data = blogDTO
            };

        }


    }
}
