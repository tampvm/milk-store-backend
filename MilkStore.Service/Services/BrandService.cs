﻿using AutoMapper;
using Microsoft.AspNetCore.Identity;
using MilkStore.Domain.Entities;
using MilkStore.Repository.Common;
using MilkStore.Repository.Interfaces;
using MilkStore.Service.Interfaces;
using MilkStore.Service.Models.ResponseModels;
using MilkStore.Service.Models.ViewModels.BrandViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore.Service.Services
{
	public class BrandService : IBrandService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public BrandService(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		#region Brand management
		// Get all brands
		public async Task<ResponseModel> GetBrandsAsync(int pageIndex, int pageSize)
		{
			var brands = await _unitOfWork.BrandRepository.GetAsync(
				pageIndex: pageIndex,
				pageSize: pageSize
				);

			var brandDtos = _mapper.Map<Pagination<ViewListBrandDTO>>(brands);

			return new SuccessResponseModel<object>
			{
				Success = true,
				Message = "Brands retrieved successfully.",
				Data = brandDtos
			};
		}

		// Create a new brand
		public async Task<ResponseModel> CreateBrandAsync(CreateBrandDTO model)
		{
			var existingBrand = await _unitOfWork.BrandRepository.FindByNameAsync(model.Name);

			if (existingBrand == null)
			{
				var mapper = _mapper.Map<Brand>(model);
				await _unitOfWork.BrandRepository.AddAsync(mapper);
				await _unitOfWork.SaveChangeAsync();

				return new SuccessResponseModel<object>
				{
					Success = true,
					Message = "Brand create successfully."
				};
			}
			else
			{
				return new ErrorResponseModel<object>
				{
					Success = false,
					Message = "Brand already exists."
				};
			}
		}

		// Update a brand
		public async Task<ResponseModel> UpdateBrandAsync(UpdateBrandDTO model)
		{
			var brand = await _unitOfWork.BrandRepository.GetByIdAsync(model.Id);

			if (brand == null)
			{
				return new ErrorResponseModel<object>
				{
					Success = false,
					Message = "Brand not found."
				};
			}

			_mapper.Map(model, brand);
			_unitOfWork.BrandRepository.Update(brand);
			await _unitOfWork.SaveChangeAsync();

			return new SuccessResponseModel<object>
			{
				Success = true,
				Message = "Brand updated successfully."
			};
		}

		// Delete a brand
		public async Task<ResponseModel> DeleteBrandAsync(int id)
		{
			var brand = await _unitOfWork.BrandRepository.GetByIdAsync(id);

			if (brand == null)
			{
				return new ErrorResponseModel<object>
				{
					Success = false,
					Message = "Brand not found."
				};
			}

			brand.IsDeleted = true;
			_unitOfWork.BrandRepository.Update(brand);
			await _unitOfWork.SaveChangeAsync();

			return new SuccessResponseModel<object>
			{
				Success = true,
				Message = "Brand deleted successfully."
			};
		}
		#endregion
	}
}