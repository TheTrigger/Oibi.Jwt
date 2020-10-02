using System.ComponentModel.DataAnnotations;

namespace Oibi.Jwt.Demo.Models.Dto
{
    /// <summary>
    /// New user request
    /// </summary>
    public class RegistrationDto
    {
        /// <summary>
        /// Username ...
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Obv user's email
        /// </summary>
        [Required]
        public string Email { get; set; }

        /// <summary>
        /// Plain user's password
        /// </summary>
        [Required]
        public string Password { get; set; }
    }
}