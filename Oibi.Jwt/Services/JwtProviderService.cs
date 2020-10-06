using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Oibi.Jwt.Models.Configurations;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Oibi.Jwt.Services.AuthService
{
    public class JwtProviderService : IJwtProviderService
    {
        private readonly JwtSettings _jwtSettings;
        private readonly IHttpContextAccessor _httpContext;

        public JwtProviderService(
            IOptions<JwtSettings> jwtSettings,
            IHttpContextAccessor httpContext)
        {
            _jwtSettings = jwtSettings.Value;
            _httpContext = httpContext;
        }

        /// <inheritdoc/>
        public string GenerateToken(IEnumerable<Claim> claims) => GenerateToken(default, claims);

        /// <inheritdoc/>
        public string GenerateToken(ClaimsIdentity claimsIdentity) => GenerateToken(default, claimsIdentity);

        /// <inheritdoc/>
        public string GenerateToken(IDictionary<string, object> claims, IEnumerable<Claim> claimsIdentity)
        {
            return GenerateToken(claims, new ClaimsIdentity(claimsIdentity));
        }

        /// <inheritdoc/>
        public string GenerateToken(IDictionary<string, object> claims, ClaimsIdentity claimsIdentity)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Audience = _jwtSettings.Audience,
                Issuer = _jwtSettings.Issuer,
                IssuedAt = DateTime.UtcNow,
                NotBefore = DateTime.UtcNow,
                Expires = DateTime.UtcNow.Add(_jwtSettings.TokenDuration),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature),
                Claims = claims,
                Subject = claimsIdentity,
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        /// <inheritdoc/>
        public ClaimsPrincipal RetrieveUserClaims()
        {
            return _httpContext.HttpContext?.User;
        }

        /// <inheritdoc/>
        public JwtSecurityToken ValidateToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = _jwtSettings.ValidateIssuerSigningKey,
                    IssuerSigningKey = new SymmetricSecurityKey(_jwtSettings.SecretKey),
                    ValidateIssuer = _jwtSettings.ValidateIssuer,
                    ValidateAudience = _jwtSettings.ValidateAudience,
                    ValidateLifetime = _jwtSettings.ValidateLifetime,
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;

                return jwtToken;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}