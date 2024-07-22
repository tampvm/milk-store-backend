using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using MilkStore.Domain.Entities;
using MilkStore.Repository.Interfaces;
using MilkStore.Service.Common;
using MilkStore.Service.Interfaces;
using MilkStore.Service.Models.ResponseModels;
using MilkStore.Service.Models.ViewModels.ProductTypeViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore.Service.Services
{
    public class ProductTypeService : BaseService, IProductTypeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ProductTypeService(IUnitOfWork unitOfWork, IMapper mapper, ICurrentTime currentTime, IClaimsService claimsService, AppConfiguration appConfiguration, ISmsSender smsSender, IEmailSender emailSender, IMemoryCache cache) : base(unitOfWork, mapper, currentTime, claimsService, appConfiguration, smsSender, emailSender, cache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ResponseModel> GetAllProductTypeAsync()
        {
            try
            {
                var productTypes = await _unitOfWork.ProductTypeRepository.GetAllProductTypesAsync();
                var productTypeDTOs = _mapper.Map<List<ViewListProductTypeDTO>>(productTypes);
                return new SuccessResponseModel<object>()
                {
                    Success = true,
                    Message = "Product types retrieved successfully.",
                    Data = productTypeDTOs
                };
            }
            catch (Exception ex)
            {
                return new ErrorResponseModel<object>()
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ResponseModel> GetProductTypeByIdAsync(int productTypeId)
        {
            try
            {
                var productType = await _unitOfWork.ProductTypeRepository.GetProductTypeByIdAsync(productTypeId);
                var productTypeDTO = _mapper.Map<ViewListProductTypeDTO>(productType);
                return new SuccessResponseModel<object>()
                {
                    Success = true,
                    Message = "Product type retrieved successfully.",
                    Data = productTypeDTO
                };
            }
            catch (Exception ex)
            {
                return new ErrorResponseModel<object>()
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ResponseModel> CreateProductTypeAsync(CreateProductTypeDTO productType)
        {
            try
            {
                var productTypeExist = await _unitOfWork.ProductTypeRepository.GetProductTypeByNameAsync(productType.Name);
                if (productTypeExist != null)
                    return new ErrorResponseModel<object>()
                    {
                        Success = false,
                        Message = "Product type already exists."
                    };

                var productTypeEntity = _mapper.Map<ProductType>(productType);
                productTypeEntity.IsDeleted = false;
                productTypeEntity.CreatedAt = DateTime.UtcNow;
                await _unitOfWork.ProductTypeRepository.AddAsync(productTypeEntity);
                await _unitOfWork.SaveChangeAsync();
                return new SuccessResponseModel<object>()
                {
                    Success = true,
                    Message = "Product type created successfully.",
                    Data = productType
                };
            }
            catch (Exception ex)
            {
                return new ErrorResponseModel<object>()
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ResponseModel> UpdateProductTypeAsync(UpdateProductTypeDTO productType)
        {
            try
            {
                var productTypeExist = await _unitOfWork.ProductTypeRepository.GetProductTypeByNameAsync(productType.Name);
                if (productTypeExist != null && productTypeExist.Id != productType.Id)
                    return new ErrorResponseModel<object>()
                    {
                        Success = false,
                        Message = "Product type already exists."
                    };

                var existingProductType = await _unitOfWork.ProductTypeRepository.GetByIdAsync(productType.Id);
                if (existingProductType == null)
                {
                    return new ErrorResponseModel<object>()
                    {
                        Success = false,
                        Message = "Product type not found."
                    };
                }

                if (existingProductType.Active == false && existingProductType.IsDeleted == true && productType.Active == true)
                {
                    return new ErrorResponseModel<object>()
                    {
                        Success = false,
                        Message = "Product type is not active."
                    };
                }

                existingProductType.Name = productType.Name;
                existingProductType.Description = productType.Description;
                existingProductType.Active = productType.Active;
                existingProductType.UpdatedBy = productType.UpdatedBy;
                existingProductType.UpdatedAt = DateTime.UtcNow;

                await _unitOfWork.ProductTypeRepository.UpdateProductTypeAsync(existingProductType);
                await _unitOfWork.SaveChangeAsync();

                return new SuccessResponseModel<object>()
                {
                    Success = true,
                    Message = "Product type updated successfully.",
                    Data = productType
                };
            }
            catch (Exception ex)
            {
                return new ErrorResponseModel<object>()
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }


        public async Task<ResponseModel> DeleteProductTypeAsync(DeleteProductTypeDTO deleteProductType)
        {
            try
            {
                var productTypeExist = await _unitOfWork.ProductRepository.GetProductByProductTypeIdAsync(deleteProductType.Id);
                if (productTypeExist != null) return new ErrorResponseModel<object> { Success = false, Message = "Product type is in use." };

                var productType = await _unitOfWork.ProductTypeRepository.GetProductTypeByIdAsync(deleteProductType.Id);
                if (productType == null) return new ErrorResponseModel<object> { Success = false, Message = "Not found product type." };
                productType.IsDeleted = true;
                productType.Active = false;
                productType.DeletedAt = DateTime.UtcNow;
                await _unitOfWork.ProductTypeRepository.UpdateProductTypeAsync(productType);
                await _unitOfWork.SaveChangeAsync();
                return new SuccessResponseModel<object> { Success = true, Message = "Product type deleted successfully." };
            }
            catch (Exception ex)
            {
                return new ErrorResponseModel<object> { Success = false, Message = ex.Message };
            }
        }

        public async Task<ResponseModel> RestoreProductTypeAsync(RestoreProductTypeDTO restoreProductType)
        {
            try
            {
                var productType = await _unitOfWork.ProductTypeRepository.GetProductTypeByIdAsync(restoreProductType.Id);
                if (productType == null) return new ErrorResponseModel<object> { Success = false, Message = "Not found product type." };
                productType.IsDeleted = false;
                productType.Active = false;
                productType.UpdatedAt = DateTime.UtcNow;
                await _unitOfWork.ProductTypeRepository.UpdateProductTypeAsync(productType);
                await _unitOfWork.SaveChangeAsync();
                return new SuccessResponseModel<object> { Success = true, Message = "Product type restored successfully." };
            }
            catch (Exception ex) {
                return new ErrorResponseModel<object> { Success = false, Message = ex.Message };
            }
        }
    }
}
