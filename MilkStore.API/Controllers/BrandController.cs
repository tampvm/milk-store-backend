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
		// Get all brands
		[HttpGet]
		public async Task<IActionResult> GetBrandsAsync(int pageIndex = 0, int pageSize = 10)
		{
			var brands = await _brandService.GetBrandsAsync(pageIndex, pageSize);
			return Ok(brands);
		}

		// Get a detail brand 
		[HttpGet("{id}")]
		public async Task<IActionResult> ViewBrandDetailAsync(int id)
		{
			var brand = await _brandService.ViewBrandDetailModelAsync(id);
			return Ok(brand);
		}

		// Create a new brand
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

		// Update a brand
		[HttpPut]
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

		// Delete a brand
		[HttpDelete]
		public async Task<IActionResult> DeleteBrandAsync(int id)
		{
			var response = await _brandService.DeleteBrandAsync(id);

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
