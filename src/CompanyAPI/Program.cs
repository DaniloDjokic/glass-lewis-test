using Application;
using Infrastructure;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddControllers();
builder.Services.AddOpenApi();

ApplicationServiceExtensions.ConfigureServices(builder.Services);
InfrastructureServiceExtensions.ConfigureServices(builder.Services, builder.Configuration);

var app = builder.Build();

app.MapControllers();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
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

app.UseHttpsRedirection();
app.Run();
