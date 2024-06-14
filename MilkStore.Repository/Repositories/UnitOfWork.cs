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
        //private readonly IAcccountRepository _accountRepository;
        //private readonly IRoleRepository _roleRepository;

        public UnitOfWork(AppDbContext dbContext/*, IAcccountRepository accountRepository, IRoleRepository roleRepository*/)
        {
            _dbContext = dbContext;
            //_accountRepository = accountRepository;
            //_roleRepository = roleRepository;
        }

        //public IAcccountRepository AcccountRepository => _accountRepository;
        //public IRoleRepository RoleRepository => _roleRepository;

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
