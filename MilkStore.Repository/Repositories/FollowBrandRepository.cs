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
			var followBrand = _context.FollowBrands.Where(x => x.AccountId == accountId && x.BrandId == brandId).ToList();

			return followBrand.Any();
		}

		// Get UserFollowsBrandDTO by AccountId and BrandId
		public async Task<FollowBrand> GetFollowBrandByUserAsync(string accountId, int brandId)
		{
			var followBrand = _context.FollowBrands.Where(x => x.AccountId == accountId && x.BrandId == brandId).FirstOrDefault();

			if (followBrand == null)
			{
				return null;
			}

			return followBrand;
		}
	}
}
