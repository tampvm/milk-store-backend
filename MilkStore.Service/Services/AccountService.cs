using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using MilkStore.Domain.Entities;
using MilkStore.Domain.Enums;
using MilkStore.Repository.Common;
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
using static MilkStore.Service.Models.ViewModels.AccountViewModels.ViewUserRolesDTO;

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

        #region Update User Phone Number Or Link Phone Number
        // Send verification code to the new phone number when user wants to change phone number
        public async Task<ResponseModel> SendVerificationCodeAsync(NewPhoneNumberDTO model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                return new ResponseModel
                {
                    Success = false,
                    Message = "User not found."
                };
            }

            // Check if the new phone number is already in use
            var existingUser = await _unitOfWork.AcccountRepository.FindByPhoneNumberAsync(model.NewPhoneNumber);
            if (existingUser != null && existingUser.Id != user.Id)
            {
                return new ResponseModel
                {
                    Success = false,
                    Message = "Phone number is already in use."
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
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                return new ResponseModel
                {
                    Success = false,
                    Message = "User not found."
                };
            }

            // Retrieve the code from the cache
            if (_cache.TryGetValue(model.NewPhoneNumber, out string? cachedCode) && cachedCode == model.Code)
            {
                // Verify the verification code
                var isTokenValid = await _userManager.VerifyChangePhoneNumberTokenAsync(user, model.Code, model.NewPhoneNumber);
                if (!isTokenValid)
                {
                    return new ResponseModel
                    {
                        Success = false,
                        Message = "Invalid verification code."
                    };
                }

                // Update the phone number
                var result = await _userManager.ChangePhoneNumberAsync(user, model.NewPhoneNumber, model.Code);
                if (!result.Succeeded)
                {
                    return new ErrorResponseModel<string>
                    {
                        Success = false,
                        Message = "Failed to update phone number.",
                        Errors = result.Errors.Select(e => e.Description).ToList()
                    };
                }

                user.UpdatedAt = _currentTime.GetCurrentTime();
                user.UpdatedBy = _claimsService.GetCurrentUserId().ToString();

                await _userManager.UpdateAsync(user);

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
        #endregion

        #region Get All Users For Admin
        // Get all users for admin
        public async Task<ResponseModel> GetAllUsersForAdminAsync(int pageIndex, int pageSize)
        {
            //var users = await _userManager.Users.ToListAsync(); // Lấy danh sách người dùng từ DB

            var users = await _unitOfWork.AcccountRepository.GetAsync(
                filter: u => !u.IsDeleted,
                orderBy: u => u.OrderBy(u => u.Id),
                pageIndex: pageIndex,
                pageSize: pageSize
            );

            var userDtos = _mapper.Map<Pagination<ViewListUserDTO>>(users); // Ánh xạ danh sách người dùng sang ViewListUserDTO

            // Lặp qua từng người dùng để lấy và ánh xạ vai trò
            foreach (var userDto in userDtos.Items)
            {
                var user = await _userManager.FindByIdAsync(userDto.Id); // Tìm người dùng theo Id để lấy vai trò
                var roles = await _userManager.GetRolesAsync(user); // Lấy danh sách vai trò của người dùng

                // Gán danh sách vai trò vào DTO
                userDto.Roles = roles;
                userDto.CreatedBy = user.CreatedBy == null ? null : (await _userManager.FindByIdAsync(user.CreatedBy))?.UserName ?? user.CreatedBy;
                userDto.UpdatedBy = string.IsNullOrEmpty(user.UpdatedBy) ? null : (await _userManager.FindByIdAsync(user.UpdatedBy))?.UserName ?? user.UpdatedBy;
                userDto.DeletedBy = string.IsNullOrEmpty(user.DeletedBy) ? null : (await _userManager.FindByIdAsync(user.DeletedBy))?.UserName ?? user.DeletedBy;
            }

            return new SuccessResponseModel<object>
            {
                Success = true,
                Message = "Users found.",
                Data = userDtos
            };
        }
        #endregion

        #region Update User Roles For Admin
        // Add role to user
        public async Task<ResponseModel> AddRoleToUserAsync(UpdateUserRolesDTO model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                return new ResponseModel
                {
                    Success = false,
                    Message = "User not found.",
                };
            }

            var roleExists = await _roleManager.RoleExistsAsync(model.RoleName);
            if (!roleExists)
            {
                return new ResponseModel
                {
                    Success = false,
                    Message = "Role does not exist.",
                };
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            if (userRoles.Contains(model.RoleName))
            {
                return new ResponseModel
                {
                    Success = false,
                    Message = "User already has this role.",
                };
            }

            var result = await _userManager.AddToRoleAsync(user, model.RoleName);

            if (result.Succeeded)
            {
                user.UpdatedAt = _currentTime.GetCurrentTime();
                user.UpdatedBy = _claimsService.GetCurrentUserId().ToString();

                await _userManager.UpdateAsync(user);

                return new SuccessResponseModel<object>
                {
                    Success = true,
                    Message = "Role added to user successfully.",
                    Data = new 
                    {
                        UserId = user.Id,
                        RoleName = model.RoleName
                    }
                };
            }
            else
            {
                return new ErrorResponseModel<string>
                {
                    Success = false,
                    Message = "Failed to add role to user.",
                    Errors = result.Errors.Select(e => e.Description).ToList()
                };
            }
        }

        // Remove role from user
        public async Task<ResponseModel> RemoveRoleToUserAsync(UpdateUserRolesDTO model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                return new ResponseModel
                {
                    Success = false,
                    Message = "User not found.",
                };
            }

            var roleExists = await _roleManager.RoleExistsAsync(model.RoleName);
            if (!roleExists)
            {
                return new ResponseModel
                {
                    Success = false,
                    Message = "Role does not exist.",
                };
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            if (!userRoles.Contains(model.RoleName))
            {
                return new ResponseModel
                {
                    Success = false,
                    Message = "User does not have this role.",
                };
            }

            // Check if the user has only one role
            if (userRoles.Count == 1)
            {
                return new ResponseModel
                {
                    Success = false,
                    Message = "User cannot be without any role.",
                };
            }

            var result = await _userManager.RemoveFromRoleAsync(user, model.RoleName);

            if (result.Succeeded)
            {
                user.UpdatedAt = _currentTime.GetCurrentTime();
                user.UpdatedBy = _claimsService.GetCurrentUserId().ToString();

                await _userManager.UpdateAsync(user);

                return new SuccessResponseModel<object>
                {
                    Success = true,
                    Message = "Role removed from user successfully.",
                    Data = new 
                    {
                        UserId = user.Id,
                        RoleName = model.RoleName
                    }
                };
            }
            else
            {
                return new ErrorResponseModel<string>
                {
                    Success = false,
                    Message = "Failed to remove role from user.",
                    Errors = result.Errors.Select(e => e.Description).ToList()
                };
            }
        }

        // Get a list of user roles
        public async Task<ResponseModel> GetUserRolesAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new Exception("User not found.");
            }

            var roles = await _userManager.GetRolesAsync(user);

            // Sử dụng AutoMapper để ánh xạ thông tin người dùng sang DTO
            var userRolesDTO = _mapper.Map<ViewUserRolesDTO>(user);

            // Thêm danh sách vai trò vào DTO
            userRolesDTO.RoleNames = roles.ToList();

            return new SuccessResponseModel<object>
            {
                Success = true,
                Message = "User roles found.",
                Data = userRolesDTO
            };
        }

        // Get the list of roles that have not been assigned to the user
        public async Task<ResponseModel> GetNotAssignedUserRolesAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new Exception("User not found.");
            }

            var allRoles = await _roleManager.Roles.ToListAsync();
            var userRoles = await _userManager.GetRolesAsync(user);
            var notAssignedRoles = allRoles.Where(role => !userRoles.Contains(role.Name)).Select(role => role.Name).ToList();

            return new SuccessResponseModel<object>
            {
                Success = true,
                Message = "Not assigned user roles found.",
                Data = notAssignedRoles
            };
        }
        #endregion

        public async Task<ResponseModel> UpdateUserAvatarAsync(UpdateUserAvatarDTO model)
        {
            try
            {
                // Tìm người dùng dựa trên các tiêu chí tìm kiếm khác nhau
                var user = await _unitOfWork.AcccountRepository.FindByAnyCriteriaAsync(
                    model.Email, model.PhoneNumber, model.UserName, model.GoogleEmail, model.FacebookEmail
                );

                if (user == null)
                {
                    return new ResponseModel { Success = false, Message = "User not found." };
                }

                // Tạo một bản ghi mới cho hình ảnh avatar
                var newAvatar = new Image
                {
                    ImageUrl = model.AvatarUrl,
                    ThumbnailUrl = model.AvatarUrl,
                    Type = ImageTypeEnums.Avatar.ToString(),
                    CreatedBy = user.Id
                };

                await _unitOfWork.ImageRepository.AddAsync(newAvatar);
                await _unitOfWork.SaveChangeAsync();

                // Cập nhật avatar cho user
                user.AvatarId = newAvatar.Id;
                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    return new ResponseModel { Success = true, Message = "Avatar updated successfully." };
                }
                else
                {
                    return new ResponseModel { Success = false, Message = "Failed to update user avatar." };
                }
            }
            catch (Exception ex)
            {
                return new ErrorResponseModel<string>
                {
                    Success = false,
                    Message = "An error occurred while updating avatar.",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        #region Get User Profile
        public async Task<ResponseModel> GetUserProfileAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new ResponseModel { Success = false, Message = "User not found." };
            }

            var roles = await _userManager.GetRolesAsync(user);
            var avatar = await _unitOfWork.ImageRepository.GetByIdAsync(user.AvatarId);
            var background = await _unitOfWork.ImageRepository.GetByIdAsync(user.BackgroundId);
            var address = await _unitOfWork.AddressRepository.GetDefaultAddressOrFirstAsync(userId);

            var userProfileDTO = _mapper.Map<ViewUserProfileDTO>(user);
            userProfileDTO.Roles = roles.ToList();
            userProfileDTO.Avatar = avatar?.ImageUrl;
            userProfileDTO.Background = background?.ImageUrl;
            if (address != null) userProfileDTO.Address = address.AddressLine + ", " + address.Ward + ", " + address.District + ", " + address.City;


            return new SuccessResponseModel<object>
            {
                Success = true,
                Message = "User profile found.",
                Data = userProfileDTO
            };
        }
        #endregion

        #region Update User Profile
        public async Task<ResponseModel> UpdateUserProfileAsync(UpdateUserProfileDTO model)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(model.UserId);
                if (user == null)
                {
                    return new ResponseModel { Success = false, Message = "User not found." };
                }

                // Sử dụng mapper để cập nhật thuộc tính của đối tượng user từ model
                _mapper.Map(model, user);

                user.UpdatedAt = _currentTime.GetCurrentTime();
                user.UpdatedBy = _claimsService.GetCurrentUserId().ToString();

                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    return new ResponseModel { Success = true, Message = "User profile updated successfully." };
                }
                else
                {
                    return new ErrorResponseModel<string>
                    {
                        Success = false,
                        Message = "Failed to update user profile.",
                        Errors = result.Errors.Select(e => e.Description).ToList()
                    };
                }
            }
            catch (Exception ex)
            {
                return new ErrorResponseModel<string>
                {
                    Success = false,
                    Message = "An error occurred while updating user profile.",
                    Errors = new List<string> { ex.Message }
                };
            }
        }
        #endregion

        // Generate verification code
        private string GenerateVerificationCode()
        {
            return new Random().Next(100000, 999999).ToString();
        }

        #region Update User Email Or Link Email
        // Send verification code to the new email when user wants to change email
        public async Task<ResponseModel> SendVerificationCodeEmailAsync(NewEmailDTO model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                return new ResponseModel
                {
                    Success = false,
                    Message = "User not found."
                };
            }

            // Check if the email is already in use
            var existingUser = await _userManager.FindByEmailAsync(model.NewEmail);
            if (existingUser != null)
            {
                return new ResponseModel
                {
                    Success = false,
                    Message = "Email is already in use."
                };
            }

            // Generate and send the verification code to the new email
            var code = GenerateVerificationCode();
            await _emailSender.SendEmailAsync(model.NewEmail, "Verification Code", $"Mã xác thực của bạn là: {code}, mã sẽ hết hiệu lực sau 10 phút.");

            // Cache code with a timeout (optional)
            _cache.Set(model.NewEmail, code, TimeSpan.FromMinutes(10)); // Save the code in the cache for 10 minutes
            DateTime expiryTime = DateTime.UtcNow.ToLocalTime().AddMinutes(10); // Thời gian hết hạn 10 phút từ bây giờ

            return new SuccessResponseModel<object>
            {
                Success = true,
                Message = "Verification code sent to new email.",
                Data = new
                {
                    CodeExpiryTime = expiryTime
                }
            };
        }

        // Verify the new email with the verification code
        public async Task<ResponseModel> VerifyNewEmailAsync(ChangeEmailDTO model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                return new ResponseModel
                {
                    Success = false,
                    Message = "User not found."
                };
            }

            // Retrieve the code from the cache
            if (_cache.TryGetValue(model.NewEmail, out string? cachedCode) && cachedCode == model.Code)
            {
                // Verify the verification code
                if (cachedCode != model.Code)
                {
                    return new ResponseModel
                    {
                        Success = false,
                        Message = "Invalid verification code."
                    };
                }

                // Update the email
                user.Email = model.NewEmail;
                user.EmailConfirmed = true;
                user.UpdatedAt = _currentTime.GetCurrentTime();
                user.UpdatedBy = _claimsService.GetCurrentUserId().ToString();
                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                {
                    return new ErrorResponseModel<string>
                    {
                        Success = false,
                        Message = "Failed to update email.",
                        Errors = result.Errors.Select(e => e.Description).ToList()
                    };
                }

                // Optionally, remove the code from the cache after successful verification
                _cache.Remove(model.NewEmail);

                return new SuccessResponseModel<object>
                {
                    Success = true,
                    Message = "Email updated successfully.",
                    Data = new
                    {
                        UserId = user.Id,
                        NewEmail = user.Email
                    }
                };
            }

            return new ResponseModel
            {
                Success = false,
                Message = "Invalid verification code."
            };
        }
        #endregion

        #region Link Account With Username
        public async Task<ResponseModel> LinkAccountWithUserNameAsync(UpdateUserAccountDTO model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                return new ResponseModel
                {
                    Success = false,
                    Message = "User not found."
                };
            }

            // Check if the username is provided and update if necessary
            if (!string.IsNullOrEmpty(model.UserName))
            {
                var existingUser = await _userManager.FindByNameAsync(model.UserName);
                if (existingUser != null && existingUser.Id != model.UserId)
                {
                    return new ResponseModel
                    {
                        Success = false,
                        Message = "Username is already taken."
                    };
                }
                user.UserName = model.UserName;
            }

            if (!string.IsNullOrEmpty(model.Password))
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var result = await _userManager.ResetPasswordAsync(user, token, model.Password);
                if (!result.Succeeded)
                {
                    return new ErrorResponseModel<string>
                    {
                        Success = false,
                        Message = "Failed to reset password.",
                        Errors = result.Errors.Select(e => e.Description).ToList()
                    };
                }
            }

            user.UpdatedAt = _currentTime.GetCurrentTime();
            user.UpdatedBy = _claimsService.GetCurrentUserId().ToString();

            var updateResult = await _userManager.UpdateAsync(user);

            if (!updateResult.Succeeded)
            {
                return new ResponseModel
                {
                    Success = false,
                    Message = "Failed to update user account."
                };
            }

            return new SuccessResponseModel<object>
            {
                Success = true,
                Message = "User account updated successfully."
            };
        }
        #endregion

        #region Change Password
        public async Task<ResponseModel> ChangePasswordAsync(ChangePasswordDTO model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                return new ResponseModel
                {
                    Success = false,
                    Message = "User not found."
                };
            }

            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            if (!result.Succeeded)
            {
                return new ErrorResponseModel<string>
                {
                    Success = false,
                    Message = "Failed to change password.",
                    Errors = result.Errors.Select(e => e.Description).ToList()
                };
            }

            user.UpdatedAt = _currentTime.GetCurrentTime();
            user.UpdatedBy = _claimsService.GetCurrentUserId().ToString();

            await _userManager.UpdateAsync(user);

            return new SuccessResponseModel<object>
            {
                Success = true,
                Message = "Password changed successfully."
            };
        }
        #endregion
    }
}
