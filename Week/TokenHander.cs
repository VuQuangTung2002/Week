using Azure.Core;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Week.Models;

namespace Week
{
    public class TokenHander
    {
        private readonly IConfiguration _configuration;
        public MyDbConnect db;
        public Task ValidateToken(TokenValidatedContext context)
        {
            // Kiểm tra nếu không có Claim nào
            var claims = context.Principal.Claims.ToList();
            if (claims.Count == 0)
            {
                context.Fail("Không có Claim nào tồn tại");
                return Task.CompletedTask;
            }

            var claimsIdentity = context.Principal.Identity as ClaimsIdentity;
            if (claimsIdentity != null)
            {
                // Kiểm tra Role Claim
                var roleClaim = claimsIdentity.FindFirst(ClaimTypes.Role);
                if (roleClaim == null || string.IsNullOrEmpty(roleClaim.Value))
                {
                    context.Fail("Thiếu vai trò");
                    return Task.CompletedTask;
                }

                // Kiểm tra Email Claim
                var userEmail = claimsIdentity.FindFirst(ClaimTypes.Email);
                if (userEmail == null || string.IsNullOrEmpty(userEmail.Value))
                {
                    context.Fail("Thiếu email");
                    return Task.CompletedTask;
                }

                // Kiểm tra Username Claim
                var usernameClaim = claimsIdentity.FindFirst(ClaimTypes.Name);
                if (usernameClaim == null || string.IsNullOrEmpty(usernameClaim.Value))
                {
                    context.Fail("Thiếu Username");
                    return Task.CompletedTask;
                }

                // Kiểm tra thời gian hết hạn của Token
                var expClaim = claimsIdentity.FindFirst(JwtRegisteredClaimNames.Exp);
                if (expClaim != null)
                {
                    var ticks = long.Parse(expClaim.Value);
                    var expiryDate = DateTimeOffset.FromUnixTimeSeconds(ticks).UtcDateTime;

                    if (expiryDate < DateTime.UtcNow)
                    {
                        context.Fail("Token đã hết hạn");
                        return Task.CompletedTask;
                    }
                }
            }

            return Task.CompletedTask;
        }
       

       
    }
}
