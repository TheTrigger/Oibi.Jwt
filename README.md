# Jwt helper for ASP.NET Core

A basic service to implement jwt token. Use this in your auth service.

## How to use

```ps
Install-Package Oibi.Jwt
```

### Startup.cs
```csharp
private readonly IConfiguration _configuration;

public Startup(IConfiguration configuration)
{
    _configuration = configuration;
}

public void ConfigureServices(IServiceCollection services)
{
	services.AddJwt(_configuration);
}

```

### AuthService.cs
```csharp


```


## Default table

| Property                 | Default value | Description                                                                                                            |
|--------------------------|---------------|------------------------------------------------------------------------------------------------------------------------|
| Secret                   | null          | Gets or sets the <see cref="Microsoft.IdentityModel.Tokens.SecurityKey"/> that is to be used for signature validation. |
| Issuer                   | null          | Gets or sets a String that represents a valid issuer that will be used to check against the token’s issuer             |
| Audience                 | null          | Gets or sets a string that represents a valid audience that will be used to check against the token’s audience         |
| ValidateIssuer           | false         | Gets or sets a value indicating whether the Issuer should be validated. True means Yes validation required             |
| ValidateAudience         | false         | Gets or sets a boolean to control if the audience will be validated during token validation                            |
| ValidateLifetime         | true          | Gets or sets a boolean to control if the lifetime will be validated during token validation                            |
| ValidateIssuerSigningKey | true          | Gets or sets a boolean that controls if validation of the SecurityKey that signed the securityToken is called          |