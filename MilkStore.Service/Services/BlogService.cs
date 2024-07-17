using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MilkStore.Domain.Entities;
using MilkStore.Domain.Enums;
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
        private readonly UserManager<Account> _userManager;

        string currentDate = DateTime.Now.ToString("yyyy/MM/dd");
        public BlogService(IUnitOfWork unitOfWork, IMapper mapper, UserManager<Account> userManager)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _mapper = mapper;

        }

        public async Task<ResponseModel> CreateBlog(CreateBlogDTO model)
        {
            var blog = new Post
            {
                Title = model.Title,
                Content = model.Content,
                Status = true,
                IsDeleted = false
            };

            var newAvatar = new Image
            {
                ImageUrl = model.Img,
                ThumbnailUrl = model.Img,
                Type = ImageTypeEnums.Avatar.ToString(),
                CreatedBy = blog.CreatedBy,
            };

            try
            {
                // Thêm ảnh mới vào cơ sở dữ liệu
                await _unitOfWork.ImageRepository.AddAsync(newAvatar);
                await _unitOfWork.SaveChangeAsync(); // Lưu thay đổi để tạo Id cho newAvatar

                // Thêm blog vào cơ sở dữ liệu
                await _unitOfWork.BlogRepostiory.AddAsync(blog);
                await _unitOfWork.SaveChangeAsync(); // Lưu thay đổi để tạo Id cho blog

                // Thêm hình ảnh vào blog
                await _unitOfWork.BlogImageRepository.AddAsync(new PostImage
                {
                    ImageId = newAvatar.Id,
                    PostId = blog.Id,
                    IsDeleted = false
                });
                await _unitOfWork.SaveChangeAsync(); // Lưu tất cả các thay đổi

                // Tạo DTO cho phản hồi
                var postDTO = new BlogResponeDTO
                {
                    Id = blog.Id,
                    Title = blog.Title,
                    Content = blog.Content,
                    Status = blog.Status,
                    PostImages = new List<CreateBlogImgDTO>
            {
                new CreateBlogImgDTO
                {
                    PostId = blog.Id,
                    ImageId = newAvatar.Id
                }
            }
                };

                return new SuccessResponseModel<object>
                {
                    Success = true,
                    Message = "Blog created successfully.",
                    Data = postDTO
                };
            }
            catch (Exception ex)
            {
                // Ghi lỗi nếu có
                Console.WriteLine(ex.Message);

                // Trả về phản hồi lỗi
                return new ErrorResponseModel<object>
                {
                    Success = false,
                    Message = "An error occurred while creating the blog.",
                };
            }
        }

        public async Task<ResponseModel> GetAllBlog(int pageIndex, int pageSize)
        {
            // Fetch blogs with pagination
            var blogs = await _unitOfWork.BlogRepostiory.GetAsync(
                filter: r => r.Status.Equals(true),
                pageIndex: pageIndex,
                pageSize: pageSize
            );

            // Map blogs to DTOs
            var blogDTO = _mapper.Map<Pagination<ViewBlogModel>>(blogs);

            // Fetch and assign images for each blog
            foreach (var blog in blogDTO.Items)
            {
                var blogImg = await _unitOfWork.BlogImageRepository.FindAsync(r => r.PostId == blog.Id);
                if (blogImg != null)
                {
                    var image = await _unitOfWork.ImageRepository.FindAsync(img => img.Id == blogImg.ImageId);
                    blog.BlogImg = image?.ImageUrl;
                }
            }

            // Return the response
            return new SuccessResponseModel<object>
            {
                Success = true,
                Message = "Blog retrieved successfully.",
                Data = blogDTO
            };
        }
        public async Task<ResponseModel> GetBlogByUserId(int pageIndex, int pageSize, string id, int postId)
        {
            // Fetch the blog by the postId
            var createrBlog = await _unitOfWork.BlogRepostiory.FindAsync(r => r.Id == postId && r.CreatedBy == id);
            var createrBlogImg = await _unitOfWork.BlogImageRepository.FindAsync(r => r.PostId == postId);
            if (createrBlog == null)
            {
                return new ErrorResponseModel<object>
                {
                    Success = false,
                    Message = "Blog not found or you do not have permission to view it."
                };
            }

            // Fetch the image related to the postId
            var blogImg = await _unitOfWork.ImageRepository.FindAsync(img => img.Id == createrBlogImg.ImageId);

            var blogDTO = _mapper.Map<ViewBlogModel>(createrBlog);

            // Ensure blogImg is not null before accessing ImageUrl
            blogDTO.BlogImg = blogImg?.ImageUrl;

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

        public async Task<ResponseModel> GetBlogByUserIdWithouImg(int id)
        {
            // Fetch the blog by the postId
            var blog = await _unitOfWork.BlogRepostiory.GetByIdAsync(id);
            var blogDTO = _mapper.Map<Pagination<ViewBlogModel>>(blog);
            //Ensure blogImg is not null before accessing ImageUrl

            return new SuccessResponseModel<object>
            {
                Success = true,
                Message = "Blog retrieved successfully.",
                Data = blogDTO
            };

        }

        public async Task<ResponseModel> CreateBlogImg(CreateBlogImgDTO model)
        {
            var blogImg =  _mapper.Map<PostImage>(model);
            blogImg.ImageId = model.ImageId;
            blogImg.IsDeleted = false;
            blogImg.PostId = model.PostId;
            try
            {
                // Add blog to the repository
                await _unitOfWork.BlogImageRepository.AddAsync(blogImg);

                // Commit the changes to the database
                await _unitOfWork.SaveChangeAsync();

                // Return the appropriate response
                return new SuccessResponseModel<object>
                {
                    Success = true,
                    Message = "Blog created successfully.",
                    Data = blogImg
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
        public async Task<ResponseModel> GetBlogByBlogId(int blogId)
        {
            // Fetch the blog by the blogId
            var blog = await _unitOfWork.BlogRepostiory.GetByIdAsync(blogId);
            var blogDTO = _mapper.Map<ViewBlogModel>(blog);
            //Ensure blogImg is not null before accessing ImageUrl  
            var blogImg = await _unitOfWork.BlogImageRepository.FindAsync(r => r.PostId == blog.Id);
            if (blogImg != null)
            {
                var image = await _unitOfWork.ImageRepository.FindAsync(img => img.Id == blogImg.ImageId);
                blogDTO.BlogImg = image?.ImageUrl;

            }
            return new SuccessResponseModel<object>
            {
                Success = true,
                Message = "Blog retrieved successfully.",
                Data = blogDTO
            };
        }
    }
}
