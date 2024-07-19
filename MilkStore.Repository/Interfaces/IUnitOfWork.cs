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
		IFollowBrandRepository FollowBrandRepository { get; }
		IVoucherRepository VoucherRepository { get; }
		IPointRepository PointRepository { get; }
		IBlogRepostiory BlogRepostiory { get; }
        IAddressRepository AddressRepository { get; }
		IBlogCategoryRepository BlogCategoryRepository { get; }
		ICategoryRepository CategoryRepository { get; }

		IProductRepository ProductRepository { get; }
		IAgeRangeRepository AgeRangeRepository { get; }
		IProductTypeRepository ProductTypeRepository { get; }
		IProductImageRepository ProductImageRepository { get; }

		IBlogImageRepository BlogImageRepository { get; }
		ILikeRepository LikeRepository { get; }
		ICommentBlogRepository CommentBlogRepository { get; }
		IOrderRepository OrderRepository { get; }
		ICartRepository CartRepository { get; }
		IOrderDetailRepository OrderDetailRepository { get;}

        Task<int> SaveChangeAsync();
	}
}
