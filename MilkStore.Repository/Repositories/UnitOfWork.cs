using MilkStore.Repository.Data;
using MilkStore.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore.Repository.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _dbContext;
        private readonly IAcccountRepository _accountRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IImageRepository _imageRepository;
        private readonly IBrandRepository _brandRepository;
        private readonly IFollowBrandRepository _followBrandRepository;
		private readonly IVoucherRepository _voucherRepository;
        private readonly IPointRepository _pointRepository;
        private readonly IBlogRepostiory _blogRepostiory;
        private readonly IAddressRepository _addressRepository;
        private readonly IBlogCategoryRepository _blogCategoryRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IProductRepository _productRepository;
        private readonly IAgeRangeRepository _ageRangeRepository;
        private readonly IProductTypeRepository _productTypeRepository;
        private readonly IProductImageRepository _productImageRepository;
        private readonly IBlogImageRepository _blogImageRepository;
        private readonly ILikeRepository _likeRepository;
        private readonly ICommentBlogRepository _commentBlogRepository;
        private readonly ICartRepository _cartRepository;


        public UnitOfWork(AppDbContext dbContext, 
            IAcccountRepository accountRepository, 
            IRoleRepository roleRepository, 
            IImageRepository imageRepository,
            IBrandRepository brandRepository,
			IFollowBrandRepository followBrandRepository,
			IVoucherRepository voucherRepository,
            IPointRepository pointRepository,
            IBlogRepostiory blogRepostiory,
            IAddressRepository addressRepository,
            IBlogCategoryRepository blogCategoryRepository, 
            ICategoryRepository categoryRepository,
            IProductRepository productRepository,
            IAgeRangeRepository ageRangeRepository,
            IProductTypeRepository productTypeRepository,
            IProductImageRepository productImageRepository,
            IBlogImageRepository blogImageRepository,
            ILikeRepository likeRepository,
            ICommentBlogRepository commentBlogRepository,
            ICartRepository cartRepository)
            )

        {
            _dbContext = dbContext;
            _accountRepository = accountRepository;
            _roleRepository = roleRepository;
            _imageRepository = imageRepository;
            _brandRepository = brandRepository;
			_followBrandRepository = followBrandRepository;
			_voucherRepository = voucherRepository;
			_pointRepository = pointRepository;
            _blogRepostiory = blogRepostiory;
            _addressRepository = addressRepository;
            _blogCategoryRepository = blogCategoryRepository;
            _categoryRepository = categoryRepository;
            _productRepository = productRepository;
            _ageRangeRepository = ageRangeRepository;
            _productTypeRepository = productTypeRepository;
            _productImageRepository = productImageRepository;
            _blogImageRepository = blogImageRepository;
            _likeRepository = likeRepository;
            _commentBlogRepository = commentBlogRepository;
            _cartRepository = cartRepository;
        }

        public IAcccountRepository AcccountRepository => _accountRepository;
        public IRoleRepository RoleRepository => _roleRepository;
        public IImageRepository ImageRepository => _imageRepository;
        public IBlogRepostiory BlogRepostiory => _blogRepostiory;
        public IBrandRepository BrandRepository => _brandRepository;
		public IFollowBrandRepository FollowBrandRepository => _followBrandRepository;
		public IVoucherRepository VoucherRepository => _voucherRepository;
		public IPointRepository PointRepository => _pointRepository;
        public IAddressRepository AddressRepository => _addressRepository;
        public IBlogCategoryRepository BlogCategoryRepository => _blogCategoryRepository;
        public ICategoryRepository CategoryRepository => _categoryRepository;
        public IProductRepository ProductRepository => _productRepository;
        public IAgeRangeRepository AgeRangeRepository => _ageRangeRepository;
        public IProductTypeRepository ProductTypeRepository => _productTypeRepository;
        public IProductImageRepository ProductImageRepository => _productImageRepository;
        public IBlogImageRepository BlogImageRepository => _blogImageRepository;
        public ILikeRepository LikeRepository => _likeRepository;
        public ICommentBlogRepository CommentBlogRepository => _commentBlogRepository;
        public ICartRepository CartRepository => _cartRepository;

        public async Task<int> SaveChangeAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
