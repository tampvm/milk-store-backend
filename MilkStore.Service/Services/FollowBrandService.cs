using AutoMapper;
using Microsoft.Identity.Client;
using MilkStore.Domain.Entities;
using MilkStore.Repository.Common;
using MilkStore.Repository.Interfaces;
using MilkStore.Service.Interfaces;
using MilkStore.Service.Models.ResponseModels;
using MilkStore.Service.Models.ViewModels.FollowBrandViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore.Service.Services
{
	public class FollowBrandService : IFollowBrandService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		private readonly IBrandService _brandService;

		public FollowBrandService(IUnitOfWork unitOfWork, IMapper mapper, IBrandService brandService)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
			_brandService = brandService;
		}

		// Get all FollowBrand by BrandId
		public async Task<ResponseModel> GetFollowBrandByBrandIdAsync(int brandId, int pageIndex, int pageSize)
		{
			var followBrands = await _unitOfWork.FollowBrandRepository
									.GetAsync(
											filter: x => x.BrandId == brandId,
											pageIndex: pageIndex,
											pageSize: pageSize
											);
			var followBrandResponse = _mapper.Map<Pagination<ViewListFollowBrandDTO>>(followBrands);

			return new SuccessResponseModel<object>
			{
				Success = true,
				Message = "Get all FollowBrand by BrandId successfully",
				Data = followBrandResponse
			};
		}

		// Get all FollowBrand by AccountId
		public async Task<ResponseModel> GetFollowBrandByAccountIdAsync(string accountId, int pageIndex, int pageSize)
		{
			var followBrands = await _unitOfWork.FollowBrandRepository
									.GetAsync(
											filter: x => x.AccountId == accountId,
											pageIndex: pageIndex,
											pageSize: pageSize
											);

			var followBrandDtos = _mapper.Map<Pagination<ViewFollowBrandByUserDTO>>(followBrands);

			foreach (var item in followBrands.Items)
			{
				var brandDetail = await _brandService.ViewBrandDetailAsync(item.BrandId);

				var followBrandDto = followBrandDtos.Items.FirstOrDefault(x => x.Id == item.Id);
				if (followBrandDto != null)
				{
					followBrandDto.Brand = brandDetail;
				}
			}

			return new SuccessResponseModel<object>
			{
				Success = true,
				Message = "Get all FollowBrand by AccountId successfully",
				Data = followBrandDtos
			};
		}

		// User follows brand
		public async Task<ResponseModel> UserFollowsBrand(UserFollowsBrandDTO model)
		{
			var existingFollowBrand = await _unitOfWork.FollowBrandRepository.CheckUserFollowsBrandAsync(model.AccountId, model.BrandId);

			if (!existingFollowBrand)
			{
				var followBrand = _mapper.Map<FollowBrand>(model);
				await _unitOfWork.FollowBrandRepository.AddAsync(followBrand);
				await _unitOfWork.SaveChangeAsync();

				return new SuccessResponseModel<object>
				{
					Success = true,
					Message = "Follow a brand successfully."
				};
			}
			else
			{
				return new ErrorResponseModel<object>
				{
					Success = false,
					Message = "You have already followed this brand."
				};
			}
		}

		// User unfollows brand
		public async Task<ResponseModel> UserUnfollowsBrandAsync(string AccountId, int BrandId)
		{
			var model = await _unitOfWork.FollowBrandRepository.GetFollowBrandByUserAsync(AccountId, BrandId);

			if (model != null)
			{
				_unitOfWork.FollowBrandRepository.Delete(model.Id);
			}

			await _unitOfWork.SaveChangeAsync();

			return new SuccessResponseModel<object>
			{
				Success = true,
				Message = "Unfollow successfully."
			};
		}
	}
}
