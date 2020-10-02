using Oibi.Jwt.Demo;
using Oibi.Jwt.Demo.Models;
using Oibi.Jwt.Demo.Models.Dto;
using Oibi.Jwt.Demo.Services;
using Oibi.TestHelper;
using System.Threading.Tasks;
using Xunit;

namespace Oibi.Jwt.XunitTest
{
    public class JwtTests : IClassFixture<ServerFixture<Startup>>
    {
        private readonly ServerFixture<Startup> _serverFixture;
        private readonly AuthService _authService;

        public JwtTests(ServerFixture<Startup> serverFixture)
        {
            _serverFixture = serverFixture;
            _authService = _serverFixture.GetService<AuthService>();
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

            Assert.IsType<LoginResponse<YourErrorDto>>(result);
        }
    }
}