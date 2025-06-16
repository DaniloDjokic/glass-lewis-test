using AuthServer;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

AuthServerInitializer.InitializeAuthStore(builder.Services, builder.Configuration);

builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

var app = builder.Build();

AuthServerInitializer.InitializeAuthData(app.Services, app.Configuration);

if (app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseIdentityServer();
app.MapGet("/", () => "IdentityServer is running!");

app.Run();

