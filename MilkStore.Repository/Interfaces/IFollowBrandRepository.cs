using MilkStore.Domain.Entities;
using MilkStore.Repository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore.Repository.Interfaces
{
	public interface IFollowBrandRepository : IGenericRepository<FollowBrand>
	{
		Task<IEnumerable<FollowBrand>> GetFollowBrandByBrandIdAsync(int brandId, int pageIndex, int pageSize);
		Task<List<FollowBrand>> GetFollowBrandByAccountIdAsync(string accountId, int pageIndex, int pageSize);
		Task<bool> CheckUserFollowsBrandAsync(string accountId, int brandId);
	}
}
