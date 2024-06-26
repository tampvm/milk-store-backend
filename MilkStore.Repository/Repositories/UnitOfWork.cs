﻿using MilkStore.Repository.Data;
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

        public UnitOfWork(AppDbContext dbContext, IAcccountRepository accountRepository, IRoleRepository roleRepository, IImageRepository imageRepository)
        {
            _dbContext = dbContext;
            _accountRepository = accountRepository;
            _roleRepository = roleRepository;
            _imageRepository = imageRepository;
        }

        public IAcccountRepository AcccountRepository => _accountRepository;
        public IRoleRepository RoleRepository => _roleRepository;
        public IImageRepository ImageRepository => _imageRepository;

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
