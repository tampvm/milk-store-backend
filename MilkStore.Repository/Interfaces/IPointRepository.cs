using System;
using MilkStore.Domain.Entities;
using MilkStore.Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore.Repository.Interfaces
{
	public interface IPointRepository : IGenericRepository<Point>
	{
		Task<List<Point>> GetPointsByAccountIdAsync(string accountId, int pageIndex, int pageSize);
		Task<int> GetTotalPointsByAccountIdAsync(string accountId);
		Task<List<Point>> GetPointsByOrderIdAsync(string orderId, int pageIndex, int pageSize);
	}
}
