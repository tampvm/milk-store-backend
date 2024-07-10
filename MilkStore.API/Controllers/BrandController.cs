﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MilkStore.Service.Interfaces;
using MilkStore.Service.Models.ResponseModels;
using MilkStore.Service.Models.ViewModels.BrandViewModels;

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

		[HttpPost]
		public async Task<IActionResult> CreateBrandAsync(CreateBrandDTO model)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var response = await _brandService.CreateBrandAsync(model);

			if (response.Success)
			{
				return Ok(response);
			}
			else
			{
				return BadRequest(response);
			}
		}

		[HttpPost]
		public async Task<IActionResult> UpdateBrandAsync(UpdateBrandDTO model)
		{
			var response = await _brandService.UpdateBrandAsync(model);

			if (response.Success)
			{
				return Ok(response);
			}
			else
			{
				return BadRequest(response);
			}
		}

		#endregion
	}
}