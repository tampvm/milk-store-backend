using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text;
using System.Net.Http;

namespace MilkStore.WebApp.Pages
{
    public class LoginModel : PageModel
    {
        private readonly HttpClient _httpClient;

        [BindProperty]
        public string Username { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public LoginModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var response = await _httpClient.PostAsJsonAsync("https://localhost:44329/api/Auth/Login", new { Username = Username, Password = Password });

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadFromJsonAsync<LoginResponseModel>();

                // Lưu accessToken và refreshToken vào cookies hoặc local storage
                // Ví dụ:
                HttpContext.Response.Cookies.Append("accessToken", content.AccessToken);

                // Chuyển hướng đến trang Index hoặc Dashboard
                return RedirectToPage("/Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt");
                return Page();
            }
        }
    }

    public class LoginResponseModel
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime TokenExpiryTime { get; set; }
    }
}