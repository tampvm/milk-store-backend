using AutoMapper;
using Microsoft.AspNetCore.Identity;
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
				filter: r => !r.IsDeleted,
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


		#endregion
	}
}
