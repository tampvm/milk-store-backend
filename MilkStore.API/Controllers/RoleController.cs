using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MilkStore.Service.Interfaces;
using MilkStore.Service.Models.ViewModels.RoleViewModels;

namespace MilkStore.API.Controllers
{
    public class RoleController : BaseController
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        #region Role Management
        [HttpGet]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetActiveRolesAsync(int pageIndex = 0, int pageSize = 10)
        {
            var roles = await _roleService.GetActiveRolesAsync(pageIndex, pageSize);
            return Ok(roles);
        }

        [HttpPost]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateRoleAsync(CreateRoleDTO model)
        {
            if (!ModelState.IsValid)
            {
                // Xử lý lỗi nếu dữ liệu không hợp lệ
                return BadRequest(ModelState);
            }

            var response = await _roleService.CreateRoleAsync(model);

            if (response.Success)
            {
                // Vai trò đã được tạo thành công
                return Ok(response);
            }
            else
            {
                // Xử lý lỗi nếu việc tạo vai trò không thành công
                return BadRequest(response);
            }
        }

        [HttpPut]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateRoleAsync(UpdateRoleDTO model)
        {
            if (!ModelState.IsValid)
            {
                // Xử lý lỗi nếu dữ liệu không hợp lệ
                return BadRequest(ModelState);
            }

            var response = await _roleService.UpdateRoleAsync(model);

            if (response.Success)
            {
                // Vai trò đã được cập nhật thành công
                return Ok(response);
            }
            else
            {
                // Xử lý lỗi nếu việc cập nhật vai trò không thành công
                return BadRequest(response);
            }
        }

        [HttpDelete]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteRoleAsync(string roleId)
        {
            var response = await _roleService.DeleteRoleAsync(roleId);

            if (response.Success)
            {
                // Vai trò đã được xóa thành công
                return Ok(response);
            }
            else
            {
                // Xử lý lỗi nếu việc xóa vai trò không thành công
                return BadRequest(response);
            }
        }
        #endregion
    }
}
