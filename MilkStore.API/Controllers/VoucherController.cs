using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MilkStore.Service.Interfaces;

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
		#endregion
	}
}
