using Bookify.Api.Extensions;
using Bookify.Application;
using Bookify.Infrastructure;

namespace Bookify.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddApplication();
        builder.Services.AddInfrastructure(builder.Configuration);

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();

            app.ApplyMigrations();

            //app.SeedData();
        }

        app.UseHttpsRedirection();

        app.UseCustomExceptionHandler();

        app.MapControllers();

        app.Run();
    }
}
