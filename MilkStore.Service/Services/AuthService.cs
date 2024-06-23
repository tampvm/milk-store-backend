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
using MilkStore.Service.Models.ViewModels.AuthViewModels;
using MilkStore.Service.Utils;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore.Service.Services
{
    public class AuthService : BaseService, IAuthService
    {
        private readonly SignInManager<Account> _signInManager;
        private readonly UserManager<Account> _userManager;
        private readonly ITokenService _tokenService;
        private readonly IAccountService _accountService;
        private readonly IGoogleSerive _googleSerive;
        private readonly IFacebookService _facebookService;

        public AuthService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ICurrentTime currentTime,
            IClaimsService claimsService,
            AppConfiguration appConfiguration,
            SignInManager<Account> signInManager,
            UserManager<Account> userManager,
            ITokenService tokenService,
            ISmsSender smsSender,
            IEmailSender emailSender,
            IMemoryCache cache,
            IAccountService accountService,
            IGoogleSerive googleSerive,
            IFacebookService facebookService)
            : base(unitOfWork, mapper, currentTime, claimsService, appConfiguration, smsSender, emailSender, cache)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _tokenService = tokenService;
            _accountService = accountService;
            _googleSerive = googleSerive;
            _facebookService = facebookService;
        }

        #region Register
        // Generate verification code
        private string GenerateVerificationCode()
        {
            return new Random().Next(100000, 999999).ToString();
        }

        // Send verification code to user's phone number when user register
        public async Task<ResponseModel> SendRegisterVerificationCodeAsync(PhoneNumberDTO model)
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

            var code = GenerateVerificationCode();
            DateTime expiryTime = DateTime.UtcNow.ToLocalTime().AddMinutes(10); // Thời gian hết hạn 10 phút từ bây giờ

            await _smsSender.SendSmsAsync(model.PhoneNumber, $"Mã xác thực của bạn là: {code}, mã sẽ hết hiệu lực sau 10 phút.");

            _cache.Set(model.PhoneNumber, code, TimeSpan.FromMinutes(10));

            return new SuccessResponseModel<object>
            {
                Success = true,
                Message = "Verification code sent.",
                Data = new
                {
                    CodeExpiryTime = expiryTime
                }
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

            // Validate verification code
            if (_cache.TryGetValue(model.PhoneNumber, out string savedCode) && savedCode == model.Code)
            {
                // Remove the verification code from cache to prevent reuse
                _cache.Remove(model.PhoneNumber);

                // Generate register token
                var registerToken = _tokenService.GenerateToken(model.PhoneNumber, "phone", "register", out DateTime expiryTime); // Generate token by custom logic

                return new SuccessResponseModel<object>
                {
                    Success = true,
                    Message = "Phone number verified.",
                    Data = new
                    {
                        Token = registerToken,
                        TokenExpiryTime = expiryTime
                    }
                };
            }

            return new ResponseModel
            {
                Success = false,
                Message = "Invalid verification code."
            };
        }

        // Register
        public async Task<ResponseModel> RegisterAsync(RegisterDTO model)
        {
            if (!_tokenService.ValidateToken(model.PhoneNumber, "phone", "register", model.RegisterToken))
            {
                return new ResponseModel
                {
                    Success = false,
                    Message = "Invalid or expired registration token."
                };
            }

            var phoneExists = await _unitOfWork.AcccountRepository.FindByPhoneNumberAsync(model.PhoneNumber);
            if (phoneExists != null)
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
        #endregion

        #region Login
        // Login
        public async Task<ResponseModel> LoginAsync(LoginDTO model)
        {
            var user = await FindByUsernameOrEmailOrPhoneNumberAsync(model.Username);

            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, isPersistent: false, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    var accessToken = GenerateJsonWebToken(user, out DateTime tokenExpiryTime);
                    var refreshToken = GenerateRefreshToken();

                    user.RefreshToken = refreshToken;
                    user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_appConfiguration.JWT.RefreshTokenDurationInDays);
                    user.LastLogin = _currentTime.GetCurrentTime();

                    await _userManager.UpdateAsync(user);

                    return new AuthenticationResponseModel
                    {
                        Success = true,
                        Message = "Login success",
                        AccessToken = accessToken,
                        RefreshToken = refreshToken,
                        TokenExpiryTime = tokenExpiryTime
                    };
                }
                else
                {
                    return new AuthenticationResponseModel
                    {
                        Success = false,
                        Message = "Wrong password!",
                    };
                }
            }
            else
            {
                return new AuthenticationResponseModel
                {
                    Success = false,
                    Message = "User not found by username, email, or phone number",
                };
            }
        }

        // Find user by username, email, or phone number
        private async Task<Account> FindByUsernameOrEmailOrPhoneNumberAsync(string identifier, bool includeUsername = true)
        {
            // Try to find the user by phone number first
            var user = await _unitOfWork.AcccountRepository.FindByPhoneNumberAsync(identifier);

            // If not found by phone number, try to find by email
            if (user == null)
            {
                user = await _userManager.FindByEmailAsync(identifier);
            }

            // If not found by email and if username search is included, try to find by username
            if (user == null && includeUsername)
            {
                user = await _userManager.FindByNameAsync(identifier);
            }

            return user;
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
                //new Claim(JwtRegisteredClaimNames.Email, user.Email),
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

        // Generate refresh token
        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
        #endregion

        #region Refresh Token
        // Get principal from expired token
        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appConfiguration.JWT.JWTSecretKey)),
                ValidateLifetime = false // Không cần kiểm tra thời hạn
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }

            return principal;
        }

        // Refresh token
        public async Task<ResponseModel> RefreshTokenAsync(RefreshTokenDTO model)
        {
            try
            {
                var principal = GetPrincipalFromExpiredToken(model.AccessToken);
                var username = principal.Identity.Name;

                var user = await FindByUsernameOrEmailOrPhoneNumberAsync(username);

                if (user == null || user.RefreshToken != model.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
                {
                    return new AuthenticationResponseModel
                    {
                        Success = false,
                        Message = "Invalid refresh token"
                    };
                }

                // Kiểm tra nếu token vẫn còn trong thời hạn sử dụng
                var validToClaim = principal.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Exp)?.Value;
                if (validToClaim != null && DateTime.UtcNow < DateTime.UnixEpoch.AddSeconds(Convert.ToInt64(validToClaim)))
                {
                    return new AuthenticationResponseModel
                    {
                        Success = false,
                        Message = "Token is still valid. No need to refresh."
                    };
                }
                await _signInManager.RefreshSignInAsync(user);

                var newAccessToken = GenerateJsonWebToken(user, out DateTime tokenExpiryTime);
                var newRefreshToken = GenerateRefreshToken();

                user.RefreshToken = newRefreshToken;
                user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_appConfiguration.JWT.RefreshTokenDurationInDays);

                await _userManager.UpdateAsync(user);

                return new AuthenticationResponseModel
                {
                    Success = true,
                    Message = "Token refreshed successfully",
                    AccessToken = newAccessToken,
                    RefreshToken = newRefreshToken,
                    TokenExpiryTime = tokenExpiryTime
                };
            }
            catch (Exception)
            {
                return new AuthenticationResponseModel
                {
                    Success = false,
                    Message = "Failed to refresh token"
                };
            }
        }
        #endregion

        #region Forgot Password
        // Send verification code to user's phone number or email when user forgot password
        public async Task<ResponseModel> SendForgotPasswordVerificationCodeByPhoneNumberOrEmailAsync(SendForgotPasswordCodeDTO model)
        {
            //var user = await _userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == model.PhoneNumber);
            //var user = await _unitOfWork.AcccountRepository.FindByPhoneNumberAsync(model.PhoneNumber);
            var user = await FindByUsernameOrEmailOrPhoneNumberAsync(model.PhoneNumberOrEmail, includeUsername: false);

            if (user is null)
            {
                return new ResponseModel
                {
                    Success = false,
                    Message = "User not found."
                };
            }

            //var code = await _userManager.GenerateChangePhoneNumberTokenAsync(user, model.PhoneNumber);
            string code;
            DateTime expiryTime = DateTime.UtcNow.ToLocalTime().AddMinutes(10);

            if (model.PhoneNumberOrEmail.Contains("@"))
            {
                code = GenerateVerificationCode();
                await _emailSender.SendEmailAsync(user.Email, "Reset mật khẩu", $"Mã xác thực của bạn là: {code}, mã sẽ hết hiệu lực sau 10 phút.");
            }
            else
            {
                code = await _userManager.GenerateChangePhoneNumberTokenAsync(user, model.PhoneNumberOrEmail);
                await _smsSender.SendSmsAsync(model.PhoneNumberOrEmail, $"Mã xác thực của bạn là: {code}, mã sẽ hết hiệu lực sau 10 phút.");
            }

            // Cache code with a timeout (optional)
            _cache.Set(model.PhoneNumberOrEmail, code, TimeSpan.FromMinutes(10)); // Lưu mã xác thực vào bộ đệm với thời gian hết hạn 10 phút

            return new SuccessResponseModel<object>
            {
                Success = true,
                Message = "Verification code sent.",
                Data = new
                {
                    CodeExpiryTime =  expiryTime
                }
            };
        }

        // Verify forgot password code by phone number or email
        public async Task<ResponseModel> VerifyForgotPasswordCodeByPhoneNumberOrEmailAsync(VerifyForgotPasswordCodeDTO model)
        {
            //var user = await _userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == model.PhoneNumber);
            //var user = await _unitOfWork.AcccountRepository.FindByPhoneNumberAsync(model.PhoneNumberOrEmail);
            var user = await FindByUsernameOrEmailOrPhoneNumberAsync(model.PhoneNumberOrEmail, includeUsername: false);

            if (user is null)
            {
                return new ResponseModel
                {
                    Success = false,
                    Message = "User not found."
                };
            }

            // Retrieve the code from the cache
            if (_cache.TryGetValue(model.PhoneNumberOrEmail, out string? cachedCode) && cachedCode == model.Code)
            {
                bool isTokenValid;
                if (model.PhoneNumberOrEmail.Contains("@")) isTokenValid = true; // For email, assume token is valid if it matches the cached code
                else isTokenValid = await _userManager.VerifyChangePhoneNumberTokenAsync(user, model.Code, model.PhoneNumberOrEmail);

                if (!isTokenValid)
                {
                    return new ResponseModel
                    {
                        Success = false,
                        Message = "Invalid verification code."
                    };
                }

                // Code is verified, you can now redirect the user to the password reset page
                // Here, you might generate a token and send it to the client to allow password reset
                //var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user); // Generate token by ASP.NET Core Identity
                var resetToken = _tokenService.GenerateToken(model.PhoneNumberOrEmail, model.PhoneNumberOrEmail.Contains("@") ? "email" : "phone", "reset", out DateTime expiryTime); // Generate token by custom logic

                return new SuccessResponseModel<object>
                {
                    Success = true,
                    Message = "Verification successful. Proceed to password reset.",
                    Data = new
                    {
                        ResetToken = resetToken,
                        ResetTokenExpiryTime = expiryTime
                    }
                };
            }

            return new ResponseModel
            {
                Success = false,
                Message = "Invalid verification code."
            };
        }

        // Reset password by custom logic
        public async Task<ResponseModel> ResetPasswordAsync(ResetPasswordDTO model)
        {
            var user = await FindByUsernameOrEmailOrPhoneNumberAsync(model.PhoneNumberOrEmail, includeUsername: false);

            if (user is null)
            {
                return new ResponseModel
                {
                    Success = false,
                    Message = "User not found."
                };
            }

            string method = model.PhoneNumberOrEmail.Contains("@") ? "email" : "phone";
            if (!_tokenService.ValidateToken(model.PhoneNumberOrEmail, method, "reset", model.ResetToken))
            {
                return new ResponseModel
                {
                    Success = false,
                    Message = "Invalid or expired reset token."
                };
            }

            // Directly update the password since the token is valid
            user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, model.NewPassword);

            // Save changes to the database
            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return new ResponseModel
                {
                    Success = true,
                    Message = "Password has been reset successfully."
                };
            }

            return new ErrorResponseModel<List<string>>
            {
                Success = false,
                Message = "Failed to reset password.",
                Errors = result.Errors.Select(e => e.Description).ToList()
            };
        }

        // Reset password by ASP.NET Core Identity
        //public async Task<ResponseModel> ForgotPasswordAsync(ResetPasswordDTO model)
        //{
        //    //var user = await _userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == model.PhoneNumber);
        //    //var user = await _unitOfWork.AcccountRepository.FindByPhoneNumberAsync(model.PhoneNumberOrEmail);
        //    var user = await FindByUsernameOrEmailOrPhoneNumberAsync(model.PhoneNumberOrEmail, includeUsername: false);

        //    if (user is null)
        //    {
        //        return new ResponseModel
        //        {
        //            Success = false,
        //            Message = "User not found."
        //        };
        //    }

        //    var result = await _userManager.ResetPasswordAsync(user, model.ResetToken, model.NewPassword);
        //    if (result.Succeeded)
        //    {
        //        return new ResponseModel
        //        {
        //            Success = true,
        //            Message = "Password has been reset successfully."
        //        };
        //    }

        //    return new ErrorResponseModel<List<string>>
        //    {
        //        Success = false,
        //        Message = "Failed to reset password.",
        //        Errors = result.Errors.Select(e => e.Description).ToList()
        //    };
        //}
        #endregion

        #region Social Login
        // Google login
        public async Task<ResponseModel> GoogleLoginAsync(GoogleLoginDTO model)
        {
            GooglePayload payload;

            if (model.IsCredential)
            {
                payload = await _googleSerive.VerifyGoogleTokenAsync(model.GoogleToken);
            }
            else
            {
                var tokenResponse = await _googleSerive.ExchangeAuthCodeForTokensAsync(model.GoogleToken);
                if (tokenResponse == null)
                {
                    return new ResponseModel
                    {
                        Success = false,
                        Message = "Invalid Google auth code."
                    };
                }

                payload = await _googleSerive.VerifyGoogleTokenAsync(tokenResponse.IdToken);
            }

            if (payload == null)
            {
                return new ResponseModel
                {
                    Success = false,
                    Message = "Invalid Google token."
                };
            }

            var socialLoginDto = new SocialLoginDTO
            {
                Email = payload.Email,
                FirstName = payload.FirstName,
                LastName = payload.LastName,
                PictureUrl = payload.PictureUrl,
                ProviderId = payload.Sub,
                Provider = "Google"
            };

            return await ProcessSocialLoginAsync(socialLoginDto);
        }

        // Facebook login
        public async Task<ResponseModel> FacebookLoginAsync(FacebookLoginDTO model)
        {
            var userInfo = await _facebookService.GetUserInfoFromFacebookAsync(model.AccessToken);
            if (userInfo == null)
            {
                return new ResponseModel
                {
                    Success = false,
                    Message = "Invalid Facebook access token."
                };
            }

            var socialLoginDto = new SocialLoginDTO
            {
                Email = userInfo.Email,
                FirstName = userInfo.FirstName,
                LastName = userInfo.LastName,
                PictureUrl = userInfo.Picture.Data.Url,
                ProviderId = userInfo.Id,
                Provider = "Facebook"
            };

            return await ProcessSocialLoginAsync(socialLoginDto);
        }

        // Process social login
        private async Task<ResponseModel> ProcessSocialLoginAsync(SocialLoginDTO dto)
        {
            try
            {
                // Tìm hoặc tạo người dùng dựa trên GoogleEmail hoặc FacebookEmail
                var user = await _unitOfWork.AcccountRepository.FindByAnyCriteriaAsync(null, null, null,
                    dto.Provider == "Google" ? dto.Email : null,
                    dto.Provider == "Facebook" ? dto.Email : null
                );

                if (user == null)
                {
                    user = _mapper.Map<Account>(dto);

                    // Thiết lập các thuộc tính cho Google hoặc Facebook email
                    if (dto.Provider == "Google")
                    {
                        user.GoogleEmail = dto.Email;
                    }
                    else if (dto.Provider == "Facebook")
                    {
                        user.FacebookEmail = dto.Email;
                    }

                    user.CreatedAt = _currentTime.GetCurrentTime();
                    user.CreatedBy = user.Id;
                    user.LastLogin = _currentTime.GetCurrentTime();

                    var result = await _userManager.CreateAsync(user);

                    if (!result.Succeeded)
                    {
                        return new ResponseModel
                        {
                            Success = false,
                            Message = "Failed to create user."
                        };
                    }

                    await _userManager.AddLoginAsync(user, new UserLoginInfo(dto.Provider, dto.ProviderId, dto.Provider));
                    var role = RoleEnums.Customer.ToString(); // Ví dụ, mặc định đăng ký là Customer
                    await _userManager.AddToRoleAsync(user, role);

                    // Cập nhật avatar sau khi tạo người dùng
                    var updateUserAvatarDto = new UpdateUserAvatarDTO
                    {
                        GoogleEmail = dto.Provider == "Google" ? dto.Email : null,
                        FacebookEmail = dto.Provider == "Facebook" ? dto.Email : null,
                        AvatarUrl = dto.PictureUrl
                    };

                    // Cập nhật ảnh đại diện
                    await _accountService.UpdateUserAvatarAsync(updateUserAvatarDto);
                }
                else
                {
                    // Kiểm tra và thêm thông tin đăng nhập nếu chưa có
                    var logins = await _userManager.GetLoginsAsync(user);
                    if (logins.All(l => l.LoginProvider != dto.Provider))
                    {
                        await _userManager.AddLoginAsync(user, new UserLoginInfo(dto.Provider, dto.ProviderId, dto.Provider));
                    }
                }

                // Đăng nhập người dùng
                await _signInManager.SignInAsync(user, isPersistent: false);

                // Tạo JWT token hoặc phản hồi xác thực khác
                var accessToken = GenerateJsonWebToken(user, out DateTime tokenExpiryTime);
                var refreshToken = GenerateRefreshToken();

                //user.CreatedBy = user.Id;
                user.RefreshToken = refreshToken;
                user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_appConfiguration.JWT.RefreshTokenDurationInDays);

                await _userManager.UpdateAsync(user);

                return new AuthenticationResponseModel
                {
                    Success = true,
                    Message = "Login successful.",
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                    TokenExpiryTime = tokenExpiryTime
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new ResponseModel
                {
                    Success = false,
                    Message = "An error occurred while processing login."
                };
            }
        }
        #endregion

    }
}
