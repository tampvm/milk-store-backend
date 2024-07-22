using Microsoft.IdentityModel.Tokens;
using MilkStore.Domain.Entities;
using MilkStore.Service.Common;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore.Service.Utils
{
    public static class GenerateJsonWebTokenString
    {
        public static string GenerateJsonWebToken(this Account user, JWTSettings jwt, IList<string> roles)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.JWTSecretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Name, user.UserName),
                new Claim(ClaimTypes.Sid, user.Id.ToString()),             
            };

            // Thêm các claim về vai trò
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var token = new JwtSecurityToken(
                issuer: jwt.Issuer,
                audience: jwt.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(jwt.AccessTokenDurationInMinutes),
                signingCredentials: credentials);


            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        //public static string GenerateJsonWebToken(this Account account, JWTSetting jwt, DateTime now)
        //{
        //    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.JWTSecretKey));
        //    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        //    var claims = new[]
        //    {
        //            new Claim(ClaimTypes.Name, account.Namee),
        //            new Claim(ClaimTypes.Username, account.Emaill),
        //            new Claim(ClaimTypes.Role, account.Rolee),
        //            new Claim("UserId", account.AccountName),
        //            new Claim("Password", account.Passwordd)
        //        };

        //    var token = new JwtSecurityToken(
        //        issuer: "TestJWT",
        //        claims: claims,
        //        expires: DateTime.UtcNow.AddMinutes(5),
        //        signingCredentials: credentials);


        //    return new JwtSecurityTokenHandler().WriteToken(token);
        //}
    }
}
