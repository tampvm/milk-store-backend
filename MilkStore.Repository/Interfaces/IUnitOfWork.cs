using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore.Repository.Interfaces
{
	public interface IUnitOfWork : IDisposable
	{
		IAcccountRepository AcccountRepository { get; }
		IRoleRepository RoleRepository { get; }
		IImageRepository ImageRepository { get; }
		IBrandRepository BrandRepository { get; }
		IVoucherRepository VoucherRepository { get; }
		IPointRepository PointRepository { get; }
		IBlogRepostiory BlogRepostiory { get; }
        IAddressRepository AddressRepository { get; }
		IBlogCategoryRepository BlogCategoryRepository { get; }
		ICategoryRepository CategoryRepository { get; }
		IBlogImageRepository BlogImageRepository { get; }
		
        Task<int> SaveChangeAsync();
	}
}
