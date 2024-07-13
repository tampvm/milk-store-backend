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
        private readonly IVoucherRepository _voucherRepository;
        private readonly IPointRepository _pointRepository;
        private readonly IBlogRepostiory _blogRepostiory;

		public UnitOfWork(AppDbContext dbContext, 
            IAcccountRepository accountRepository, 
            IRoleRepository roleRepository, 
            IImageRepository imageRepository,
            IBrandRepository brandRepository,
            IVoucherRepository voucherRepository,
            IPointRepository pointRepository,
            IBlogRepostiory blogRepostiory)
        {
            _dbContext = dbContext;
            _accountRepository = accountRepository;
            _roleRepository = roleRepository;
            _imageRepository = imageRepository;
            _brandRepository = brandRepository;
			_voucherRepository = voucherRepository;
			_pointRepository = pointRepository;
            _blogRepostiory = blogRepostiory;
		}

        public IAcccountRepository AcccountRepository => _accountRepository;
        public IRoleRepository RoleRepository => _roleRepository;
        public IImageRepository ImageRepository => _imageRepository;
        public IBlogRepostiory BlogRepostiory => _blogRepostiory;

        public IBrandRepository BrandRepository => _brandRepository;
        public IVoucherRepository VoucherRepository => _voucherRepository;
		public IPointRepository PointRepository => _pointRepository;

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
