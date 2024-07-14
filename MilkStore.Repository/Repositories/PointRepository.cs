using MilkStore.Domain.Entities;
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
			var totalPoints = _context.Points.Where(p => p.AccountId == accountId).Sum(p => p.Points);
			return totalPoints;
		}
	}
}
