using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using MilkStore.Repository.Common;
using MilkStore.Repository.Interfaces;
using MilkStore.Service.Common;
using MilkStore.Service.Interfaces;
using MilkStore.Service.Models.ResponseModels;
using MilkStore.Service.Models.ViewModels.ProductViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore.Service.Services
{
    public class ProductService : BaseService, IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductService(IUnitOfWork unitOfWork, IMapper mapper, ICurrentTime currentTime, IClaimsService claimsService, AppConfiguration appConfiguration, ISmsSender smsSender, IEmailSender emailSender, IMemoryCache cache) : base(unitOfWork, mapper, currentTime, claimsService, appConfiguration, smsSender, emailSender, cache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ResponseModel> GetAllProductsAsync()
        {
            try
            {
                var products = await _unitOfWork.ProductRepository.GetAllProductsAsync();
                var productDTO = _mapper.Map<List<ViewListProductsDTO>>(products);
                return new SuccessResponseModel<object>
                {
                    Success = true,
                    Message = "Products retrieved successfully.",
                    Data = productDTO
                };
            }
            catch (Exception ex) {
                return new ErrorResponseModel<object>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }


        public async Task<ResponseModel> GetProductByIdAsync(string productId)
        {
            try
            {
                var product = await _unitOfWork.ProductRepository.GetProductByIdAsync(productId);
                var productDTO = _mapper.Map<ViewListProductsDTO>(product);
                return new SuccessResponseModel<object>
                {
                    Success = true,
                    Message = "Product retrieved successfully.",
                    Data = productDTO
                };
            }
            catch (Exception ex) {
                return new ErrorResponseModel<object>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        //public async Task<ResponseModel> GetProductsPaginationAsync(int pageIndex, int pageSize)
        //{
        //    try
        //    {
        //        var products = await _unitOfWork.ProductRepository.GetProductsPaginationAsync(pageIndex, pageSize);
        //        var productDTO = _mapper.Map<Pagination<List<ViewListProductsDTO>>>(products);
        //        return new SuccessResponseModel<object>
        //        {
        //            Success = true,
        //            Message = "Products retrieved successfully.",
        //            Data = productDTO
        //        };
        //    }
        //    catch (Exception ex) {
        //        return new ErrorResponseModel<object>
        //        {
        //            Success = false,
        //            Message = ex.Message
        //        };
        //    }
        //}
    }
}
