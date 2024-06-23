using MilkStore.Service.Models.ResponseModels;
using MilkStore.Service.Models.ViewModels.AccountViewModels;
using MilkStore.Service.Models.ViewModels.AuthViewModels;
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
        Task<ResponseModel> SendForgotPasswordVerificationCodeByPhoneNumberOrEmailAsync(SendForgotPasswordCodeDTO model);
        Task<ResponseModel> VerifyForgotPasswordCodeByPhoneNumberOrEmailAsync(VerifyForgotPasswordCodeDTO model);
        Task<ResponseModel> ResetPasswordAsync(ResetPasswordDTO model); // Reset Password by custom logic
        Task<ResponseModel> GoogleLoginAsync(GoogleLoginDTO model);
        Task<ResponseModel> FacebookLoginAsync(FacebookLoginDTO model);
    }
}
