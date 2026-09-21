using Bookify.Application.Abstractions.Behaviors;
using Bookify.Domain.Bookings;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace Bookify.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddMediatR(mConfig =>
        {
            mConfig.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);

            mConfig.AddOpenBehavior(typeof(LoggingBehavior<,>));

            mConfig.AddOpenBehavior(typeof(ValidationBehavior<,>));

            mConfig.LicenseKey = configuration["MediatR:LicenseKey"];
        });

        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

        services.AddTransient<PricingService>();

        return services;
    }
}
