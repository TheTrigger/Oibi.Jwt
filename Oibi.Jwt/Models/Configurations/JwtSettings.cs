using System;
using System.Text;
using System.Text.Json.Serialization;

namespace Oibi.Jwt.Models.Configurations
{
    public class JwtSettings
    {
        public const string SectionName = "Jwt";

        /// <summary>
        /// Gets or sets the <see cref="Microsoft.IdentityModel.Tokens.SecurityKey"/> that is to be used for signature validation.
        /// </summary>
        public string Secret { get; set; }

        /// <summary>
        /// Gets or sets a String that represents a valid issuer that will be used to check against the token’s issuer
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// Gets or sets a string that represents a valid audience that will be used to check against the token’s audience
        /// </summary>
        public string Audience { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the Issuer should be validated. True means Yes validation required
        /// </summary>
        public bool ValidateIssuer { get; set; } = false;

        /// <summary>
        /// Gets or sets a boolean to control if the audience will be validated during token validation
        /// </summary>
        public bool ValidateAudience { get; set; } = false;

        /// <summary>
        /// Gets or sets a boolean to control if the lifetime will be validated during token validation
        /// </summary>
        public bool ValidateLifetime { get; set; } = true;

        /// <summary>
        /// Gets or sets a boolean that controls if validation of the SecurityKey that signed the securityToken is called
        /// </summary>
        public bool ValidateIssuerSigningKey { get; set; } = true;

        /// <summary>
        /// Duration from <see cref="DateTime.UtcNow"/>.
        /// Default is 30days. Format is: [-]P[{days}D][T[{hours}H][{min}M][{sec}S]]
        /// </summary>
        public TimeSpan TokenDuration { get; set; } = new TimeSpan(days: 30, 0, 0, 0);

        /// <summary>
        /// Decoded <see cref="Secret"/>
        /// </summary>
        [JsonIgnore]
        public byte[] SecretKey => Encoding.ASCII.GetBytes(Secret);
    }
}