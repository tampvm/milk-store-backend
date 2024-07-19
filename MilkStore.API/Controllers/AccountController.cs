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

        #region View User Profile
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUserProfileAsync(string userId)
        {
            var response = await _accountService.GetUserProfileAsync(userId);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
        #endregion

        #region Update User Profile
        [HttpPut]
        [Authorize]
        public async Task<IActionResult> UpdateUserProfileAsync(UpdateUserProfileDTO model)
        {
            var response = await _accountService.UpdateUserProfileAsync(model);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> UpdateUserAvatarAsync(UpdateUserAvatarDTO model)
        {
            var response = await _accountService.UpdateUserAvatarAsync(model);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
        #endregion

        #region Update User Phone Number Or Link Phone Number
        [HttpPost]
        [Authorize]
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
        [Authorize]
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

        #region Update User Email Or Link Email
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> SendVerificationCodeEmailAsync(NewEmailDTO model)
        {
            var response = await _accountService.SendVerificationCodeEmailAsync(model);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> VerifyNewEmailAsync(ChangeEmailDTO model)
        {
            var response = await _accountService.VerifyNewEmailAsync(model);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
        #endregion

        #region Link Account With UserName
        [HttpPut]
        [Authorize]
        public async Task<IActionResult> LinkAccountWithUserNameAsync(UpdateUserAccountDTO model)
        {
            var response = await _accountService.LinkAccountWithUserNameAsync(model);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
        #endregion

        #region Change Password
        [HttpPut]
        [Authorize]
        public async Task<IActionResult> ChangePasswordAsync(ChangePasswordDTO model)
        {
            var response = await _accountService.ChangePasswordAsync(model);
            if (response.Success)
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

        [HttpDelete]
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

        #region Manage User By Admin
        // Get All Users For Admin
        [HttpGet]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllUsersForAdminAsync(int pageIndex = 0, int pageSize = 10)
        {
            var response = await _accountService.GetAllUsersForAdminAsync(pageIndex, pageSize);
            if (response != null)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        // Add New User By Admin
        [HttpPost]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddNewUserByAdminAsync(CreateAccountDTO model)
        {
            //if (ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            var response = await _accountService.AddNewUserByAdminAsync(model);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        // Edit User By Admin
        [HttpPut]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditUserByAdminAsync(EditAccountDTO model)
        {
            var response = await _accountService.EditUserByAdminAsync(model);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        // Block Or Unblock User By Admin
        [HttpPut]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> BlockOrUnBlockUserByAdmin(BlockOrUnBlockAccountDTO model)
        {
            var response = await _accountService.BlockOrUnBlockUserByAdmin(model);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        // Search Users By Full Name Or Phone And Filter By Status
        [HttpGet]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> SearchUsersAsync(string? keyword, string? status, int pageIndex = 0, int pageSize = 10)
        {
            var response = await _accountService.SearchUsersAsync(keyword, status, pageIndex, pageSize);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
        #endregion
    }
}
