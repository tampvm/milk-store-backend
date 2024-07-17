using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MilkStore.Service.Interfaces;
using MilkStore.Service.Models.ViewModels.AddressViewModels;

namespace MilkStore.API.Controllers
{
    public class AddressController : BaseController
    {
        private readonly IAddressService _addressService;

        public AddressController(IAddressService addressService)
        {
            _addressService = addressService;
        }

        #region Address Management
        [HttpGet]
        public async Task<IActionResult> GetAddressByUserIdAsync(string userId, int pageIndex = 0, int pageSize = 10)
        {
            var response = await _addressService.GetAddressByUserIdAsync(userId, pageIndex, pageSize);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPost]
        public async Task<IActionResult> AddAddressAsync(CreateAddressDTO model)
        {
            var response = await _addressService.AddAddressAsync(model);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAddressAsync(UpdateAddressDTO model)
        {
            var response = await _addressService.UpdateAddressAsync(model);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAddressAsync(int addressId)
        {
            var response = await _addressService.DeleteAddressAsync(addressId);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
        #endregion
    }
}
