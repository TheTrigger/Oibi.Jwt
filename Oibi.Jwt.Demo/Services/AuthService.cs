using Microsoft.AspNetCore.Identity;
using Oibi.Jwt.Demo.Models;
using Oibi.Jwt.Demo.Models.Dto;
using Oibi.Jwt.Services.AuthService;
using System;
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

        /// <summary>
        /// Replace this with your users store.
        /// </summary>
        private readonly IDictionary<string, DummyUser> _dummyUsersService = new Dictionary<string, DummyUser>();

        public AuthService(IJwtProviderService jwtProviderService)
        {
            _jwtProviderService = jwtProviderService;
        }

        /// <summary>
        /// Authenticate with user's email and password
        /// </summary>
        public async Task<LoginResponse> AuthenticateAsync(string email, string inputPassword)
        {
            await Task.Delay(0); // just because we dont have a remote(db) call here

            if (!_dummyUsersService.ContainsKey(email))
            {
                return new LoginResponse<YourErrorDto>
                {
                    Success = false,
                    Data = new YourErrorDto("‍User does not exists ...") // btw i don't like to give those informations
                };
            }

            var user = _dummyUsersService[email]; // get from your user storage
            if (_jwtProviderService.VerifyHashedPassword(user, user.Password, inputPassword) != PasswordVerificationResult.Success)
            {
                // someone doenst agree to use exceptions for excpected exceptions...
                // so i did LoginResponseDto<T> to returns Success = false

                //throw new UnauthorizedAccessException("Invalid credentials 🤷‍");

                return new LoginResponse<YourErrorDto>
                {
                    Success = false,
                    Data = new YourErrorDto("Invalid credentials 🤷‍")
                };
            }

            var userClaims = new[] {
                new Claim(type: "sub", user.Id.ToString()),
                new Claim(type: ClaimTypes.Name, user.Name),
                new Claim(type: ClaimTypes.Email, user.Email),
                new Claim(type: ClaimTypes.Role, "user's role"),
            };

            var dictionaryClaims = new Dictionary<string, object>
            {
                { "wtf", Guid.NewGuid() },
                { "test", DateTime.Now }
            };

            var dto = new LoginResponse<DummyUser>
            {
                Success = true,
                Data = user,
                Token = _jwtProviderService.GenerateToken(dictionaryClaims, userClaims),
            };

            return dto;
        }

        /// <summary>
        /// Super-dummy registration
        /// </summary>
        /// <param name="registration"></param>
        /// <returns></returns>
        public async Task RegistrationAsync(RegistrationDto registration)
        {
            await Task.Delay(0); // just because we dont have a remote(db) call here

            var user = new DummyUser
            {
                Name = registration.Name,
                Email = registration.Email,
            };

            user.Password = _jwtProviderService.HashPassword(user, registration.Password);

            _dummyUsersService[registration.Email] = user;

            // you can return your own error messages and/or call AuthenticateAsync
        }
    }
}