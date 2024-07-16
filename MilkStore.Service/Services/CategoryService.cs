using AutoMapper;
using MilkStore.Domain.Entities;
using MilkStore.Repository.Common;
using MilkStore.Repository.Interfaces;
using MilkStore.Service.Interfaces;
using MilkStore.Service.Models.ResponseModels;
using MilkStore.Service.Models.ViewModels.CategoryViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore.Service.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;

        }
        public async Task<ResponseModel> CreateCategory(CreateCategoryDTO model)
        {
            //create a new category
            var category = _mapper.Map<Category>(model);
            category.Name = model.Name;
            category.Description = model.Description;
            category.Active = true;
            category.IsDeleted = false;

            try
            {
                await _unitOfWork.CategoryRepository.AddAsync(category);
                await _unitOfWork.SaveChangeAsync();
                return new SuccessResponseModel<object>
                {
                    Success = true,
                    Message = "Category create successfully.",
                    Data = category
                };
            }
            catch (Exception ex)
            {
                return new ErrorResponseModel<object>
                {
                    Success = false,
                    Message = ex.Message
                };
            }


        }

        public async Task<ResponseModel> DeleteCategory(int id)
        {
            //delete a category
            try
            {
                var category = await _unitOfWork.CategoryRepository.GetByIdAsync(id);
                if (category == null)
                {
                    return new ErrorResponseModel<object>
                    {
                        Success = false,
                        Message = "Category not found."
                    };
                }
                //check isDeleted
                if (category.IsDeleted == true)
                {
                    return new ErrorResponseModel<object>
                    {
                        Success = false,
                        Message = "Can't delete category is deleted."
                    };
                }

                _unitOfWork.CategoryRepository.SoftRemove(category);
                await _unitOfWork.SaveChangeAsync();
                return new SuccessResponseModel<object>
                {
                    Success = true,
                    Message = "Category deleted successfully.",
                    Data = category
                };
            }
            catch (Exception ex)
            {
                return new ErrorResponseModel<object>
                {
                    Success = false,
                    Message = ex.Message
                };
            }

            
        }

        public async Task<ResponseModel> GetAllCategory(int pageIndex, int pageSize)
        {
            //get all category
            var categories = await _unitOfWork.CategoryRepository.GetAsync(filter: r => r.Active.Equals(true),
              pageIndex: pageIndex,
              pageSize: pageSize);
            //map to view model
            var result = _mapper.Map<Pagination<ViewListCategoryDTO>>(categories);
          
            return new SuccessResponseModel<object>
            {
                Success = true,
                Message = "Blog retrieved successfully.",
                Data = result
            };

        }

        public async Task<ResponseModel> GetCategoryById(int id)
        {
            //get a category by id
            var category = await _unitOfWork.CategoryRepository.GetByIdAsync(id);

            // If the category is not found, handle the case accordingly
            if (category == null)
            {
                return new ErrorResponseModel<object>
                {
                    Success = false,
                    Message = "Category not found."
                };
            }

            // Map the category to the view model
            var result = _mapper.Map<ViewListCategoryDTO>(category);

            return new SuccessResponseModel<object>
            {
                Success = true,
                Message = "Category retrieved successfully.",
                Data = result
            };

        }

        public async Task<ResponseModel> UpdateCategory(UpdateCategoryDTO model, int id)
        {
            try
            {
                //update a category
                var category = await _unitOfWork.CategoryRepository.GetByIdAsync(id);
                if (category == null)
                {
                    return new ErrorResponseModel<object>
                    {
                        Success = false,
                        Message = "Category not found."
                    };
                }
                //check isDeleted
                if (category.IsDeleted == true)
                {
                    return new ErrorResponseModel<object>
                    {
                        Success = false,
                        Message = "Can't modify category is deleted."
                    };
                }


                category.Name = model.Name;
                category.Description = model.Description;
                category.Active = model.Active;
              
                try
                {
                    _unitOfWork.CategoryRepository.Update(category);
                    await _unitOfWork.SaveChangeAsync();
                    return new SuccessResponseModel<object>
                    {
                        Success = true,
                        Message = "Category updated successfully.",
                        Data = category
                    };
                }
                catch (Exception ex)
                {
                    return new ErrorResponseModel<object>
                    {
                        Success = false,
                        Message = ex.Message
                    };
                }
            }
            catch (Exception ex)
            {
                return new ErrorResponseModel<object>
                {
                    Success = false,
                    Message = ex.Message
                };


            }
        }
    }
}
