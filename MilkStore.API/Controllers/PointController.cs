using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MilkStore.Service.Interfaces;

namespace MilkStore.API.Controllers
{
	public class PointController : BaseController
	{
		private readonly IPointService _pointService;

		public PointController(IPointService pointService)
		{
			_pointService = pointService;
		}

		// Get all points by account id
		[HttpGet]
		public async Task<IActionResult> GetPointsByAccountIdAsync(string accountId, int pageIndex = 0, int pageSize = 10)
		{
			var points = await _pointService.GetPointsByAccountIdAsync(accountId, pageIndex, pageSize);
			return Ok(points);
		}

		// Get all points by order id
		[HttpGet]
		public async Task<IActionResult> GetPointsByOrderIdAsync(string orderId, int pageIndex = 0, int pageSize = 10)
		{
			var points = await _pointService.GetPointsByOrderIdAsync(orderId, pageIndex, pageSize);
			return Ok(points);
		}

		// Get total points by account id
		[HttpGet]
		public async Task<IActionResult> GetTotalPointsByAccountIdAsync(string accountId)
		{
			var totalPoints = await _pointService.GetTotalPointsByAccountIdAsync(accountId);
			return Ok(totalPoints);
		}
	}
}
