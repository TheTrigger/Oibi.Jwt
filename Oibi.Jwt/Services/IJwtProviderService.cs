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