using Microsoft.Extensions.Caching.Memory;
using MilkStore.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore.Service.Services
{
    public class TokenService : ITokenService
    {
        private readonly IMemoryCache _cache;

        public TokenService(IMemoryCache cache)
        {
            _cache = cache;
        }

        // Tạo token và lưu vào cache sau khi xác thực thành công của đăng ký và quên mật khẩu
        public string GenerateToken(string identifier, string method)
        {
            var token = Guid.NewGuid().ToString();
            _cache.Set($"{identifier}_{method}_reset_token", token, TimeSpan.FromMinutes(10)); // 10 phút hết hạn
            return token;
        }

        public bool ValidateToken(string identifier, string method, string token)
        {
            if (_cache.TryGetValue($"{identifier}_{method}_reset_token", out string? cachedToken))
            {
                return cachedToken == token;
            }
            return false;
        }
    }
}
