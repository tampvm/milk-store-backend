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
    public class AccountRepository : GenericRepository<Account>, IAcccountRepository
    {
        private readonly AppDbContext _context;
        public AccountRepository(AppDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
        {
            _context = context;
        }

        public async Task<Account> FindByPhoneNumberAsync(string phoneNumber)
        {
            var user = await _context.Accounts.FirstOrDefaultAsync(record => record.PhoneNumber == phoneNumber);
            if (user is null)
            {
                return null;
            }
            return user;
        }

        //public Task<bool> CheckUserNameExited(string username) => _context.Accounts.AnyAsync(u => u.Username == username);

        //public async Task<Account> GetUserByUserNameAndPasswordHash(string username, string passwordHash)
        //{
        //    var user = await _context.Accounts
        //        .FirstOrDefaultAsync(record => record.Username == username
        //                                && record.Password == passwordHash);
        //    if (user is null)
        //    {
        //        throw new Exception("UserName & password is not correct");
        //    }


        //    return user;

        //}
    }
}
