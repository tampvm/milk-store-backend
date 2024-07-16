using Microsoft.EntityFrameworkCore;
using MilkStore.Domain.Entities;
using MilkStore.Domain.Enums;
using MilkStore.Repository.Data;
using MilkStore.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore.Repository.Repositories
{
	public class PointRepository : GenericRepository<Point>, IPointRepository
	{
		private readonly AppDbContext _context;

		public PointRepository(AppDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
		{
			_context = context;
		}

		// Get all points by account id
		public async Task<List<Point>> GetPointsByAccountIdAsync(string accountId, int pageIndex, int pageSize)
		{
			var points = _context.Points.Where(p => p.AccountId == accountId).Skip(pageIndex * pageSize).Take(pageSize).ToList();
			return points;
		}

		// Get total points by account id
		public async Task<int> GetTotalPointsByAccountIdAsync(string accountId)
		{
			var points = await _context.Points
							   .Where(p => p.AccountId == accountId)
							   .ToListAsync();

			int totalPoints = points.Sum(p => p.TransactionType == PointTransactionTypeEnums.Spending.ToString() ? -p.Points : p.Points);

			return totalPoints;
		}

		// Get all points by order id
		public async Task<List<Point>> GetPointsByOrderIdAsync(string orderId, int pageIndex, int pageSize)
		{
			var points = _context.Points.Where(p => p.OrderId == orderId).Skip(pageIndex * pageSize).Take(pageSize).ToList();
			return points;
		}
	}
}
