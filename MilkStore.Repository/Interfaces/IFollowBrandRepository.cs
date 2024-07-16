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
		Task<bool> CheckUserFollowsBrandAsync(string accountId, int brandId);
		Task<FollowBrand> GetFollowBrandByUserAsync(string accountId, int brandId);
	}
}
