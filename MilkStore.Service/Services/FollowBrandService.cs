using AutoMapper;
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

		public FollowBrandService(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		// Get all FollowBrand by BrandId
		public async Task<ResponseModel> GetFollowBrandByBrandIdAsync(int brandId, int pageIndex, int pageSize)
		{
			var followBrands = await _unitOfWork.FollowBrandRepository
								.GetFollowBrandByBrandIdAsync(brandId, pageIndex, pageSize);
			var followBrandResponse = _mapper.Map<IEnumerable<Pagination<ViewListFollowBrandDTO>>>(followBrands);

			return new SuccessResponseModel<object>
			{
				Success = true,
				Message = "Get all FollowBrand by BrandId successfully",
				Data = new
				{
					FollowBrands = followBrandResponse,
					Total = followBrands.Count()
				}
			};
		}

		// Get all FollowBrand by AccountId
		public async Task<ResponseModel> GetFollowBrandByAccountIdAsync(string accountId, int pageIndex, int pageSize)
		{
			var followBrands = await _unitOfWork.FollowBrandRepository
								.GetFollowBrandByAccountIdAsync(accountId, pageIndex, pageSize);
			var followBrandResponse = _mapper.Map<IEnumerable<Pagination<ViewListFollowBrandDTO>>>(followBrands);

			return new SuccessResponseModel<object>
			{
				Success = true,
				Message = "Get all FollowBrand by AccountId successfully",
				Data = new
				{
					FollowBrands = followBrandResponse,
					Total = followBrands.Count()
				}
			};
		}
	}
}
