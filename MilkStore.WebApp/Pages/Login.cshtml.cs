using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using MilkStore.Domain.Entities;
using System.Security.Claims;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.EntityFrameworkCore;
using MilkStore.Repository.Data;
using System.Net.Http.Headers;
using System.IdentityModel.Tokens.Jwt;
using MilkStore.Domain.Enums;

namespace MilkStore.WebApp.Pages
{
    public class LoginModel : PageModel
    {
        private readonly HttpClient _httpClient;
        private readonly SignInManager<Account> _signInManager;
        private readonly UserManager<Account> _userManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly AppDbContext _context;

        [BindProperty]
        public string Username { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public LoginModel(HttpClient httpClient, SignInManager<Account> signInManager, UserManager<Account> userManager, ILogger<LoginModel> logger, AppDbContext context)
        {
            _httpClient = httpClient;
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
            _context = context;              
        }

        // Đăng nhập bằng Username/Password
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var response = await _httpClient.PostAsJsonAsync("https://localhost:44329/api/Auth/Login", new { Username, Password });

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadFromJsonAsync<LoginResponseModel>();
                // Giải mã AccessToken để lấy thông tin người dùng
                var handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadToken(content.AccessToken) as JwtSecurityToken;

                // Lấy thông tin người dùng từ AccessToken
                var userName = jsonToken?.Subject; // Subject chính là username của người dùng
                var email = jsonToken?.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
                var avatarUrl = jsonToken?.Claims.FirstOrDefault(c => c.Type == "avatar")?.Value;

                // Lưu thông tin người dùng vào Session
                HttpContext.Session.SetString("UserName", userName);
                HttpContext.Session.SetString("Email", email);
                HttpContext.Session.SetString("AvatarUrl", avatarUrl ?? "/default-avatar.png");

                // Lưu accessToken vào cookies
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

        public IActionResult OnGetGoogleLogin()
        {
            var properties = new AuthenticationProperties
            {
                RedirectUri = Url.Page("/Login", pageHandler: "GoogleCallback")
            };

            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        public async Task<IActionResult> OnGetGoogleCallbackAsync()
        {
            var authenticateResult = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            // Liệt kê tất cả các token và kiểm tra chúng
            //var tokens = authenticateResult.Properties.GetTokens();
            //foreach (var token in tokens)
            //{
            //    Console.WriteLine($"{token.Name}: {token.Value}");
            //}
            if (!authenticateResult.Succeeded)
            {
                return RedirectToPage("/Login");
            }

            var email = authenticateResult.Principal.FindFirstValue(ClaimTypes.Email);
            var firstName = authenticateResult.Principal.FindFirstValue(ClaimTypes.GivenName) ?? "";
            var lastName = authenticateResult.Principal.FindFirstValue(ClaimTypes.Surname) ?? "";
            var avatarUrl = authenticateResult.Principal.FindFirstValue("urn:google:picture");

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                var avatarImage = new Image
                {
                    ImageUrl = avatarUrl,
                    ThumbnailUrl = avatarUrl // Use the same URL for thumbnail if no separate URL is provided
                };

                _context.Images.Add(avatarImage);
                await _context.SaveChangesAsync();

                user = new Account
                {
                    UserName = email,
                    Email = email,
                    FirstName = firstName,
                    LastName = lastName,
                    CreatedAt = DateTime.UtcNow,
                    AvatarId = avatarImage.Id, // Link to the avatar image
                    Gender = GenderEnums.Unknown.ToString(), // Default value
                    Status = "Active",
                    TotalPoints = 0,
                    IsDeleted = false
                };

                var createResult = await _userManager.CreateAsync(user);
                if (createResult.Succeeded)
                {
                    await _userManager.AddLoginAsync(user, new UserLoginInfo("Google", authenticateResult.Principal.FindFirstValue(ClaimTypes.NameIdentifier), "Google"));
                    await _signInManager.SignInAsync(user, isPersistent: false);
                }
            }
            else
            {
                user.Avatar = await _context.Images.FindAsync(user.AvatarId);
                user.FirstName = firstName;
                user.LastName = lastName;

                // Update avatar if it's different
                if (user.Avatar != null && user.Avatar.ImageUrl != avatarUrl)
                {
                    user.Avatar.ImageUrl = avatarUrl;
                    user.Avatar.ThumbnailUrl = avatarUrl;
                    _context.Images.Update(user.Avatar);
                }

                await _userManager.UpdateAsync(user);
            }


                var googleToken = authenticateResult.Properties.GetTokenValue("access_token");

                if (!string.IsNullOrEmpty(googleToken))
                {
                    // Gọi Google API để lấy thông tin người dùng
                    var client = new HttpClient();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", googleToken);

                    var response = await client.GetAsync("https://www.googleapis.com/oauth2/v2/userinfo");
                    var content = await response.Content.ReadAsStringAsync();

                    ViewData["UserInfo"] = content;
                }
                
            

            // Store user information in session
            HttpContext.Session.SetString("UserName", $"{user.FirstName} {user.LastName}");
            HttpContext.Session.SetString("AvatarUrl", user.Avatar?.ImageUrl ?? "/default-avatar.png");
            //HttpContext.Session.SetString("GoogleToken", googleToken);

            //return Page();
            return RedirectToPage("/Index");
        }

        public async Task<IActionResult> OnPostLogoutAsync()
        {
            // Sign out of the cookie authentication
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Clear the session
            HttpContext.Session.Clear();

            // Redirect to the login page
            return RedirectToPage("/Login");
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
