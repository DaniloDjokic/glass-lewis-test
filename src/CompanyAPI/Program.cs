using Application;
using Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

ApplicationServiceExtensions.ConfigureServices(builder.Services);
InfrastructureServiceExtensions.ConfigureServices(builder.Services);

var app = builder.Build();

app.MapControllers();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // app.MapOpenApi();
}

app.UseHttpsRedirection();
app.Run();
