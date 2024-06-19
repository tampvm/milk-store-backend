using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MilkStore.Domain.Entities;
using MilkStore.Repository.Common;
using MilkStore.Repository.Interfaces;
using MilkStore.Service.Interfaces;
using MilkStore.Service.Models.ResponseModels;
using MilkStore.Service.Models.ViewModels.RoleViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore.Service.Services
{
    public class RoleService : IRoleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentTime _currentTime;
        private readonly IClaimsService _claimsService;
        private readonly UserManager<Account> _userManager;
        private readonly RoleManager<Role> _roleManager;

        public RoleService(IUnitOfWork unitOfWork, IMapper mapper, ICurrentTime currentTime, IClaimsService claimsService, UserManager<Account> userManager, RoleManager<Role> roleManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentTime = currentTime;
            _claimsService = claimsService;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        #region Role Management
        // Get all active roles
        public async Task<ResponseModel> GetActiveRolesAsync(int pageIndex, int pageSize)
        {
            var roles = await _unitOfWork.RoleRepository.GetAsync(
                filter: r => !r.IsDeleted,
                pageIndex: pageIndex,
                pageSize: pageSize
                );

            // Dùng AutoMapper để ánh xạ các thuộc tính của role sang DTO
            var roleDtos = _mapper.Map<Pagination<ViewListRoleDTO>>(roles);

            // Ánh xạ các thuộc tính đặc biệt cần xử lý
            foreach (var roleDto in roleDtos.Items)
            {
                var role = await _roleManager.FindByIdAsync(roleDto.Id);

                // Tìm người tạo, cập nhật, xóa và ánh xạ tên người dùng
                var createdByUser = await _userManager.FindByIdAsync(role.CreatedBy);
                var updatedByUser = !string.IsNullOrEmpty(role.UpdatedBy) ? await _userManager.FindByIdAsync(role.UpdatedBy) : null;
                var deletedByUser = !string.IsNullOrEmpty(role.DeletedBy) ? await _userManager.FindByIdAsync(role.DeletedBy) : null;

                // Cập nhật các trường CreatedBy, UpdatedBy, DeletedBy
                roleDto.CreatedBy = createdByUser?.UserName ?? role.CreatedBy;
                roleDto.UpdatedBy = updatedByUser?.UserName ?? role.UpdatedBy;
                roleDto.DeletedBy = deletedByUser?.UserName ?? role.DeletedBy;
            }

            return new SuccessResponseModel<object>
            {
                Success = true,
                Message = "Roles retrieved successfully.",
                Data = roleDtos
            };
        }

        // Create a new role
        public async Task<ResponseModel> CreateRoleAsync(CreateRoleDTO model)
        {
            // Tìm kiếm role theo tên, kể cả role đã bị xóa mềm
            var existingRole = await _roleManager.Roles
                .FirstOrDefaultAsync(r => r.Name == model.RoleName);

            if (existingRole != null)
            {
                if (existingRole.IsDeleted)
                {
                    // Role đã bị xóa mềm, cập nhật lại role này
                    existingRole.IsDeleted = false;
                    existingRole.DeletedAt = null;
                    existingRole.DeletedBy = null;
                    existingRole.Description = model.Description;
                    existingRole.UpdatedAt = _currentTime.GetCurrentTime();
                    existingRole.UpdatedBy = _claimsService.GetCurrentUserId().ToString();

                    var updateResult = await _roleManager.UpdateAsync(existingRole);

                    return new SuccessResponseModel<string>
                    {
                        Success = updateResult.Succeeded,
                        Message = updateResult.Succeeded ? "Role restored successfully." : "Failed to restore role.",
                        Data = updateResult.Succeeded ? existingRole.Id : null
                    };
                }
                else
                {
                    return new ResponseModel
                    {
                        Success = false,
                        Message = "Role already exists.",
                    };
                }
            }

            var createBy = _claimsService.GetCurrentUserId().ToString();
            var role = new Role(model.RoleName, model.Description, false, _currentTime.GetCurrentTime(), createBy, false);
            var result = await _roleManager.CreateAsync(role);

            return new SuccessResponseModel<string>
            {
                Success = result.Succeeded,
                Message = result.Succeeded ? "Role created successfully." : "Failed to create role.",
                Data = result.Succeeded ? role.Id : null
            };
        }

        // Update a role
        public async Task<ResponseModel> UpdateRoleAsync(UpdateRoleDTO model)
        {
            try
            {
                var role = await _roleManager.FindByIdAsync(model.Id);

                if (role is null)
                {
                    return new ResponseModel
                    {
                        Success = false,
                        Message = "Role not found.",
                    };
                }

                // Kiểm tra xem role đã bị xóa hay chưa
                if (role.IsDeleted)
                {
                    return new ResponseModel
                    {
                        Success = false,
                        Message = "Cannot update a deleted role.",
                    };
                }

                // Check if the new role name is already taken by another role
                if (await _roleManager.Roles.AnyAsync(r => r.Name == model.RoleName && r.Id != model.Id))
                {
                    return new ResponseModel { Success = false, Message = "Role name already exists." };
                }

                var updateRole = _mapper.Map(model, role);

                updateRole.UpdatedAt = _currentTime.GetCurrentTime();
                updateRole.UpdatedBy = _claimsService.GetCurrentUserId().ToString();

                var result = await _roleManager.UpdateAsync(updateRole);

                return new SuccessResponseModel<string>
                {
                    Success = result.Succeeded,
                    Message = result.Succeeded ? "Role updated successfully." : "Failed to update role.",
                    Data = result.Succeeded ? role.Id : null
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel
                {
                    Success = false,
                    Message = "An error occurred while updating the role. " + ex.Message
                };
            }
        }

        // Delete a role (soft delete)
        public async Task<ResponseModel> DeleteRoleAsync(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);

            if (role is null)
            {
                return new ResponseModel
                {
                    Success = false,
                    Message = "Role not found.",
                };
            }

            // Kiểm tra xem role có phải là role mặc định không
            if (role.IsDefault)
            {
                return new ResponseModel
                {
                    Success = false,
                    Message = "Cannot delete default role.",
                    //Data = null
                };
            }

            role.IsDeleted = true;
            role.DeletedAt = _currentTime.GetCurrentTime();
            role.DeletedBy = _claimsService.GetCurrentUserId().ToString();

            var result = await _roleManager.UpdateAsync(role);

            return new SuccessResponseModel<string>
            {
                Success = result.Succeeded,
                Message = result.Succeeded ? "Role deleted successfully." : "Failed to delete role.",
                Data = result.Succeeded ? role.Id : null
            };
        }
        #endregion
    }
}
