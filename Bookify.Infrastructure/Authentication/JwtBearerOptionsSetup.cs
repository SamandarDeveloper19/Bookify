using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;

namespace Bookify.Infrastructure.Authentication;

internal sealed class JwtBearerOptionsSetup : IConfigureNamedOptions<JwtBearerOptions>
{
    private readonly AuthenticationOptions authenticationOptions;

    public JwtBearerOptionsSetup(AuthenticationOptions authenticationOptions)
    {
        this.authenticationOptions = authenticationOptions;
    }

    public void Configure(JwtBearerOptions options)
    {
        options.Audience = authenticationOptions.Audience;
        options.MetadataAddress = authenticationOptions.MetadataUrl;
        options.RequireHttpsMetadata = authenticationOptions.RequireHttpsMetadata;
        options.TokenValidationParameters.ValidIssuer = authenticationOptions.ValidIssuer;
    }

    public void Configure(string? name, JwtBearerOptions options)
    {
        Configure(options);
    }
}
