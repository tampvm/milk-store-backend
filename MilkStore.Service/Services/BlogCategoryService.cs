using AutoMapper;
using MilkStore.Domain.Entities;
using MilkStore.Repository.Interfaces;
using MilkStore.Service.Interfaces;
using MilkStore.Service.Models.ResponseModels;
using MilkStore.Service.Models.ViewModels.BlogCategoryViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore.Service.Services
{
    public class BlogCategoryService : IBlogCategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

      
        public BlogCategoryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;

        }

        public async Task<ResponseModel> CreateBlogCategory(CreateBlogCategoryDTO model)
        {
            //check if that postid already have this category id
            var result = await _unitOfWork.BlogCategoryRepository.FindAsync(x => x.PostId == model.PostId && x.CategoryId == model.CategoryId);
            if (result != null)
            {
                return new ErrorResponseModel<BlogCategory>
                {
                    Success = false,
                    Message = "This category already exist for this post."
                };
            }
            else
            {
               
                    var blogCategory = _mapper.Map<PostCategory>(model);
                    blogCategory.PostId = model.PostId;
                    blogCategory.CategoryId = model.CategoryId;
                    blogCategory.IsDeleted = false;
                try
                {
                    await _unitOfWork.BlogCategoryRepository.AddAsync(blogCategory);
                    await _unitOfWork.SaveChangeAsync();
                    return new SuccessResponseModel<object>
                    {
                        Success = true,
                        Message = "Blog category created successfully.",
                        Data = blogCategory
                    };
                }
                catch (Exception ex)
                {
                    // Log the exception (consider using a logging framework)
                    Console.WriteLine(ex.Message);

                    // Return a failure response
                    return new ErrorResponseModel<object>
                    {
                        Success = false,
                        Message = "An error occurred while creating the blog.",

                    };
                }
            }        
          // Create blog category       
        }

        public async Task<ResponseModel> DeleteBlogCategory(int id)
        {
            //delete blog category
            var blogCategory = await _unitOfWork.BlogCategoryRepository.GetByIdAsync(id);
            if (blogCategory == null)
                {
                return new ErrorResponseModel<BlogCategory>
                {
                    Success = false,
                    Message = "Blog category not found."
                };
            }
                try
                {
                     _unitOfWork.BlogCategoryRepository.SoftRemove(blogCategory);
                    await _unitOfWork.SaveChangeAsync();
                    return new SuccessResponseModel<object>
                    {
                        Success = true,
                        Message = "Blog category deleted successfully.",
                        Data = blogCategory
                    };
                }
                catch (Exception ex)
                {
                    // Log the exception (consider using a logging framework)
                    Console.WriteLine(ex.Message);

                    // Return a failure response
                    return new ErrorResponseModel<object>
                    {
                        Success = false,
                        Message = "An error occurred while deleting the blog category.",

                    };
                }
            

        }

        public Task<ResponseModel> GetAllBlogCategory(int pageIndex, int pageSize)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseModel> GetBlogCategoryById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseModel> UpdateBlogCategory(UpdateBlogCategoryDTO model, int id)
        {
            //check if that postid already have this category id
            var result = await _unitOfWork.BlogCategoryRepository.FindAsync(x => x.PostId == id && x.CategoryId == model.CategoryId);
            if (result != null)
            {
                return new ErrorResponseModel<BlogCategory>
                {
                    Success = false,
                    Message = "This category already exist for this post."
                };
            }
            else
            {
                var blogCategoryToUpdate = await _unitOfWork.BlogCategoryRepository.FindAsync(x => x.PostId == id);
                if (blogCategoryToUpdate == null)
                {
                    return new ErrorResponseModel<BlogCategory>
                    {
                        Success = false,
                        Message = "Blog not found."
                    };
                }
                blogCategoryToUpdate.CategoryId = model.CategoryId;
                try
                {
                     _unitOfWork.BlogCategoryRepository.Update(blogCategoryToUpdate);
                     await _unitOfWork.SaveChangeAsync();
                     return new SuccessResponseModel<object>
                        {
                        Success = true,
                        Message = "Blog category updated successfully.",
                        Data = blogCategoryToUpdate
                     };
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);

                    // Return a failure response
                    return new ErrorResponseModel<object>
                    {
                       
                        Success = false,
                        Message = "An error occurred while creating the blog.",

                    };
                }

            }
            

    }
    }
}
