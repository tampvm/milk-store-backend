using MilkStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore.Repository.Interfaces
{
    public interface IAcccountRepository : IGenericRepository<Account>
    {
        //Task<Account> GetUserByUserNameAndPasswordHash(string username, string passwordHash);

        //Task<bool> CheckUserNameExited(string username);

        Task<Account> FindByPhoneNumberAsync(string phoneNumber);
        Task<Account> GetByGoogleEmailAsync(string googleEmail);
        Task<Account> GetByFacebookEmailAsync(string facebookEmail);
        Task<Account> FindByAnyCriteriaAsync(string email, string phoneNumber, string userName, string googleEmail, string facebookEmail);
    }
}
