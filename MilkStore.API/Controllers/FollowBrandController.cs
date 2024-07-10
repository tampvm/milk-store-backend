using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MilkStore.Service.Interfaces;

namespace MilkStore.API.Controllers
{
	public class FollowBrandController : BaseController
	{
		private readonly IFollowBrandService _followBrandService;

		public FollowBrandController(IFollowBrandService followBrandService)
		{
			_followBrandService = followBrandService;
		}

		// Get all FollowBrand by BrandId
		[HttpGet]
		public async Task<IActionResult> GetFollowBrandByBrandIdAsync(int brandId, int pageIndex, int pageSize)
		{
			var response = await _followBrandService.GetFollowBrandByBrandIdAsync(brandId, pageIndex, pageSize);
			return Ok(response);
		}

		// Get all FollowBrand by AccountId
		[HttpGet]
		public async Task<IActionResult> GetFollowBrandByAccountIdAsync(string accountId, int pageIndex, int pageSize)
		{
			var response = await _followBrandService.GetFollowBrandByAccountIdAsync(accountId, pageIndex, pageSize);
			return Ok(response);
		}
	}
}
