using AutoMapper;
using MilkStore.Repository.Common;
using MilkStore.Repository.Interfaces;
using MilkStore.Service.Interfaces;
using MilkStore.Service.Models.ResponseModels;
using MilkStore.Service.Models.ViewModels.PointViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore.Service.Services
{
	public class PointService : IPointService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public PointService(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		// Get all points by account id
		public async Task<ResponseModel> GetPointsByAccountIdAsync(string accountId, int pageIndex, int pageSize)
		{
			var points = await _unitOfWork.PointRepository.GetPointsByAccountIdAsync(accountId, pageIndex, pageSize);
			var totalPoints = await _unitOfWork.PointRepository.GetTotalPointsByAccountIdAsync(accountId);

			var pointDtos = _mapper.Map<List<Pagination<ViewListPointDTO>>>(points);

			return new SuccessResponseModel<object>
			{
				Success = true,
				Message = "Points retrieved successfully.",
				Data = new
				{
					Points = pointDtos,
					TotalPoints = totalPoints
				}
			};
		}
	}
}
