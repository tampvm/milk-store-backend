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

		// Check if user follows brand
		public async Task<bool> CheckUserFollowsBrandAsync(string accountId, int brandId)
		{
			var followBrand = await GetAsync(
								filter: x => x.AccountId == accountId && x.BrandId == brandId
								);

			return followBrand.Items.Any();
		}
	}
}
