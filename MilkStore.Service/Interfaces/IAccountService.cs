using MilkStore.Service.Models.ResponseModels;
using MilkStore.Service.Models.ViewModels.AccountViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore.Service.Interfaces
{
    public interface IAccountService
    {
        Task<ResponseModel> SendVerificationCodeAsync(NewPhoneNumberDTO model);
        Task<ResponseModel> VerifyNewPhoneNumberAsync(ChangePhoneNumberDTO model);
        Task<ResponseModel> GetAllUsersForAdminAsync(int pageIndex, int pageSize);
    }
}
