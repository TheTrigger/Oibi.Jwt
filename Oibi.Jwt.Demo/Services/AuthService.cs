using Microsoft.AspNetCore.Identity;
using Oibi.Authentication.Models.Dto;
using Oibi.Authentication.Services;
using Oibi.Jwt.Demo.Models;
using Oibi.Jwt.Demo.Models.Dto;
using Oibi.Jwt.Services.AuthService;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Oibi.Jwt.Demo.Services
{
    /// <summary>
    /// I put authentication and registration together for brevity, idk how is your environment
    /// </summary>
    public class AuthService
    {
        private readonly IJwtProviderService _jwtProviderService;
        private readonly IHasherService _hasherService;

        /// <summary>
        /// Replace this with your users store.
        /// </summary>
        private readonly IDictionary<string, DummyUser> _dummyUsersService = new Dictionary<string, DummyUser>();

        public AuthService(IJwtProviderService jwtProviderService, IHasherService hasherService)
        {
            _jwtProviderService = jwtProviderService;
            _hasherService = hasherService;
        }

        /// <summary>
        /// Authenticate with user's email and password
        /// </summary>
        public async Task<LoginResponse> AuthenticateAsync(string email, string inputPassword)
        {
            await Task.Delay(0); // just because we dont have a remote(db) call here

            if (!_dummyUsersService.ContainsKey(email))
            {
                return new LoginResponse<GenericError>
                {
                    Success = false,
                    Data = new GenericError("‍User does not exists ...") // i don't like to give those informations ...
                };
            }

            var user = _dummyUsersService[email]; // get from your user storage
            if (_hasherService.VerifyHashedPassword(user, user.Password, inputPassword) != PasswordVerificationResult.Success)
            {
                return new LoginResponse<GenericError>
                {
                    Success = false,
                    Data = new GenericError("Invalid credentials 🤷‍")
                };
            }

            var userClaims = new[] {
                new Claim(type: "sub", user.Id.ToString(), ClaimValueTypes.String),
                new Claim(type: ClaimTypes.Name, user.Name, ClaimValueTypes.String),
                new Claim(type: ClaimTypes.Email, user.Email, ClaimValueTypes.Email),
                new Claim(type: ClaimTypes.Role, "user's role"),
            };

            user.Password = null; // Make your own UserDto without password property! This is just a demo project.
            var dto = new LoginResponse<DummyUser>
            {
                Success = true,
                Data = user,
                Token = _jwtProviderService.GenerateToken(userClaims),
            };

            return dto;
        }

        /// <summary>
        /// Super-dummy registration
        /// </summary>
        public async Task RegistrationAsync(RegistrationDto registration)
        {
            await Task.Delay(0); // just because we dont have a remote(db) call here

            var user = new DummyUser
            {
                Name = registration.Name,
                Email = registration.Email,
            };

            user.Password = _hasherService.HashPassword(user, registration.Password);

            _dummyUsersService[registration.Email] = user;

            // you can return your own error messages and/or call AuthenticateAsync
        }
    }
}