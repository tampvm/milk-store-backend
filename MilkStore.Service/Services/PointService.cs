using AutoMapper;
using MilkStore.Domain.Entities;
using MilkStore.Domain.Enums;
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

		// Get all points by order id
		public async Task<ResponseModel> GetPointsByOrderIdAsync(string orderId, int pageIndex, int pageSize)
		{
			var points = await _unitOfWork.PointRepository.GetPointsByOrderIdAsync(orderId, pageIndex, pageSize);

			var pointDtos = _mapper.Map<List<Pagination<ViewListPointDTO>>>(points);

			return new SuccessResponseModel<object>
			{
				Success = true,
				Message = "Points retrieved successfully.",
				Data = pointDtos
			};
		}

		// Get total points by account id
		public async Task<ResponseModel> GetTotalPointsByAccountIdAsync(string accountId)
		{
			var totalPoints = await _unitOfWork.PointRepository.GetTotalPointsByAccountIdAsync(accountId);

			return new SuccessResponseModel<object>
			{
				Success = true,
				Message = "Total points retrieved successfully.",
				Data = new
				{
					TotalPoints = totalPoints
				}
			};
		}

		// Spend points
		public async Task<ResponseModel> SpendingPointsAsync(PointsTradingDTO model)
		{
			var totalPoints = await _unitOfWork.PointRepository.GetTotalPointsByAccountIdAsync(model.AccountId);

			if (totalPoints < model.Points)
			{
				return new ErrorResponseModel<object>
				{
					Success = false,
					Message = "Not enough points to spend."
				};
			}

			var point = _mapper.Map<Point>(model);
			point.TransactionType = PointTransactionTypeEnums.Spending.ToString(); 
			point.Points = -model.Points; // Subtract points

			await _unitOfWork.PointRepository.AddAsync(point);
			await _unitOfWork.SaveChangeAsync();

			return new SuccessResponseModel<object>
			{
				Success = true,
				Message = "Points spent successfully."
			};
		}


		// Earn points
		public async Task<ResponseModel> EarningPointsAsync(PointsTradingDTO model)
		{
			var point = _mapper.Map<Point>(model);
			point.TransactionType = PointTransactionTypeEnums.Earning.ToString(); 

			await _unitOfWork.PointRepository.AddAsync(point);
			await _unitOfWork.SaveChangeAsync();

			return new SuccessResponseModel<object>
			{
				Success = true,
				Message = "Points earned successfully."
			};
		}
	}
}
