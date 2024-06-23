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

        public async Task<Account> GetByGoogleEmailAsync(string googleEmail)
        {
            return await _context.Accounts.FirstOrDefaultAsync(u => u.GoogleEmail == googleEmail);
        }

        public async Task<Account> GetByFacebookEmailAsync(string facebookEmail)
        {
            return await _context.Accounts.FirstOrDefaultAsync(u => u.FacebookEmail == facebookEmail);
        }

        public async Task<Account> FindByAnyCriteriaAsync(string email, string phoneNumber, string userName, string googleEmail, string facebookEmail)
        {
            return await _context.Accounts.FirstOrDefaultAsync(a =>
                (!string.IsNullOrEmpty(email) && a.Email == email) ||
                (!string.IsNullOrEmpty(phoneNumber) && a.PhoneNumber == phoneNumber) ||
                (!string.IsNullOrEmpty(userName) && a.UserName == userName) ||
                (!string.IsNullOrEmpty(googleEmail) && a.GoogleEmail == googleEmail) ||
                (!string.IsNullOrEmpty(facebookEmail) && a.FacebookEmail == facebookEmail)
            );
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
