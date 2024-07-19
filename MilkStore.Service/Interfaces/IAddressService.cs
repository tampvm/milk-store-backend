using MilkStore.Service.Models.ResponseModels;
using MilkStore.Service.Models.ViewModels.AddressViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore.Service.Interfaces
{
    public interface IAddressService
    {
        Task<ResponseModel> GetAddressByUserIdAsync(string userId, int pageIndex, int pageSize);
        Task<ResponseModel> AddAddressAsync(CreateAddressDTO model);
        Task<ResponseModel> UpdateAddressAsync(UpdateAddressDTO model);
        Task<ResponseModel> DeleteAddressAsync(int addressId);
    }
}
