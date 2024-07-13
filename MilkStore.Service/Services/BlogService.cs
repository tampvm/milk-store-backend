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

        string currentDate = DateTime.Now.ToString("yyyy/MM/dd");
        public BlogService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;

        }

        public async Task<ResponseModel> CreateBlog(CreateBlogDTO model)
        {
            // Create blog
            // Create blog
            var blog = _mapper.Map<Post>(model);
            blog.Title = model.Title;
            blog.Content = model.Content;
            blog.IsDeleted = false;
            blog.CreatedAt = DateTime.Now;
            blog.Status = true;
            blog.CreatedBy = model.createBy;
            blog.UpdatedAt = DateTime.Now;
            blog.UpdatedBy = model.updateBy;
            blog.DeletedAt = DateTime.Now;
            blog.DeletedBy = model.deleteBy;

            try
            {
                // Add blog to the repository
                await _unitOfWork.BlogRepostiory.AddAsync(blog);

                // Commit the changes to the database
                await _unitOfWork.SaveChangeAsync();

                // Return the appropriate response
                return new SuccessResponseModel<object>
                {
                    Success = true,
                    Message = "Blog created successfully.",
                    Data = blog
                };
            }
            catch (Exception ex)
            {
                // Log the exception (consider using a logging framework)
                Console.WriteLine(ex.Message);

                // Return a failure response
                return new SuccessResponseModel<object>
                {
                    Success = false,
                    Message = "An error occurred while creating the blog.",
                    Data = null
                };
            }
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

        public async Task<ResponseModel> UpdateBlog(UpdateBlogDTO model, int id)
        {
            if (model.UpdateAt == default)
            {
                return new ResponseModel
                {
                    Success = false,
                    Message = "Update date is not valid."
                };
            }

            try
            {
                // Retrieve the blog by Id
                var blog = await _unitOfWork.BlogRepostiory.GetByIdAsync(id);

                if (blog == null)
                {
                    return new ResponseModel
                    {
                        Success = false,
                        Message = "Blog not found.",
                    };
                }

                // Check if the blog has been deleted
                if (blog.IsDeleted)
                {
                    return new ResponseModel
                    {
                        Success = false,
                        Message = "Cannot update a deleted blog.",
                    };
                }

                // Map the properties
                blog.Title = model.Title;
                blog.Content = model.Content;
                blog.Status = model.Status;
                blog.UpdatedAt = DateTime.Now; // This is redundant if Update sets it
                blog.UpdatedBy = model.UpdateBy; // This is redundant if Update sets it

                // Use the Update method
                _unitOfWork.BlogRepostiory.Update(blog);

                // Save changes
                await _unitOfWork.SaveChangeAsync();

                return new SuccessResponseModel<object>
                {
                    Success = true,
                    Message = "Blog updated successfully.",
                    Data = blog
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return new ResponseModel
                {
                    Success = false,
                    Message = "An error occurred while updating the blog."
                };
            }
        }
        //update status blog name delete with delete by who is entered in api
        public async Task<ResponseModel> DeleteBlog(int id, string deleteBy)
        {
            try
            {
                // Retrieve the blog by Id
                var blog = await _unitOfWork.BlogRepostiory.GetByIdAsync(id);

                if (blog == null)
                {
                    return new ResponseModel
                    {
                        Success = false,
                        Message = "Blog not found.",
                    };
                }

                // Check if the blog has been deleted
                if (blog.IsDeleted)
                {
                    return new ResponseModel
                    {
                        Success = false,
                        Message = "Cannot delete a deleted blog.",
                    };
                }

                // Map the properties
                blog.IsDeleted = true;
                blog.DeletedAt = DateTime.Now;
                blog.DeletedBy = deleteBy;

                // Use the Update method
                _unitOfWork.BlogRepostiory.SoftRemove(blog);

                // Save changes
                await _unitOfWork.SaveChangeAsync();

                return new SuccessResponseModel<object>
                {
                    Success = true,
                    Message = "Blog deleted successfully.",
                    Data = blog
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return new ResponseModel
                {
                    Success = false,
                    Message = "An error occurred while deleting the blog."
                };
            }
        }
    }
}
