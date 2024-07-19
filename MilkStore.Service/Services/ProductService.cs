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

namespace MilkStore.Service.Services
{
    public class ProductService : BaseService, IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductService(IUnitOfWork unitOfWork, IMapper mapper, ICurrentTime currentTime, IClaimsService claimsService, AppConfiguration appConfiguration, ISmsSender smsSender, IEmailSender emailSender, IMemoryCache cache)
            : base(unitOfWork, mapper, currentTime, claimsService, appConfiguration, smsSender, emailSender, cache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
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
        public async Task<ResponseModel> CreateProductAsync(CreateProductDTO productCreateDTO)
        {
            //Check empty
            if (productCreateDTO == null)
                return new ErrorResponseModel<object> { Success = false, Message = "Empty product data." };

            //Check validate
            string validationMessage = await CheckValidate(productCreateDTO);
            if (validationMessage != "Success")
                return new ErrorResponseModel<object> { Success = false, Message = validationMessage };

            try
            {
                //Product
                var product = _mapper.Map<Product>(productCreateDTO);
                product.Id = GenerateRandomString(20);
                product.Active = true;
                product.IsDeleted = false;
                product.CreatedAt = DateTime.UtcNow;
                await _unitOfWork.ProductRepository.AddAsync(product);
                await _unitOfWork.SaveChangeAsync();

                return new SuccessResponseModel<object> { Success = true, Message = "Product created successfully." };
            }
            catch (Exception ex)
            {
                return new ErrorResponseModel<object> { Success = false, Message = ex.Message };
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

        #region UpdateProduct
        //Update product
        public async Task<ResponseModel> UpdateProductAsync(UpdateProductDTO productUpdateDTO)
        {
            //Check empty
            if (productUpdateDTO == null)
                return new ErrorResponseModel<object> { Success = false, Message = "Empty product data." };

            //Check validate
            string validationMessage = await CheckUpdateValidate(productUpdateDTO);
            if (validationMessage != "Success")
                return new ErrorResponseModel<object> { Success = false, Message = validationMessage };

            //Get product
            var product = await _unitOfWork.ProductRepository.GetProductByIdAsync(productUpdateDTO.Id);
            if (product == null) return new ErrorResponseModel<object> { Success = false, Message = "Not found product." };

            //Product
            product.Name = productUpdateDTO.Name;
            product.Sku = productUpdateDTO.Sku;
            product.Description = productUpdateDTO.Description;
            product.Price = productUpdateDTO.Price;
            product.Discount = productUpdateDTO.Discount;
            product.Quantity = productUpdateDTO.Quantity;
            product.Weight = productUpdateDTO.Weight;
            product.TypeId = productUpdateDTO.TypeId;
            product.BrandId = productUpdateDTO.BrandId;
            product.AgeId = productUpdateDTO.AgeId;
            product.UpdatedBy = productUpdateDTO.UpdatedBy;
            product.UpdatedAt = DateTime.UtcNow;
            
            try
            {
                await _unitOfWork.ProductRepository.UpdateProductAsync(product);
                await _unitOfWork.SaveChangeAsync();
                return new SuccessResponseModel<object> { Success = true, Message = "Product updated successfully." };
            }
            catch (Exception ex)
            {
                return new ErrorResponseModel<object> { Success = false, Message = ex.Message };
            }
        }

        //Check update validate
        private async Task<string> CheckUpdateValidate(UpdateProductDTO productUpdateDTO)
        {
            try
            {
                var products = await _unitOfWork.ProductRepository.GetAllProductsAsync();

                if (products.Any(x => x.Name == productUpdateDTO.Name && x.Id != productUpdateDTO.Id)) return "NameExists";
                if (products.Any(x => x.Sku == productUpdateDTO.Sku && x.Id != productUpdateDTO.Id)) return "SkuExists";
                if (productUpdateDTO.Price < 0) return "PriceInvalid";
                if (productUpdateDTO.Quantity < 0) return "QuantityInvalid";
                if (productUpdateDTO.Weight < 0) return "WeightInvalid";
                if (productUpdateDTO.TypeId < 0) return "TypeIdInvalid";
                if (productUpdateDTO.BrandId < 0) return "BrandIdInvalid";
                if (productUpdateDTO.AgeId < 0) return "AgeIdInvalid";
                return "Success";
            }
            catch (Exception ex) {
                return ex.Message;
            }

        }
        #endregion

        #region DeleteProduct
        public async Task<ResponseModel> DeleteProductAsync(DeleteProductDTO deleteProductDTO)
        {
            try {
                var product = await _unitOfWork.ProductRepository.GetProductByIdAsync(deleteProductDTO.Id);
                if (product == null) return new ErrorResponseModel<object> { Success = false, Message = "Not found product." };
                product.IsDeleted = true;
                product.DeletedAt = DateTime.UtcNow;
                product.DeletedBy = deleteProductDTO.DeletedBy;
                _unitOfWork.ProductRepository.Update(product);
                await _unitOfWork.SaveChangeAsync();
                return new SuccessResponseModel<object> { Success = true, Message = "Product deleted successfully." };
            }
            catch (Exception ex)
            {
                return new ErrorResponseModel<object> { Success = false, Message = ex.Message };
            }
        }
        #endregion

        #region RestoreProduct
        public async Task<ResponseModel> RestoreProductAsync(RestoreProductDTO restoreProductDTO)
        {
            try
            {
                var product = await _unitOfWork.ProductRepository.GetProductByIdAsync(restoreProductDTO.Id);
                if (product == null) return new ErrorResponseModel<object> { Success = false, Message = "Not found product." };
                product.IsDeleted = false;
                product.UpdatedAt = DateTime.UtcNow;
                product.UpdatedBy = restoreProductDTO.UpdatedBy;
                await _unitOfWork.ProductRepository.UpdateProductAsync(product);
                await _unitOfWork.SaveChangeAsync();
                return new SuccessResponseModel<object> { Success = true, Message = "Product restored successfully." };
            }
            catch (Exception ex)
            {
                return new ErrorResponseModel<object> { Success = false, Message = ex.Message };
            }
        }
        #endregion

        #region UpdateProductStatus
        public async Task<ResponseModel> UpdateProductStatusAsync(ChangeStatusProductDTO model)
        {
            try
            {
                var product = await _unitOfWork.ProductRepository.GetProductByIdAsync(model.Id);
                if (product == null) return new ErrorResponseModel<object> { Success = false, Message = "Not found product." };
                product.Active = !product.Active;
                product.UpdatedAt = DateTime.UtcNow;
                product.UpdatedBy = model.UpdatedBy;
                await _unitOfWork.ProductRepository.UpdateProductAsync(product);
                await _unitOfWork.SaveChangeAsync();
                return new SuccessResponseModel<object> { Success = true, Message = "Product status updated successfully." };
            }
            catch (Exception ex) {
                return new ErrorResponseModel<object> { Success = false, Message = ex.Message };
            }
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

        #region GetProductBySku
        public async Task<ResponseModel> GetProductBySkuAsync(string sku)
        {
            try
            {
                var product = await _unitOfWork.ProductRepository.GetProductBySKUAsync(sku);
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
        public async Task<ResponseModel> GetProductsPaginationAsync(string keySearch, int pageIndex, int pageSize)
        {
            try
            {
                var products = await _unitOfWork.ProductRepository.GetAllProductsAsync();

                if (!string.IsNullOrEmpty(keySearch))
                {
                    products = products
                        .Where(p => p.Name.Contains(keySearch) || p.Description.Contains(keySearch))
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();
                }
                else
                {
                    products = products
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();
                }

                if (products == null || !products.Any())
                {
                    return new ErrorResponseModel<object> { Success = false, Message = "No products found." };
                }

                var productDTOs = _mapper.Map<List<ViewListProductsDTO>>(products);
                return new SuccessResponseModel<object> { Success = true, Message = "Products retrieved successfully.", Data = productDTOs };
            }
            catch (Exception ex)
            {
                return new ErrorResponseModel<object> { Success = false, Message = $"An error occurred: {ex.Message}" };
            }
        }
        #endregion
    }
}
