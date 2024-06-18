using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using MilkStore.Domain.Entities;
using MilkStore.Repository.Interfaces;
using MilkStore.Service.Common;
using MilkStore.Service.Interfaces;
using MilkStore.Service.Models.ResponseModels;
using MilkStore.Service.Models.ViewModels.AccountViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore.Service.Services
{
    public class AccountService : BaseService, IAccountService
    {
        private readonly UserManager<Account> _userManager;
        private readonly RoleManager<Role> _roleManager;

        public AccountService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ICurrentTime currentTime,
            IClaimsService claimsService,
            AppConfiguration appConfiguration,
            UserManager<Account> userManager,
            RoleManager<Role> roleManager,
            ISmsSender smsSender,
            IEmailSender emailSender,
            IMemoryCache cache)
            : base(unitOfWork, mapper, currentTime, claimsService, appConfiguration, smsSender, emailSender, cache)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // Send verification code to the new phone number when user wants to change phone number
        public async Task<ResponseModel> SendVerificationCodeAsync(NewPhoneNumberDTO model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user == null)
            {
                return new ResponseModel
                {
                    Success = false,
                    Message = "User not found."
                };
            }

            // Generate and send the verification code to the new phone number
            var code = await _userManager.GenerateChangePhoneNumberTokenAsync(user, model.NewPhoneNumber);
            await _smsSender.SendSmsAsync(model.NewPhoneNumber, $"Mã xác thực của bạn là: {code}, mã sẽ hết hiệu lực sau 10 phút.");

            // Cache code with a timeout (optional)
            _cache.Set(model.NewPhoneNumber, code, TimeSpan.FromMinutes(10)); // Save the code in the cache for 10 minutes
            DateTime expiryTime = DateTime.UtcNow.ToLocalTime().AddMinutes(10); // Thời gian hết hạn 10 phút từ bây giờ

            return new SuccessResponseModel<object>
            {
                Success = true,
                Message = "Verification code sent to new phone number.",
                Data = new
                {
                    CodeExpiryTime = expiryTime
                }
            };
        }

        // Verify the new phone number with the verification code
        public async Task<ResponseModel> VerifyNewPhoneNumberAsync(ChangePhoneNumberDTO model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user == null)
            {
                return new ResponseModel
                {
                    Success = false,
                    Message = "User not found."
                };
            }

            // Retrieve the code from the cache
            if (_cache.TryGetValue(model.PhoneNumber, out string? cachedCode) && cachedCode == model.Code)
            {
                // Verify the verification code
                var isTokenValid = await _userManager.VerifyChangePhoneNumberTokenAsync(user, model.Code, model.PhoneNumber);
                if (!isTokenValid)
                {
                    return new ResponseModel
                    {
                        Success = false,
                        Message = "Invalid verification code."
                    };
                }

                // Update the phone number
                var result = await _userManager.ChangePhoneNumberAsync(user, model.PhoneNumber, model.Code);
                if (!result.Succeeded)
                {
                    return new ErrorResponseModel<string>
                    {
                        Success = false,
                        Message = "Failed to update phone number.",
                        Errors = result.Errors.Select(e => e.Description).ToList()
                    };
                }

                return new SuccessResponseModel<object>
                {
                    Success = true,
                    Message = "Phone number updated successfully.",
                    Data = new
                    {
                        UserId = user.Id,
                        NewPhoneNumber = user.PhoneNumber
                    }
                };
            }

            return new ResponseModel
            {
                Success = false,
                Message = "Invalid verification code."
            };
        }
    }
}
