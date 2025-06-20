using Application;
using Application.Extensions;
using CompanyApi;
using Infrastructure;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddHttpClient();
builder.Services.AddControllers();

OpenApiExtensions.AddOpenApi(builder.Services);

if (!builder.Environment.IsEnvironment("Testing"))
{
    AuthenticationExtensions.AddJwtAuthentication(builder.Services, builder.Configuration, builder.Environment);
}

ApplicationServiceExtensions.ConfigureServices(builder.Services);
InfrastructureServiceExtensions.ConfigureServices(builder.Services, builder.Configuration);

builder.Services.AddCors(options =>
{
    if (!builder.Environment.IsEnvironment("Testing"))
    {
        options.AddPolicy("AllowAngularApp",
            policy =>
            {
                policy.WithOrigins(builder.Configuration["Frontend:ClientUrl"] ?? throw new InvalidOperationException("ClientUrl is not configured."))
                   .AllowAnyHeader()
                   .AllowAnyMethod()
                   .AllowCredentials();
            });
    }
});

var app = builder.Build();

app.UseCors("AllowAngularApp");
OpenApiExtensions.MapOpenApi(app);

try
{
    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        context.Database.Migrate();
    }
    Log.Information("Database migration completed successfully.");
}
catch (System.Exception)
{
    Log.Error("An error occurred during database migration.");
}

app.UseExceptionHandler(err =>
{
    err.Run(async context =>
    {
        context.Response.StatusCode = 500;
        context.Response.ContentType = "application/json";
        var problemDetails = new
        {
            type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
            title = "An unexpected error occurred!",
            status = 500,
            detail = "An internal server error has occurred."
        };

        Log.Error("An unhandled exception occurred: {Message}", context.Features.Get<IExceptionHandlerFeature>()?.Error.Message);
        await context.Response.WriteAsJsonAsync(problemDetails);
    });
});

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.MapControllers();

app.Run();

public partial class Program { }
