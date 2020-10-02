using System;

namespace Oibi.Jwt.Demo.Models
{
    public class DummyUser
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        /// <summary>
        /// Hashed password with <see cref="IJwtProviderService.HashPassword"/>
        /// </summary>
        public string Password { get; set; }
    }
}