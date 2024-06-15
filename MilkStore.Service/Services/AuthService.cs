using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using MilkStore.Domain.Entities;
using MilkStore.Domain.Enums;
using MilkStore.Repository.Interfaces;
using MilkStore.Service.Common;
using MilkStore.Service.Interfaces;
using MilkStore.Service.Models.ResponseModels;
using MilkStore.Service.Models.ViewModels.AccountViewModels;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore.Service.Services
{
    public class AuthService : BaseService, IAuthService
    {
        private readonly SignInManager<Account> _signInManager;
        private readonly UserManager<Account> _userManager;

        public AuthService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ICurrentTime currentTime,
            IClaimsService claimsService,
            AppConfiguration appConfiguration,
            SignInManager<Account> signInManager,
            UserManager<Account> userManager,
            ISmsSender smsSender,
            IMemoryCache cache)
            : base(unitOfWork, mapper, currentTime, claimsService, appConfiguration, smsSender, cache)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        // Register
        public async Task<ResponseModel> RegisterAsync(RegisterDTO model)
        {
            var emailExists = await _unitOfWork.AcccountRepository.FindByPhoneNumberAsync(model.PhoneNumber);
            if (emailExists != null)
            {
                return new ResponseModel
                {
                    Success = false,
                    Message = $"{model.PhoneNumber} already exists"
                };
            }

            var userExists = await _userManager.FindByNameAsync(model.Username);
            if (userExists != null)
            {
                return new ResponseModel
                {
                    Success = false,
                    Message = $"{model.Username} already exists"
                };
            }

            // ko có trường email
            //var emailExists = await _userManager.FindByEmailAsync(model.Email);
            //if (emailExists != null)
            //{
            //    return new RegisterResponseModel
            //    {
            //        Success = false,
            //        Message = $"{model.Email} already exists"
            //    };
            //}

            var user = _mapper.Map<Account>(model);

            user.CreatedAt = _currentTime.GetCurrentTime();
            user.CreatedBy = user.Id;

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                var role = RoleEnums.Customer.ToString(); // Ví dụ, mặc định đăng ký là Customer
                await _userManager.AddToRoleAsync(user, role);

                //var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                //var confirmationLink = $"{_configuration["AppSettings:AppUrl"]}/api/account/confirmemail?userId={user.Id}&token={WebUtility.UrlEncode(token)}";

                //await _emailSender.SendEmailAsync(user.Email, "Confirm your email", $"Please confirm your email by clicking <a href=\"{confirmationLink}\">here</a>");

                return new SuccessResponseModel<object>
                {
                    Success = true,
                    Message = "Registration success.",
                    Data = new
                    {
                        UserId = user.Id,
                        FullName = user.FirstName + " " + user.LastName,
                        PhoneNumber = user.PhoneNumber,
                        Username = user.UserName,
                        Email = user.Email
                    }
                };
            }

            return new ResponseModel
            {
                Success = false,
                Message = string.Join("; ", result.Errors.Select(e => e.Description))
            };
        }

        // Send verification code to user's phone number when user register
        public async Task<ResponseModel> SendRegisterVerificationCodeAsync(RegisterPhoneNumberDTO model)
        {
            //var user = await _userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == model.PhoneNumber);
            var user = await _unitOfWork.AcccountRepository.FindByPhoneNumberAsync(model.PhoneNumber);

            if (user is not null)
            {
                return new ResponseModel
                {
                    Success = false,
                    Message = "Phone number already in use."
                };
            }

            var code = new Random().Next(100000, 999999).ToString();
            await _smsSender.SendSmsAsync(model.PhoneNumber, $"Mã xác thực của bạn là: {code}, mã sẽ hết hiệu lực sau 3 phút.");

            _cache.Set(model.PhoneNumber, code, TimeSpan.FromMinutes(3));

            return new ResponseModel
            {
                Success = true,
                Message = "Verification code sent."
            };
        }

        // Verify phone number by verification code when user register
        public async Task<ResponseModel> VerifyRegisterCodeAsync(VerifyPhoneNumberDTO model)
        {
            //var user = await _userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == model.PhoneNumber);
            var user = await _unitOfWork.AcccountRepository.FindByPhoneNumberAsync(model.PhoneNumber);

            if (user is not null)
            {
                return new ResponseModel
                {
                    Success = false,
                    Message = "Phone number already in use."
                };
            }

            if (_cache.TryGetValue(model.PhoneNumber, out string savedCode) && savedCode == model.Code)
            {
                return new SuccessResponseModel<string>
                {
                    Success = true,
                    Message = "Phone number verified.",
                    Data = model.PhoneNumber
                };
            }

            return new ResponseModel
            {
                Success = false,
                Message = "Invalid verification code."
            };
        }

        // Generate JWT
        private string GenerateJsonWebToken(Account user, out DateTime tokenExpiryTime)
        {
            var jwt = _appConfiguration.JWT;

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.JWTSecretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Name, user.UserName),
                new Claim(ClaimTypes.Name, user.UserName),
                //new Claim(ClaimTypes.Sid, user.Id.ToString()),
                new Claim ("userId", user.Id.ToString())
            };

            var roles = _userManager.GetRolesAsync(user).Result;

            // Thêm các claim về vai trò
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            tokenExpiryTime = _currentTime.GetCurrentTime().AddMinutes(jwt.AccessTokenDurationInMinutes);

            var token = new JwtSecurityToken(
                issuer: jwt.Issuer,
                audience: jwt.Audience,
                claims: claims,
                expires: tokenExpiryTime,
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
