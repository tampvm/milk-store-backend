using MilkStore.Service.Models.ResponseModels;
using MilkStore.Service.Models.ViewModels.AccountViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore.Service.Interfaces
{
    public interface IAuthService
    {
        Task<ResponseModel> RegisterAsync(RegisterDTO model);
        Task<ResponseModel> SendRegisterVerificationCodeAsync(PhoneNumberDTO model);
        Task<ResponseModel> VerifyRegisterCodeAsync(VerifyPhoneNumberDTO model);
        Task<ResponseModel> LoginAsync(LoginDTO model);
        Task<ResponseModel> RefreshTokenAsync(RefreshTokenDTO model);
        Task<ResponseModel> SendForgotPasswordVerificationCodeByPhoneNumberAsync(PhoneNumberDTO model);
        Task<ResponseModel> VerifyForgotPasswordCodeByPhoneNumberAsync(VerifyPhoneNumberDTO model);
        Task<ResponseModel> ResetPasswordByPhoneNumberAsync(ResetPasswordByPhoneNumberDTO model);
    }
}
