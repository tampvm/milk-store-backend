using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MilkStore.Service.Models.ResponseModels;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace MilkStore.WebApp.Pages
{
    public class GoogleLoginModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public GoogleLoginModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public string ReturnUrl { get; set; }
        public string Message { get; set; }

        public IActionResult OnPost(string provider, string returnUrl = null)
        {
            var redirectUrl = Url.Page("./Login", pageHandler: "Callback", values: new { returnUrl });
            return Challenge(new AuthenticationProperties { RedirectUri = redirectUrl }, provider);
        }

        public async Task<IActionResult> OnGetCallbackAsync(string returnUrl = null, string remoteError = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            if (remoteError != null)
            {
                Message = $"Error from external provider: {remoteError}";
                return Page();
            }

            var info = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            if (info.Principal == null)
            {
                return RedirectToPage("./Login");
            }

            // Xác thực thành công với Google
            var claims = info.Principal.Identities.FirstOrDefault().Claims;
            var email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

            // Call API để đăng nhập hoặc đăng ký người dùng
            var apiResponse = await CallApiAsync(email);

            if (apiResponse != null)
            {
                var responseContent = await apiResponse.Content.ReadAsStringAsync();
                var responseObject = JsonSerializer.Deserialize<ResponseModel>(responseContent);
                Message = responseObject.Message;

                // Xử lý kết quả từ API (ví dụ: tạo JWT token hoặc lưu thông tin người dùng)
            }
            else
            {
                Message = "Error calling API.";
            }

            return Page();
        }

        private async Task<HttpResponseMessage> CallApiAsync(string email)
        {
            var client = _httpClientFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:44329/api/Auth/GoogleLogin");

            // Gửi email đến API
            var content = JsonSerializer.Serialize(new { Email = email });
            request.Content = new StringContent(content, System.Text.Encoding.UTF8, "application/json");

            return await client.SendAsync(request);
        }
    }

    public class ResponseModel
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        // Các thuộc tính khác nếu cần
    }
}
