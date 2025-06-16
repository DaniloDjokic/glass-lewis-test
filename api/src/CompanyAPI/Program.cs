using Application;
using CompanyApi;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddHttpClient();
builder.Services.AddControllers();
builder.Services.AddOpenApi();

AuthenticationExtensions.AddJwtAuthentication(builder.Services, builder.Configuration, builder.Environment);

ApplicationServiceExtensions.ConfigureServices(builder.Services);
InfrastructureServiceExtensions.ConfigureServices(builder.Services, builder.Configuration);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp",
        policy =>
        {
            policy.WithOrigins(builder.Configuration["ClientUrl"] ?? throw new InvalidOperationException("ClientUrl is not configured."))
               .AllowAnyHeader()
               .AllowAnyMethod()
               .AllowCredentials();
        });
});

var app = builder.Build();

app.UseCors("AllowAngularApp");

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

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
