﻿using BlazingChat.Server.Data.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BlazingChat.Server
{
    public class TokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public static TokenValidationParameters GetTokenValidationParameters(IConfiguration configuration) => new()
        {
            ValidateIssuer = true,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = configuration["Jwt:Issuer"],
            IssuerSigningKey = GetSecurityKey(configuration)
        };

        public string GenerateJwt(IEnumerable<Claim> additionalClaims = null)
        {
            var securityKey = GetSecurityKey(_configuration);
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var expirationMinutes = Convert.ToInt16(_configuration["Jwt:ExpireMinutes"] ?? "10");

            var claims = new List<Claim>() {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            if (additionalClaims?.Any() == true)
            {
                claims.AddRange(additionalClaims);
            }

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: "*",
                claims: claims,
                expires: DateTime.Now.AddMinutes(expirationMinutes),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateJwt(User user, IEnumerable<Claim> additionalClaims = null)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
            };

            if (additionalClaims?.Any() == true)
            {
                claims.AddRange(additionalClaims);
            }

            return GenerateJwt(claims);
        }

        public static SymmetricSecurityKey GetSecurityKey(IConfiguration _configuration) =>
                new(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
    }
}