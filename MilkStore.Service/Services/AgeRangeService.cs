using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using MilkStore.Domain.Entities;
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

        public async Task<ResponseModel> GetAgeRangeByIdAsync(int id)
        {
            try
            {
                var ageRange = await _unitOfWork.AgeRangeRepository.GetAgeRangeByIdAsync(id);
                var ageRangeDTO = _mapper.Map<ViewListAgeRangeDTO>(ageRange);
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

        public async Task<ResponseModel> CreateAgeRangeAsync(CreateAgeRangeDTO createAgeRangeDTO)
        {
            try
            {
                var ageRangeExist = await _unitOfWork.AgeRangeRepository.GetAgeRangeByNameAsync(createAgeRangeDTO.Name);
                if (ageRangeExist != null) return new ErrorResponseModel<object> { Success = false, Message = "AgeRange already exists." };

                var ageRange = _mapper.Map<AgeRange>(createAgeRangeDTO);
                ageRange.CreatedAt = DateTime.UtcNow;
                ageRange.IsDeleted = false;
                await _unitOfWork.AgeRangeRepository.AddAsync(ageRange);
                await _unitOfWork.SaveChangeAsync();
                return new SuccessResponseModel<object> { Success = true, Message = "AgeRange created successfully." };
            }
            catch (Exception ex)
            {
                return new ErrorResponseModel<object> { Success = false, Message = ex.Message };
            }
        }

        public async Task<ResponseModel> UpdateAgeRangeAsync(UpdateAgeRangeDTO updateAgeRangeDTO)
        {
            try
            {
                var ageRangeExist = await _unitOfWork.AgeRangeRepository.GetAgeRangeByNameAsync(updateAgeRangeDTO.Name);
                if (ageRangeExist != null && ageRangeExist.Id != updateAgeRangeDTO.Id) return new ErrorResponseModel<object> { Success = false, Message = "AgeRange already exists." };

                var ageRange = await _unitOfWork.AgeRangeRepository.GetAgeRangeByIdAsync(updateAgeRangeDTO.Id);
                if (ageRange == null) return new ErrorResponseModel<object> { Success = false, Message = "Not found ageRange." };
                
                if (ageRange.Active == false && ageRange.IsDeleted == true && updateAgeRangeDTO.Active == true) return new ErrorResponseModel<object> { Success = false, Message = "AgeRange is not active." };

                ageRange.UpdatedAt = DateTime.UtcNow;
                ageRange.Name = updateAgeRangeDTO.Name;
                ageRange.UpdatedBy = updateAgeRangeDTO.UpdatedBy;
                ageRange.Active = updateAgeRangeDTO.Active;
                ageRange.Description = updateAgeRangeDTO.Description;

                await _unitOfWork.AgeRangeRepository.UpdateAgeRangeAsync(ageRange);
                await _unitOfWork.SaveChangeAsync();
                return new SuccessResponseModel<object> { Success = true, Message = "AgeRange updated successfully." };
            }
            catch (Exception ex)
            {
                return new ErrorResponseModel<object> { Success = false, Message = ex.Message };
            }
        }

        public async Task<ResponseModel> DeleteAgeRangeAsync(DeleteAgeRangeDTO deleteAgeRangeDTO)
        {
            try
            {
                var product = await _unitOfWork.ProductRepository.GetProductByAgeRangeIdAsync(deleteAgeRangeDTO.Id);
                if (product != null) return new ErrorResponseModel<object> { Success = false, Message = "AgeRange is used in product." };

                var ageRange = await _unitOfWork.AgeRangeRepository.GetAgeRangeByIdAsync(deleteAgeRangeDTO.Id);
                if (ageRange == null) return new ErrorResponseModel<object> { Success = false, Message = "Not found ageRange." };
                ageRange.IsDeleted = true;
                ageRange.Active = false;
                ageRange.DeletedAt = DateTime.UtcNow;
                await _unitOfWork.AgeRangeRepository.UpdateAgeRangeAsync(ageRange);
                await _unitOfWork.SaveChangeAsync();
                return new SuccessResponseModel<object> { Success = true, Message = "AgeRange deleted successfully." };
            }
            catch (Exception ex)
            {
                return new ErrorResponseModel<object> { Success = false, Message = ex.Message };
            }
        }

        public async Task<ResponseModel> RestoreAgeRangeAsync(RestoreAgeRangeDTO restoreAgeRangeDTO)
        {
            try
            {
                var ageRange = await _unitOfWork.AgeRangeRepository.GetAgeRangeByIdAsync(restoreAgeRangeDTO.Id);
                if (ageRange == null) return new ErrorResponseModel<object> { Success = false, Message = "Not found ageRange." };
                ageRange.IsDeleted = false;
                ageRange.UpdatedAt = DateTime.UtcNow;
                await _unitOfWork.AgeRangeRepository.UpdateAgeRangeAsync(ageRange);
                await _unitOfWork.SaveChangeAsync();
                return new SuccessResponseModel<object> { Success = true, Message = "AgeRange restored successfully." };
            }
            catch (Exception ex)
            {
                return new ErrorResponseModel<object> { Success = false, Message = ex.Message };
            }
        }
    }
}
