using MilkStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore.Repository.Interfaces
{
    public interface IImageRepository : IGenericRepository<Image>
    {
        Task<Image> FindByImageUrlAsync(string imageUrl);

        Task<Image> FindByImageIdAsync(int imageId);
        Task UpdateImageAsync(Image image);
    }
}
