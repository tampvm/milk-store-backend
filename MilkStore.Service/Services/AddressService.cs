using AutoMapper;
using MilkStore.Domain.Entities;
using MilkStore.Repository.Common;
using MilkStore.Repository.Interfaces;
using MilkStore.Service.Interfaces;
using MilkStore.Service.Models.ResponseModels;
using MilkStore.Service.Models.ViewModels.AddressViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore.Service.Services
{
    public class AddressService : IAddressService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AddressService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        #region Address Management
        public async Task<ResponseModel> GetAddressByUserIdAsync(string userId, int pageIndex, int pageSize)
        {
            var user = await _unitOfWork.AcccountRepository.GetByIdAsync(userId);

            if (user == null)
            {
                return new ResponseModel
                {
                    Success = false,
                    Message = "User not found."
                };
            }

            var addresses = await _unitOfWork.AddressRepository.GetAsync(
                filter: a => a.AccountId == userId,
                orderBy: a => a.OrderBy(a => !a.IsDefault).ThenBy(a => a.Id),  // !a.IsDefault: sắp xếp địa chỉ mặc định lên đầu, a.Id: sắp xếp theo Id
                pageIndex: pageIndex,                                          
                pageSize: pageSize);

            var addressDtos = _mapper.Map<Pagination<ViewUserAddressDTO>>(addresses);

            return new SuccessResponseModel<object>
            {
                Success = true,
                Message = "Address found successfully.",
                Data = addressDtos
            };
        }

        public async Task<ResponseModel> AddAddressAsync(CreateAddressDTO model)
        {
            var user = await _unitOfWork.AcccountRepository.GetByIdAsync(model.UserId);

            if (user == null)
            {
                return new ResponseModel
                {
                    Success = false,
                    Message = "User not found."
                };
            }

            var address = _mapper.Map<Address>(model);

            if (model.IsDefault)
            {
                var defaultAddress = await _unitOfWork.AddressRepository.GetDefaultAddressAsync(model.UserId);

                if (defaultAddress != null)
                {
                    defaultAddress.IsDefault = false;
                    _unitOfWork.AddressRepository.Update(defaultAddress);
                }
            }

            await _unitOfWork.AddressRepository.AddAsync(address);
            await _unitOfWork.SaveChangeAsync();

            //return new ResponseModel
            //{
            //    Success = true,
            //    Message = "Address added successfully."
            //};

            return new SuccessResponseModel<object>
            {
                Success = true,
                Message = "Address added successfully.",
                Data = new
                {
                    AddressId = address.Id,
                    UserId = address.AccountId,
                    FullName = address.FullName,
                    PhoneNumber = address.PhoneNumber,
                    AddressLine = address.AddressLine,
                    Ward = address.Ward,
                    District = address.District,
                    City = address.City,
                    IsDefault = address.IsDefault
                }
            };
        }

        public async Task<ResponseModel> UpdateAddressAsync(UpdateAddressDTO model)
        {
            var address = await _unitOfWork.AddressRepository.GetByIdAsync(model.AddressId);

            if (address == null)
            {
                return new ResponseModel
                {
                    Success = false,
                    Message = "Address not found."
                };
            }

            var user = await _unitOfWork.AcccountRepository.GetByIdAsync(model.UserId);

            if (user == null)
            {
                return new ResponseModel
                {
                    Success = false,
                    Message = "User not found."
                };
            }

            _mapper.Map(model, address);

            if (model.IsDefault)
            {
                var defaultAddress = await _unitOfWork.AddressRepository.GetDefaultAddressAsync(model.UserId);

                if (defaultAddress != null && defaultAddress.Id != model.AddressId)
                {
                    defaultAddress.IsDefault = false;
                    _unitOfWork.AddressRepository.Update(defaultAddress);
                }
            }

            _unitOfWork.AddressRepository.Update(address);
            await _unitOfWork.SaveChangeAsync();

            return new SuccessResponseModel<object>
            {
                Success = true,
                Message = "Address updated successfully.",
                Data = new
                {
                    AddressId = address.Id,
                    UserId = address.AccountId,
                    FullName = address.FullName,
                    PhoneNumber = address.PhoneNumber,
                    AddressLine = address.AddressLine,
                    Ward = address.Ward,
                    District = address.District,
                    City = address.City,
                    IsDefault = address.IsDefault
                }
            };
        }

        public async Task<ResponseModel> DeleteAddressAsync(int addressId)
        {
            var address = await _unitOfWork.AddressRepository.GetByIdAsync(addressId);

            if (address == null)
            {
                return new ResponseModel
                {
                    Success = false,
                    Message = "Address not found."
                };
            }

            _unitOfWork.AddressRepository.Delete(address);
            await _unitOfWork.SaveChangeAsync();

            return new ResponseModel
            {
                Success = true,
                Message = "Address deleted successfully."
            };
        }
        #endregion
    }
}
