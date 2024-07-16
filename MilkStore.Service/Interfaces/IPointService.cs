using MilkStore.Service.Models.ResponseModels;
using MilkStore.Service.Models.ViewModels.PointViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore.Service.Interfaces
{
	public interface IPointService
	{
		Task<ResponseModel> GetPointsByAccountIdAsync(string accountId, int pageIndex, int pageSize);
		Task<ResponseModel> GetPointsByOrderIdAsync(string orderId, int pageIndex, int pageSize);
		Task<ResponseModel> GetTotalPointsByAccountIdAsync(string accountId);
		Task<ResponseModel> SpendingPointsAsync(PointsTradingDTO model);
		Task<ResponseModel> EarningPointsAsync(PointsTradingDTO model);
	}
}
