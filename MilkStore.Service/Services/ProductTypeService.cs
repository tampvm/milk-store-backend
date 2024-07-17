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
    }
}
