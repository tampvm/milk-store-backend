using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using MilkStore.Repository.Interfaces;
using MilkStore.Service.Common;
using MilkStore.Service.Interfaces;
using MilkStore.Service.Models.ResponseModels;
using MilkStore.Service.Models.ViewModels.AgeRangeViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore.Service.Services
{
    public class AgeRangeService : BaseService, IAgeRangeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public AgeRangeService(IUnitOfWork unitOfWork, IMapper mapper, ICurrentTime currentTime, IClaimsService claimsService, AppConfiguration appConfiguration, ISmsSender smsSender, IEmailSender emailSender, IMemoryCache cache) : base(unitOfWork, mapper, currentTime, claimsService, appConfiguration, smsSender, emailSender, cache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ResponseModel> GetAllAgeRangeAsync()
        {
            try
            {
                var ageRanges = await _unitOfWork.AgeRangeRepository.GetAllAgeRangeAsync();
                var ageRangeDTO = _mapper.Map<List<ViewListAgeRangeDTO>>(ageRanges);
                return new SuccessResponseModel<object>
                {
                    Success = true,
                    Message = "AgeRange retrieved successfully.",
                    Data = ageRangeDTO
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
    }
}
