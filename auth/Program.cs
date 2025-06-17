using AuthServer;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

AuthServerInitializer.InitializeAuthStore(builder.Services, builder.Configuration);

builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp",
        policy =>
        {
            policy.WithOrigins(builder.Configuration["Frontend:ClientUrl"] ?? throw new ArgumentNullException("ClientUrl is not configured."))
               .AllowAnyHeader()
               .AllowAnyMethod()
               .AllowCredentials();
        });
});

var app = builder.Build();

app.UseCors("AllowAngularApp");

AuthServerInitializer.InitializeAuthData(app.Services, app.Configuration);

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseIdentityServer();
app.MapGet("/", () => "IdentityServer is running!");

app.Run();

