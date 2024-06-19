using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MilkStore.Service.Interfaces;

namespace MilkStore.API.Controllers
{
    public class RoleController : BaseController
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetActiveRolesAsync(int pageIndex = 0, int pageSize = 10)
        {
            var roles = await _roleService.GetActiveRolesAsync(pageIndex, pageSize);
            return Ok(roles);
        }
    }
}
