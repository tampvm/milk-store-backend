using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using MilkStore.Domain.Entities;
using MilkStore.Repository.Common;
using MilkStore.Repository.Interfaces;
using MilkStore.Service.Common;
using MilkStore.Service.Interfaces;
using MilkStore.Service.Models.ResponseModels;
using MilkStore.Service.Models.ViewModels.ProductViewModels;
using MilkStore.Service.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MilkStore.Service.Services
{
    public class ProductService : BaseService, IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IFirebaseService _firebaseService;

        public ProductService(IUnitOfWork unitOfWork, IMapper mapper, ICurrentTime currentTime, IClaimsService claimsService, AppConfiguration appConfiguration, ISmsSender smsSender, IEmailSender emailSender, IMemoryCache cache, IFirebaseService firebaseService)
            : base(unitOfWork, mapper, currentTime, claimsService, appConfiguration, smsSender, emailSender, cache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _firebaseService = firebaseService;
        }
        #region CreateProduct
        //Random product id
        private string GenerateRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }
        
        //Create product
        public async Task<ResponseModel> CreateProductAsync(CreateProductDTO productCreateDTO, IFormFile imageFile, IFormFile thumbnailFile)
        {
            //Check empty
            if (productCreateDTO == null)
                return new ErrorResponseModel<object> { Success = false, Message = "Empty product data." };

            //Check validate
            string validationMessage = await CheckValidate(productCreateDTO);
            if (validationMessage != "Success")
                return new ErrorResponseModel<object> { Success = false, Message = validationMessage };

            //Upload image
            string imageUrl = await UploadProductImageAsync(imageFile);
            string thumbnailUrl = await UploadProductImageAsync(thumbnailFile);
            if (string.IsNullOrEmpty(imageUrl) || string.IsNullOrEmpty(thumbnailUrl))
                return new ErrorResponseModel<object> { Success = false, Message = "Image upload failed." };

            //Product
            var product = _mapper.Map<Product>(productCreateDTO);
            product.Id = GenerateRandomString(12);
            product.Active = true;
            product.IsDeleted = false;
            product.CreatedAt = DateTime.UtcNow;

            //Image
            var image = new Image
            {
                ImageUrl = imageUrl,
                ThumbnailUrl = thumbnailUrl,
                Type = "Product",
                CreatedAt = DateTime.UtcNow,
                CreatedBy = productCreateDTO.CreatedBy,
                IsDeleted = false
            };

            try
            {
                await _unitOfWork.ProductRepository.AddAsync(product); //Add product
                await _unitOfWork.ImageRepository.AddAsync(image); //Add image
                await _unitOfWork.SaveChangeAsync(); //Save

                //Product image
                var productImage = new ProductImage
                {
                    ProductId = product.Id,
                    ImageId = image.Id,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = productCreateDTO.CreatedBy,
                    IsDeleted = false
                };

                await _unitOfWork.ProductImageRepository.AddAsync(productImage); //Add product image
                await _unitOfWork.SaveChangeAsync(); //Save

                return new SuccessResponseModel<object> { Success = true, Message = "Product created successfully." };
            }
            catch (Exception ex)
            {
                return new ErrorResponseModel<object> { Success = false, Message = ex.Message };
            }
        }

        private async Task<string> UploadProductImageAsync(IFormFile imageFile)
        {
            if (imageFile == null) return null;

            using (var stream = imageFile.OpenReadStream())
            {
                var fileName = $"{Guid.NewGuid()}_{imageFile.FileName}";
                return await _firebaseService.UploadProductImageAsync(stream, fileName);
            }
        }

        private async Task<string> CheckValidate(CreateProductDTO productCreateDTO)
        {
            var products = await _unitOfWork.ProductRepository.GetAllProductsAsync();

            if (products.Any(x => x.Name == productCreateDTO.Name)) return "NameExists";
            if (products.Any(x => x.Sku == productCreateDTO.Sku)) return "SkuExists";
            if (productCreateDTO.Price < 0) return "PriceInvalid";
            if (productCreateDTO.Quantity < 0) return "QuantityInvalid";
            if (productCreateDTO.Weight < 0) return "WeightInvalid";
            if (productCreateDTO.TypeId < 0) return "TypeIdInvalid";
            if (productCreateDTO.BrandId < 0) return "BrandIdInvalid";
            if (productCreateDTO.AgeId < 0) return "AgeIdInvalid";

            return "Success";
        }
        #endregion

        #region GetAllProducts
        public async Task<ResponseModel> GetAllProductsAsync()
        {
            try
            {
                var products = await _unitOfWork.ProductRepository.GetAllProductsAsync();
                var productDTOs = _mapper.Map<List<ViewListProductsDTO>>(products);
                return new SuccessResponseModel<object> { Success = true, Message = "Products retrieved successfully.", Data = productDTOs };
            }
            catch (Exception ex)
            {
                return new ErrorResponseModel<object> { Success = false, Message = ex.Message };
            }
        }
        #endregion

        #region GetProductById
        public async Task<ResponseModel> GetProductByIdAsync(string productId)
        {
            try
            {
                var product = await _unitOfWork.ProductRepository.GetProductByIdAsync(productId);
                var productDTO = _mapper.Map<ViewListProductsDTO>(product);
                return new SuccessResponseModel<object> { Success = true, Message = "Product retrieved successfully.", Data = productDTO };
            }
            catch (Exception ex)
            {
                return new ErrorResponseModel<object> { Success = false, Message = ex.Message };
            }
        }
        #endregion

        #region GetProductsPagination
        public async Task<ResponseModel> GetProductsPaginationAsync(int pageIndex, int pageSize)
        {
            try
            {
                var products = await _unitOfWork.ProductRepository.GetAsync(
                    pageSize: pageSize,
                    pageIndex: pageIndex);
                var productDTOs = _mapper.Map<Pagination<ViewListProductsDTO>>(products);
                return new SuccessResponseModel<object> { Success = true, Message = "Products retrieved successfully.", Data = productDTOs };
            }
            catch (Exception ex)
            {
                return new ErrorResponseModel<object> { Success = false, Message = ex.Message };
            }
        }
        #endregion
    }
}
