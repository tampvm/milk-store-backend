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
    }
}
