﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MilkStore.Service.Interfaces;
using MilkStore.Service.Models.ResponseModels;
using MilkStore.Service.Models.ViewModels.AccountViewModels;
using MilkStore.Service.Models.ViewModels.AuthViewModels;

namespace MilkStore.API.Controllers
{
    public class AuthController : BaseController
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        #region Register
        [HttpPost]
        public async Task<IActionResult> SendRegisterVerificationCodeAsync(PhoneNumberOrEmailDTO model)
        {
            var response = await _authService.SendRegisterVerificationCodeAsync(model);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPost]
        public async Task<IActionResult> VerifyRegisterCodeAsync(VerifyRegisterCodeDTO model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.ToDictionary(
                    key => key.Key,
                    value => string.Join("; ", value.Value.Errors.Select(e => e.ErrorMessage)));

                return BadRequest(new ErrorResponseModel<Dictionary<string, string>>
                {
                    Success = false,
                    Message = "Invalid request",
                    Errors = errors.Values.ToList()
                });
            }

            var response = await _authService.VerifyRegisterCodeAsync(model);
            if (response.Success)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        [HttpPost]
        public async Task<IActionResult> RegisterAsync(RegisterDTO registerDTO)
        {
            if (ModelState.IsValid)
            {
                var response = await _authService.RegisterAsync(registerDTO);

                if (response.Success)
                {
                    return Ok(response);
                }

                return BadRequest(response);
            }

            var errors = ModelState.ToDictionary(
                key => key.Key,
                value => string.Join("; ", value.Value.Errors.Select(e => e.ErrorMessage)));

            return BadRequest(new ErrorResponseModel<Dictionary<string, string>>
            {
                Success = false,
                Message = "Invalid request",
                Errors = errors.Values.ToList()
            });
        }
        #endregion

        #region Login
        [HttpPost]
        public async Task<IActionResult> LoginAsync(LoginDTO model)
        {
            if (ModelState.IsValid)
            {
                var respone = await _authService.LoginAsync(model);

                if (respone.Success)
                {
                    return Ok(respone);
                }

                return Unauthorized(respone);
            }

            var errors = ModelState.ToDictionary(
                key => key.Key,
                value => string.Join("; ", value.Value.Errors.Select(e => e.ErrorMessage)));

            return BadRequest(new ErrorResponseModel<Dictionary<string, string>>
            {
                Success = false,
                Message = "Invalid request",
                Errors = errors.Values.ToList()
            });
        }
        #endregion

        #region Refresh Token
        [HttpPost]
        public async Task<IActionResult> RefreshTokenAsync(RefreshTokenDTO model)
        {
            var response = await _authService.RefreshTokenAsync(model);

            if (response.Success)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }
        #endregion

        #region Forgot Password
        [HttpPost]
        public async Task<IActionResult> SendForgotPasswordVerificationCodeAsync(SendForgotPasswordCodeDTO model)
        {
            var response = await _authService.SendForgotPasswordVerificationCodeByPhoneNumberOrEmailAsync(model);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPost]
        public async Task<IActionResult> VerifyForgotPasswordCodeAsync(VerifyForgotPasswordCodeDTO model)
        {
            var response = await _authService.VerifyForgotPasswordCodeByPhoneNumberOrEmailAsync(model);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPasswordAsync(ResetPasswordDTO model)
        {
            var response = await _authService.ResetPasswordAsync(model);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
        #endregion

        #region Scocial Login
        [HttpPost]
        public async Task<IActionResult> GoogleLoginAsync(GoogleLoginDTO model)
        {
            var response = await _authService.GoogleLoginAsync(model);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPost]
        public async Task<IActionResult> FacebookLoginAsync(FacebookLoginDTO model)
        {
            var response = await _authService.FacebookLoginAsync(model);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
        #endregion
    }
}
