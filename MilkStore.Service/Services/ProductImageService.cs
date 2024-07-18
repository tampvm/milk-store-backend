using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using MilkStore.Domain.Entities;
using MilkStore.Repository.Interfaces;
using MilkStore.Service.Common;
using MilkStore.Service.Interfaces;
using MilkStore.Service.Models.ResponseModels;
using MilkStore.Service.Models.ViewModels.ProductImageViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Image = MilkStore.Domain.Entities.Image;

namespace MilkStore.Service.Services
{
    public class ProductImageService : BaseService, IProductImageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IFirebaseService _firebaseService;

        public ProductImageService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ICurrentTime currentTime,
            IClaimsService claimsService,
            AppConfiguration appConfiguration,
            ISmsSender smsSender,
            IEmailSender emailSender,
            IMemoryCache cache,
            IFirebaseService firebaseService)
            : base(unitOfWork, mapper, currentTime, claimsService, appConfiguration, smsSender, emailSender, cache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _firebaseService = firebaseService;
        }

        #region GetProductImages
        public async Task<ResponseModel> GetProductImagesAsync(string productId)
        {
            try
            {
                var productImages = await _unitOfWork.ProductImageRepository.GetProductImageByProductIdAsync(productId);
                var productImagesDTO = _mapper.Map<List<ViewListProductImageDTO>>(productImages);
                return new SuccessResponseModel<object>
                {
                    Success = true,
                    Message = "Product images retrieved successfully.",
                    Data = productImagesDTO
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
        #endregion

        #region CreateProductImage
        public async Task<ResponseModel> CreateProductImageAsync(CreateProductImageDTO createProductImageDTO, List<IFormFile> imageFiles, IFormFile thumbnailFile)
        {
            try
            {
                if (createProductImageDTO == null)
                {
                    return new ErrorResponseModel<object> { Success = false, Message = "Empty product image data." };
                }

                if (imageFiles == null)
                {
                    return new ErrorResponseModel<object> { Success = false, Message = "Image file is required." };
                }

                if (thumbnailFile == null)
                {
                    return new ErrorResponseModel<object> { Success = false, Message = "Thumbnail file is required." };
                }

                var thumbnailUrl = await UploadProductImageAsync(thumbnailFile);
                if (string.IsNullOrEmpty(thumbnailUrl))
                {
                    return new ErrorResponseModel<object> { Success = false, Message = "Thumbnail upload failed." };
                }

                foreach (var imageFile in imageFiles)
                {
                    var imageUrl = await UploadProductImageAsync(imageFile);
                    if (string.IsNullOrEmpty(imageUrl))
                    {
                        return new ErrorResponseModel<object> { Success = false, Message = "Image upload failed." };
                    }

                    var image = new Image
                    {
                        ImageUrl = imageUrl,
                        ThumbnailUrl = thumbnailUrl,
                        CreatedAt = DateTime.UtcNow,
                        CreatedBy = createProductImageDTO.CreatedBy,
                        Type = "Product",
                        IsDeleted = false
                    };

                    await _unitOfWork.ImageRepository.AddAsync(image);
                    await _unitOfWork.SaveChangeAsync();

                    var productImage = new ProductImage
                    {
                        ImageId = image.Id,
                        ProductId = createProductImageDTO.ProductId,
                        CreatedBy = createProductImageDTO.CreatedBy,
                        CreatedAt = DateTime.UtcNow,
                        IsDeleted = false
                    };

                    await _unitOfWork.ProductImageRepository.AddAsync(productImage);
                    await _unitOfWork.SaveChangeAsync();
                }
                return new SuccessResponseModel<object>
                {
                    Success = true,
                    Message = "Product image created successfully."
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
        #endregion

        #region UpdateProductImage
        public async Task<ResponseModel> UpdateProductImageAsync(UpdateProductImageDTO updateProductImageDTO, IFormFile imageFile, IFormFile thumbnailFile)
        {
            try
            {
                var image = await _unitOfWork.ImageRepository.GetByIdAsync(updateProductImageDTO.ImageId);
                if (image == null)
                {
                    return new ErrorResponseModel<object> { Success = false, Message = "Image not found." };
                }

                var productImages = await _unitOfWork.ProductImageRepository.GetProductImageByProductIdAsync(updateProductImageDTO.ProductId);
                var productImage = productImages.FirstOrDefault(pi => pi.ImageId == updateProductImageDTO.ImageId);
                if (productImage == null)
                {
                    return new ErrorResponseModel<object> { Success = false, Message = "Product image not found." };
                }

                string imageUrl = image.ImageUrl;
                string thumbnailUrl = image.ThumbnailUrl;

                if (imageFile != null)
                {
                    imageUrl = await UploadProductImageAsync(imageFile);
                    if (string.IsNullOrEmpty(imageUrl))
                    {
                        return new ErrorResponseModel<object> { Success = false, Message = "Image upload failed." };
                    }
                }

                if (thumbnailFile != null)
                {
                    thumbnailUrl = await UploadProductImageAsync(thumbnailFile);
                    if (string.IsNullOrEmpty(thumbnailUrl))
                    {
                        return new ErrorResponseModel<object> { Success = false, Message = "Thumbnail upload failed." };
                    }
                }

                image.ImageUrl = imageUrl;
                image.ThumbnailUrl = thumbnailUrl;
                image.UpdatedBy = updateProductImageDTO.UpdatedBy;
                image.UpdatedAt = DateTime.UtcNow;
                await _unitOfWork.ImageRepository.UpdateImageAsync(image);
                await _unitOfWork.SaveChangeAsync();

                productImage.UpdatedBy = updateProductImageDTO.UpdatedBy;
                productImage.UpdatedAt = DateTime.UtcNow;
                await _unitOfWork.ProductImageRepository.UpdateProductImageAsync(productImage);
                await _unitOfWork.SaveChangeAsync();

                return new SuccessResponseModel<object>
                {
                    Success = true,
                    Message = "Product image updated successfully."
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
        #endregion

        #region DeleteProductImage
        public async Task<ResponseModel> DeleteProductImageAsync(DeleteProductImageDTO deleteProductImageDTO)
        {
            try
            {
                var productImages = await _unitOfWork.ProductImageRepository.GetProductImageByProductIdAsync(deleteProductImageDTO.ProductId);
                var productImage = productImages.FirstOrDefault(pi => pi.ImageId == deleteProductImageDTO.ImageId);
                if (productImage == null)
                {
                    return new ErrorResponseModel<object> { Success = false, Message = "Product image not found." };
                }

                var image = await _unitOfWork.ImageRepository.GetByIdAsync(deleteProductImageDTO.ImageId);
                if (image == null)
                {
                    return new ErrorResponseModel<object> { Success = false, Message = "Image not found." };
                }

                image.IsDeleted = true;
                image.DeletedAt = DateTime.UtcNow;
                image.DeletedBy = deleteProductImageDTO.DeletedBy;
                _unitOfWork.ImageRepository.Update(image);
                await _unitOfWork.SaveChangeAsync();

                productImage.IsDeleted = true;
                productImage.DeletedAt = DateTime.UtcNow;
                productImage.DeletedBy = deleteProductImageDTO.DeletedBy;
                _unitOfWork.ProductImageRepository.Update(productImage);
                await _unitOfWork.SaveChangeAsync();

                return new SuccessResponseModel<object>
                {
                    Success = true,
                    Message = "Product image deleted successfully."
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
        #endregion

        private async Task<string> UploadProductImageAsync(IFormFile imageFile)
        {
            if (imageFile == null) return null;

            using (var stream = imageFile.OpenReadStream())
            {
                var fileName = $"{Guid.NewGuid()}_{imageFile.FileName}";
                return await _firebaseService.UploadProductImageAsync(stream, fileName);
            }
        }
    }
}
