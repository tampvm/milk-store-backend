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
	public class FollowBrandRepository : GenericRepository<FollowBrand>, IFollowBrandRepository
	{
		private readonly AppDbContext _context;

		public FollowBrandRepository(AppDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
		{
			_context = context;
		}

		// Get all FollowBrand by BrandId
		public async Task<IEnumerable<FollowBrand>> GetFollowBrandByBrandIdAsync(int brandId, int pageIndex, int pageSize)
		{
			var followBrands = await GetAsync(
								filter: x => x.BrandId == brandId,
								pageIndex: pageIndex,
								pageSize: pageSize
								);

			return followBrands.Items;
		}

		// Get all FollowBrand by AccountId
		public async Task<IEnumerable<FollowBrand>> GetFollowBrandByAccountIdAsync(string accountId, int pageIndex, int pageSize)
		{
			var followBrands = await GetAsync(
								filter: x => x.AccountId == accountId,
								pageIndex: pageIndex,
								pageSize: pageSize
								);

			return followBrands.Items;
		}
	}
}
