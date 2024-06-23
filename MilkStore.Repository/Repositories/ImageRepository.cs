using Microsoft.EntityFrameworkCore;
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
    public class ImageRepository : GenericRepository<Image>, IImageRepository
    {
        private readonly AppDbContext _context;

        public ImageRepository(AppDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
        {
            _context = context;
        }

        public async Task<Image> FindByImageUrlAsync(string imageUrl)
        {
            return await _context.Images.FirstOrDefaultAsync(x => x.ImageUrl == imageUrl);
        }

    }
}
