using Microsoft.Extensions.Options;
using Oibi.Authentication.Models.Dto;
using Oibi.Jwt.Demo;
using Oibi.Jwt.Demo.Models;
using Oibi.Jwt.Demo.Models.Dto;
using Oibi.Jwt.Demo.Services;
using Oibi.Jwt.Models.Configurations;
using Oibi.Jwt.Services.AuthService;
using Oibi.TestHelper;
using System.Threading.Tasks;
using Xunit;

namespace Oibi.Jwt.XunitTest
{
    public class JwtTests : IClassFixture<ServerFixture<Startup>>
    {
        private readonly ServerFixture<Startup> _serverFixture;
        private readonly AuthService _authService;
        private readonly IJwtProviderService _jwtProviderService;
        private readonly JwtSettings _jwtSettings;

        public JwtTests(ServerFixture<Startup> serverFixture)
        {
            _serverFixture = serverFixture;
            _authService = _serverFixture.GetService<AuthService>();
            _jwtProviderService = _serverFixture.GetService<IJwtProviderService>();
            _jwtSettings = _serverFixture.GetService<IOptions<JwtSettings>>().Value;
        }

        [Fact]
        public void VerifyJwtSettings()
        {
            Assert.NotEmpty(_jwtSettings.Audience);
            Assert.NotEmpty(_jwtSettings.Issuer);
            Assert.NotEmpty(_jwtSettings.Secret);
            Assert.NotEmpty(_jwtSettings.SecretKey);
            Assert.NotEqual(default, _jwtSettings.TokenDuration);
        }

        [Fact]
        public async Task RegistrationAndLogin()
        {
            var dto = new RegistrationDto
            {
                Email = "foo@example",
                Name = "Bar",
                Password = "my secret plain password"
            };

            await _authService.RegistrationAsync(dto);

            var result = await _authService.AuthenticateAsync(dto.Email, dto.Password);

            Assert.NotNull(result);
            Assert.True(result.Success);
            Assert.NotEmpty(result.Token);

            var securityToken = _jwtProviderService.ValidateToken(result.Token);

            Assert.IsType<LoginResponse<DummyUser>>(result);

            var reponse = result as LoginResponse<DummyUser>;
            Assert.Equal(dto.Email, reponse.Data.Email);
        }

        [Fact]
        public async Task WrongLogin()
        {
            var result = await _authService.AuthenticateAsync("wrong@example", "wrong too");

            Assert.NotNull(result);
            Assert.False(result.Success);

            Assert.IsType<LoginResponse<GenericError>>(result);
        }
    }
}