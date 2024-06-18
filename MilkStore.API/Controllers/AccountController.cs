using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MilkStore.Service.Interfaces;
using MilkStore.Service.Models.ViewModels.AccountViewModels;

namespace MilkStore.API.Controllers
{
    public class AccountController : BaseController
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        #region Update User Phone Number
        [HttpPost]
        public async Task<IActionResult> SendVerificationCode(NewPhoneNumberDTO model)
        {
            var response = await _accountService.SendVerificationCodeAsync(model);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPost]
        public async Task<IActionResult> VerifyNewPhoneNumber(ChangePhoneNumberDTO model)
        {
            var response = await _accountService.VerifyNewPhoneNumberAsync(model);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
        #endregion

        #region Get All Users For Admin
        [HttpGet]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllUsersForAdminAsync(int pageInndex = 0, int pageSize = 10)
        {
            var response = await _accountService.GetAllUsersForAdminAsync(pageInndex, pageSize);
            if (response != null)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
        #endregion

        #region Update User Roles For Admin
        [HttpPost]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddRoleToUserAsync(UpdateUserRolesDTO model)
        {
            var response = await _accountService.AddRoleToUserAsync(model);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPost]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> RemoveRoleToUserAsync(UpdateUserRolesDTO model)
        {
            var response = await _accountService.RemoveRoleToUserAsync(model);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUserRolesAsync(string userId)
        {
            try
            {
                var response = await _accountService.GetUserRolesAsync(userId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetNotAssignedUserRolesAsync(string userId)
        {
            try
            {
                var response = await _accountService.GetNotAssignedUserRolesAsync(userId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
        #endregion
    }
}
