using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Oibi.Jwt.Services.AuthService
{
    public interface IJwtProviderService
    {
        /// <summary>
        /// Generate authenticated jwt string
        /// </summary>
        /// <param name="claims"></param>
        /// <param name="claimsIdentity"></param>
        /// <returns>encoded jwt</returns>
        string GenerateToken(IDictionary<string, object> claims, IEnumerable<Claim> claimsIdentity);

        /// <summary>
        /// Generate authenticated jwt string
        /// </summary>
        /// <param name="claimsIdentity"></param>
        /// <returns>encoded jwt</returns>
        string GenerateToken(IEnumerable<Claim> claimsIdentity);

        /// <summary>
        /// Generate authenticated jwt string
        /// </summary>
        /// <param name="claimsIdentity"></param>
        /// <returns>encoded jwt</returns>
        string GenerateToken(ClaimsIdentity claimsIdentity);

        /// <summary>
        /// Generate authenticated jwt string
        /// </summary>
        /// <param name="claims"></param>
        /// <param name="claimsIdentity"></param>
        /// <returns>encoded jwt</returns>
        public string GenerateToken(IDictionary<string, object> claims, ClaimsIdentity claimsIdentity);

        /// <summary>
        /// Hash password
        /// </summary>
        /// <typeparam name="T">entity type</typeparam>
        /// <param name="entity">entity reference</param>
        /// <param name="password">plain password to hash</param>
        /// <returns>encrypted password</returns>
        string HashPassword<T>(T entity, string password) where T : class;

        /// <summary>
        /// Verify if plain <paramref name="providedPassword"/> is comparable to <paramref name="hashedPassword"/>
        /// </summary>
        /// <typeparam name="T">entity type</typeparam>
        /// <param name="entity">entity reference</param>
        /// <param name="hashedPassword">stored hashed password</param>
        /// <param name="providedPassword">plain password provided by user</param>
        /// <returns>Verification result</returns>
        PasswordVerificationResult VerifyHashedPassword<T>(T entity, string hashedPassword, string providedPassword) where T : class;

        /// <summary>
        /// Retrieve user's claims
        /// </summary>
        /// <returns></returns>
        ClaimsPrincipal RetrieveUserClaims();

        /// <summary>
        /// FYI, the validation is already triggered by <see cref="Microsoft.AspNetCore.Authorization.AuthorizeAttribute"/>
        /// </summary>
        JwtSecurityToken ValidateToken(string token);
    }
}