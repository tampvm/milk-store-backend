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
	public class BrandRepository : GenericRepository<Brand>, IBrandRepository
	{
		private readonly AppDbContext _context;

		public BrandRepository(AppDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
		{
			_context = context;
		}

		public async Task<Brand> FindByNameAsync(string name)
		{
			var brand = _context.Brands.FirstOrDefault(b => b.Name == name);
			return brand;
		}
	}
}
