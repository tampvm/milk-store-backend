using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MilkStore.Service.Interfaces;
using MilkStore.Service.Services;

namespace MilkStore.API.Controllers
{
	public class BrandController : BaseController
	{
		private readonly IBrandService _brandService;

		public BrandController(IBrandService brandService)
		{
			_brandService = brandService;
		}

		#region Brand management
		[HttpGet]
		public async Task<IActionResult> GetBrandsAsync(int pageIndex = 0, int pageSize = 10)
		{
			var brands = await _brandService.GetBrandsAsync(pageIndex, pageSize);
			return Ok(brands);
		}


		#endregion
	}
}
