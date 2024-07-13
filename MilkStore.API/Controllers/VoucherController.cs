using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MilkStore.Service.Interfaces;
using MilkStore.Service.Models.ViewModels.VoucherViewModels;

namespace MilkStore.API.Controllers
{
	public class VoucherController : BaseController
	{
		private readonly IVoucherService _voucherService;

		public VoucherController(IVoucherService voucherService)
		{
			_voucherService = voucherService;
		}

		#region Voucher management
		// Get all vouchers
		[HttpGet]
		public async Task<IActionResult> GetVouchersAsync(int pageIndex = 0, int pageSize = 10)
		{
			var vouchers = await _voucherService.GetVouchersAsync(pageIndex, pageSize);
			return Ok(vouchers);
		}

		// Create a new voucher
		[HttpPost]
		public async Task<IActionResult> CreateVoucherAsync(CreateVoucherDTO model)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var result = await _voucherService.CreateVoucherAsync(model);

			if (result.Success)
			{
				return Ok(result);
			}
			else
			{
				return BadRequest(result);
			}
		}

		// Update a voucher
		[HttpPut]
		public async Task<IActionResult> UpdateVoucherAsync(UpdateVoucherDTO model)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var result = await _voucherService.UpdateVoucherAsync(model);

			if (result.Success)
			{
				return Ok(result);
			}
			else
			{
				return BadRequest(result);
			}
		}

		// Delete a voucher
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteVoucherAsync(int id)
		{
			var result = await _voucherService.DeleteVoucherAsync(id);

			if (result.Success)
			{
				return Ok(result);
			}
			else
			{
				return BadRequest(result);
			}
		}
		#endregion
	}
}
