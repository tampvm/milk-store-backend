using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MilkStore.Service.Interfaces;
using MilkStore.Service.Models.ViewModels.FollowBrandViewModels;

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
		public async Task<IActionResult> GetFollowBrandByBrandIdAsync(int brandId, int pageIndex = 0, int pageSize = 10)
		{
			var response = await _followBrandService.GetFollowBrandByBrandIdAsync(brandId, pageIndex, pageSize);
			return Ok(response);
		}

		// Get all FollowBrand by AccountId
		[HttpGet]
		public async Task<IActionResult> GetFollowBrandByAccountIdAsync(string accountId, int pageIndex = 0, int pageSize = 10)
		{
			var response = await _followBrandService.GetFollowBrandByAccountIdAsync(accountId, pageIndex, pageSize);
			return Ok(response);
		}

		// User follows brand
		[HttpPost]
		public async Task<IActionResult> UserFollowsBrand(UserFollowsBrandDTO model)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var response = await _followBrandService.UserFollowsBrand(model);

			if (response.Success)
			{
				return Ok(response);
			}
			else
			{
				return BadRequest(response);
			}
		}
	}
}
