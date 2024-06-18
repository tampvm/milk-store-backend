using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using MilkStore.Domain.Entities;
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
using static MilkStore.Service.Models.ViewModels.AccountViewModels.UserRolesDTO;

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

        #region Update User Phone Number
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
    }
}
