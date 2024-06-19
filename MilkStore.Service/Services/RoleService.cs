using AutoMapper;
using Microsoft.AspNetCore.Identity;
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
    }
}
